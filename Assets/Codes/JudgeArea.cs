using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeArea : MonoBehaviour
{
    public enum JudgeAreaType
    {
        START,
        END,
    }
    public JudgeAreaType areaType = JudgeAreaType.START;

    private BoxCollider2D box;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

        Vector2 LB = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 RU = Camera.main.ViewportToWorldPoint(Vector2.one);

        float width = RU.x - LB.x;
        float height = RU.y - LB.y;

        switch (areaType)
        {
            case JudgeAreaType.START:
                {
                    box.size = new Vector2(2f, height - BloodVessels.WallHeight * 2f);
                    box.offset = new Vector2((width + box.size.x) / 2f + 1f, 0f);
                }
                break;
            case JudgeAreaType.END:
                {
                    box.size = new Vector2(width * 1.5f, height * 1.5f);
                    box.offset = Vector2.zero;
                }
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
