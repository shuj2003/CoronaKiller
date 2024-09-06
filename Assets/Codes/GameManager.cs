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

        int layerComponent = LayerMask.NameToLayer("Component");
        int layerBloodVessels = LayerMask.NameToLayer("BloodVessels");
        int layerCell = LayerMask.NameToLayer("Cell");
        int layerMedicine = LayerMask.NameToLayer("Medicine");

        //Physics2D.IgnoreLayerCollision(layerComponent, layerBloodVessels);
        //Physics2D.IgnoreLayerCollision(layerComponent, layerCell);
        //Physics2D.IgnoreLayerCollision(layerComponent, layerMedicine);

    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime >= 2f)
        {
            CreateCell();
            CreateCorona();
            gameTime = 0f;
        }
    }

    public GameObject CreateCell()
    {
        GameObject cell = pool.Get(Random.Range(0,10));
        
        SpriteRenderer sp = cell.GetComponent<SpriteRenderer>();

        sp.sortingOrder = 2;

        Vector2 pos = Vector2.zero;

        Bounds bounds = startArea.GetComponent<BoxCollider2D>().bounds;

        pos = new Vector2(bounds.center.x, Random.Range(bounds.min.y, bounds.max.y));
        cell.transform.position = pos;

        cell.GetComponent<Cell>().direction = Vector2.left;
        cell.GetComponent<Cell>().speed = Random.Range(0.5f, 1f);

        cell.GetComponent<Cell>().Init();

        return cell;

    }

    public GameObject CreateBullet(int id)
    {
        GameObject bullet = pool.Get(id);

        SpriteRenderer sp = bullet.GetComponent<SpriteRenderer>();

        sp.sortingOrder = 3;
        
        Vector2 pos = Vector2.zero;
        pos += player.directionVec * 0.5f;
        bullet.transform.position = pos;

        return bullet;

    }

    public GameObject CreateCorona()
    {
        GameObject corona = pool.Get(Random.Range(11, 13));

        Corona corona1 = corona.GetComponent<Corona>();
        SpriteRenderer sp = corona.GetComponent<SpriteRenderer>();

        sp.sprite = corona1.sprites[Random.Range(0, corona1.sprites.Length)];
        sp.sortingOrder = 1;

        Vector2 pos = Vector2.zero;

        Bounds bounds = startArea.GetComponent<BoxCollider2D>().bounds;

        pos = new Vector2(bounds.center.x, Random.Range(bounds.min.y, bounds.max.y));
        corona.transform.position = pos;

        corona.GetComponent<Corona>().direction = Vector2.left;
        corona.GetComponent<Corona>().speed = Random.Range(1f, 2f);

        return corona;

    }

}
