using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" # Game Control ")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;

    [Header(" # Game Object ")]
    public PoolManager pool;
    public JudgeArea startArea;
    public JudgeArea endArea;
    public Medicine player;

    private void Awake()
    {
        instance = this;

        gameTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime >= 2f)
        {
            createCell();
            gameTime = 0f;
        }
    }

    void createCell()
    {
        GameObject cell = pool.Get(Random.Range(0,10));
        cell.GetComponent<SpriteRenderer>().sortingOrder = 1;
        SpriteRenderer sp = cell.GetComponent<SpriteRenderer>();
        Vector2 pos = Vector2.zero;

        Bounds bounds = startArea.GetComponent<BoxCollider2D>().bounds;

        pos = new Vector2(bounds.center.x, Random.Range(bounds.min.y, bounds.max.y));
        cell.transform.position = pos;

        cell.GetComponent<Cell>().direction = Vector2.left;
        cell.GetComponent<Cell>().speed = Random.Range(1f, 2f);

    }

}
