using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Corona : MonoBehaviour
{
    public Sprite[] sprites;

    public enum ActionState
    {
        SEARCHING,
        AIMING,
        ERODING,
    }

    [Header(" # Move Info ")]
    public Vector2 direction;
    public float speed;

    [Header(" # Game Info ")]
    public ActionState actionState = ActionState.SEARCHING;
    public Cell target;

    [Header(" # Object Info ")]
    private Rigidbody2D rigid;

    private Cell GetNearest()
    {
        Cell result = null;
        float diff = 1f;

        foreach (Cell target in GameManager.instance.pool.GetComponentsInChildren<Cell>())
        {
            float dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < diff)
            {
                diff = dis;
                result = target;
            }
        }

        return result;

    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actionState == ActionState.AIMING)
        {
            direction = target.transform.position - transform.position;
        }
        direction = direction.normalized;

    }

    private void FixedUpdate()
    {
        if(actionState != ActionState.ERODING)
        {
            Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
            if (rigid)
            {
                rigid.MovePosition(rigid.position + nextVec);
            }
            
            if(actionState == ActionState.SEARCHING)
            {
                target = GetNearest();
                if(target != null)
                {
                    actionState = ActionState.AIMING;
                    speed *= 1.25f;
                }
            }
            else if (actionState == ActionState.AIMING)
            {

            }
            
        }
        else
        {

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.instance.endArea.gameObject)
        {
            Reseting();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (actionState == ActionState.AIMING)
        {

            int layerCell = LayerMask.NameToLayer("Cell");

            if (collision.gameObject.layer == layerCell)
            {
                transform.parent = collision.gameObject.transform;
                Destroy(rigid);
            }

        }

    }

    public void Reseting()
    {
        gameObject.transform.parent = GameManager.instance.pool.transform;
        gameObject.SetActive(false);

        actionState = ActionState.SEARCHING;
        target = null;

        if (GetComponent<Rigidbody2D>() == null)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.bodyType = RigidbodyType2D.Dynamic;
        }

    }

}
