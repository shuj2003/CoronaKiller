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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == GameManager.instance.endArea.gameObject)
        {
            gameObject.SetActive(false);
        }

    }

}
