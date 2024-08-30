using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [Header(" # Weapon Info ")]
    public float power;
    public float speed;
    public Vector2 directionVec;

    [Header(" # Object Info ")]
    private Rigidbody2D rigid;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        Vector2 nextVec = directionVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject == GameManager.instance.endArea.gameObject)
        {
            gameObject.SetActive(false);
        }

    }

}
