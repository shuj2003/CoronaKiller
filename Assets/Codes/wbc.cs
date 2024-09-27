using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.XR;
using static Corona;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static Wbc;
using Unity.VisualScripting;
using UnityEngine.U2D.Animation;
using UnityEditor;
using System.Linq;

public class Wbc : Common
{
    public enum ActionState
    {
        SEARCHING,
        AIMING,
        ERODING,
    }

    public AnimationCurve legCurve;

    [Header(" # Move Info ")]
    public Vector2 direction;
    private float speed;

    [Header(" # Game Info ")]
    public ActionState actionState = ActionState.SEARCHING;
    public Corona target;
    private Vector3 targetPosBase;

    [Header(" # Hand Info ")]
    private List<UnityEngine.Transform> hands;
    private int handIndex;
    private Vector3 posBase;
    private Vector3 scaleBase;

    [Header(" # Object Info ")]
    private float aniTime = 0f;
    private int setp = 0;
    private float[] setpTims = { 0.6f, 0.9f };

    private SpriteSkin skin;

    private void Awake()
    {
        skin = GetComponent<SpriteSkin>();
        hands = skin.boneTransforms.ToList();
        hands.RemoveAt(0);

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (target == null || !target.isActiveAndEnabled)
        {
            //targetが決める　OR　非活性化　時にリセット
            Init();
        }

        switch (actionState)
        {
            case ActionState.SEARCHING://捜索
                {
                    target = GetNearest<Corona>(10f);
                    if (target != null)
                    {
                        actionState = ActionState.AIMING;
                        speed *= 1.2f;
                    }
                }
                break;
            case ActionState.AIMING://狙い
                {
                    target = GetNearest<Corona>(10f);

                    handIndex = 0;
                    UnityEngine.Transform hand = hands[handIndex];
                    float dis = (hand.position - target.transform.position).sqrMagnitude;

                    for (int i = 1 ; i < hands.Count ; i++ )
                    {
                        float dis2 = (hands[i].position - target.transform.position).sqrMagnitude;
                        if(dis2 < dis)
                        {
                            handIndex = i;
                            dis = dis2;
                        }
                    }
                    hand = hands[handIndex];

                    //targetあった場合、目標へほ法線を計算
                    direction = target.transform.position - transform.position;
                    direction = direction.normalized;

                    Vector3 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
                    transform.position = transform.position + nextVec;

                    float angle = Vector2.SignedAngle(hand.transform.position - transform.position, direction);
                    float nextAngle = speed * 100f * Time.fixedDeltaTime;
                    if (nextAngle > Math.Abs(angle)) nextAngle = Math.Abs(angle);
                    nextAngle = angle < 0f ? nextAngle * -1f : nextAngle;
                    transform.rotation = transform.rotation * Quaternion.AngleAxis(nextAngle, Vector3.forward);


                }
                break;
            case ActionState.ERODING://捕食
                {
                    Predation(delegate () {
                        target.GetComponent<Animator>().SetBool("Dead", true);
                        Init();
                    });
                }
                break;
        }

    }

    public void Init()
    {
        actionState = ActionState.SEARCHING;
        target = null;
        speed = 1.5f;

        handIndex = 0;
        posBase = Vector2.zero;
        scaleBase = Vector2.one;
    }

    private void Predation(Action complation)
    {
        switch (setp)
        {
            case 0:
                {
                    Vector3 s = ScaleTransform(scaleBase, new Vector3(1.5f, 1.5f, 1f), aniTime, setpTims[setp], legCurve);
                    Vector3 p = ScaleTransform(posBase, posBase * 2f, aniTime, setpTims[setp], legCurve);

                    hands[handIndex].localScale = s;
                    hands[handIndex].localPosition = p;

                    if (aniTime > setpTims[setp])
                    {
                        aniTime = 0f;
                        setp++;
                    }
                }
                break;
            case 1:
                {
                    Vector3 s = ScaleTransform(scaleBase, new Vector3(1.5f, 1.5f, 1f), setpTims[setp] - aniTime, setpTims[setp], legCurve);
                    Vector3 p = ScaleTransform(posBase, posBase * 2f, setpTims[setp] - aniTime, setpTims[setp], legCurve);

                    hands[handIndex].localScale = s;
                    hands[handIndex].localPosition = p;

                    Vector3 p2 = ScaleTransform(targetPosBase, Vector3.zero, aniTime, setpTims[setp], legCurve);
                    target.transform.localPosition = p2;

                    if (aniTime > setpTims[setp])
                    {
                        aniTime = 0f;
                        setp++;
                        if (complation != null) complation();
                    }
                }
                break;
        }

        aniTime += Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (actionState)
        {
            case ActionState.SEARCHING://捜索
                {
                }
                break;
            case ActionState.AIMING://狙い
                {
                    if (collision.gameObject.GetComponent<Corona>() == target)
                    {
                        actionState = ActionState.ERODING;
                        aniTime = 0f;
                        setp = 0;

                        handIndex = 0;
                        UnityEngine.Transform hand = hands[handIndex];
                        float dis = (hand.position - target.transform.position).sqrMagnitude;

                        for (int i = 1; i < hands.Count; i++)
                        {
                            float dis2 = (hands[i].position - target.transform.position).sqrMagnitude;
                            if (dis2 < dis)
                            {
                                handIndex = i;
                                dis = dis2;
                            }
                        }
                        hand = hands[handIndex];

                        posBase = hand.localPosition;
                        scaleBase = hand.localScale;

                        target.PredationFromWBC(this);
                        targetPosBase = target.transform.localPosition;

                    }

                }
                break;
            case ActionState.ERODING://捕食
                {
                }
                break;
        }
        
    }

}
