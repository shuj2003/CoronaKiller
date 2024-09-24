using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.XR;
using static Corona;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Wbc : Common
{
    public enum ActionState
    {
        SEARCHING,
        AIMING,
        ERODING,
    }

    public AnimationCurve legCurve;
    public GameObject hand;

    [Header(" # Move Info ")]
    public Vector2 direction;
    private float speed;

    [Header(" # Game Info ")]
    public ActionState actionState = ActionState.SEARCHING;
    public Corona target;
    private Vector3 targetPosBase;

    [Header(" # Object Info ")]
    private Vector3 posBase;
    private Vector3 scaleBase;

    private float aniTime = 0f;
    private int setp = 0;
    private float[] setpTims = { 1f, 0.5f };

    private void Awake()
    {

        posBase = hand.transform.localPosition;
        scaleBase = hand.transform.localScale;

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || !target.isActiveAndEnabled)
        {
            Init();
        }

        if (target != null)
        {
            direction = target.transform.position - transform.position;
        }
        direction = direction.normalized;

    }

    private void FixedUpdate()
    {
        if (actionState != ActionState.ERODING)
        {
            Vector3 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
            transform.position = transform.position + nextVec;

            float angle = Quaternion.Angle(hand.transform.localRotation, Quaternion.LookRotation(Vector3.forward, direction));
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        if (actionState == ActionState.SEARCHING)
        {
            target = GetNearest<Corona>(10f);
            if (target != null)
            {
                actionState = ActionState.AIMING;
                speed *= 1.25f;
            }
        }
        else if (actionState == ActionState.AIMING)
        {

        }else if (actionState == ActionState.ERODING)
        {
            Predation(delegate () {
                target.GetComponent<Animator>().SetBool("Dead", true);
                Init();
            });
        }

    }

    public void Init()
    {
        actionState = ActionState.SEARCHING;
        target = null;
        speed = 1.5f;
        hand.transform.localScale = scaleBase;
        hand.transform.localPosition = posBase;
    }

    private void Predation(Action complation)
    {
        switch (setp)
        {
            case 0:
                {
                    Vector3 s = ScaleTransform(scaleBase, new Vector3(1.5f, 1f, 1f), aniTime, setpTims[setp], legCurve);
                    Vector3 p = ScaleTransform(posBase, posBase + new Vector3(0f, -0.31f, 0f), aniTime, setpTims[setp], legCurve);

                    hand.transform.localScale = s;
                    hand.transform.localPosition = p;

                    if (target != null)
                    {
                        Vector3 p2 = ScaleTransform(targetPosBase, Vector3.zero, aniTime, setpTims[setp], legCurve);
                        target.transform.localPosition = p2;
                    }

                    if (aniTime > setpTims[setp])
                    {
                        aniTime = 0f;
                        setp++;
                    }
                }
                break;
            case 1:
                {
                    Vector3 s = ScaleTransform(scaleBase, new Vector3(1.5f, 1f, 1f), setpTims[setp] - aniTime, setpTims[setp], legCurve);
                    Vector3 p = ScaleTransform(posBase, posBase + new Vector3(0f, -0.31f, 0f), setpTims[setp] - aniTime, setpTims[setp], legCurve);

                    hand.transform.localScale = s;
                    hand.transform.localPosition = p;

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
        if (collision.gameObject.GetComponent<Corona>() == target)
        {
            actionState = ActionState.ERODING;
            aniTime = 0f;
            setp = 0;
            speed = 0f;

            target.PredationFromWBC(this);
            targetPosBase = target.transform.localPosition;

        }
    }

}
