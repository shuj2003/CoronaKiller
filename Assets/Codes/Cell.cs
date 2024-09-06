using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Common
{
    [Header(" # Move Info ")]
    public Vector2 direction;
    public float speed;

    [Header(" # Object Info ")]
    private Rigidbody2D rigid;
    private SpriteRenderer sp;
    private float life = 1f;
    private Color colorFrom = new Color(1f, 1f, 1f, 1f);
    private Color colorTo = new Color(69f / 255f, 1f, 84f / 255f, 1f);

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {

        direction = direction.normalized;

    }

    private void FixedUpdate()
    {

        Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        if (GetComponentsInChildren<Corona>().Length > 0 && life > 0f)
        {

            life -= 0.5f * Time.fixedDeltaTime;
            if(life < 0f) life = 0f;
            sp.color = (colorFrom - colorTo) * life + colorTo;

        }

        if(life == 0f)
        {
            Dead();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject == GameManager.instance.endArea.gameObject)
        {

            Reseting();

        }

    }

    public void Reseting()
    {
        Corona[] objs = GetComponentsInChildren<Corona>();
        foreach (Corona obj in objs)
        {
            obj.Reseting();
        }

        gameObject.SetActive(false);
    }

    public void Init()
    {
        life = 1f;
        sp.color = colorFrom;
    }

    public void Dead()
    {
        Reseting();
    }

}
