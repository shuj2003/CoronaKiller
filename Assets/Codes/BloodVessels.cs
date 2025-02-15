using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodVessels : Common
{
    private BoxCollider2D[] colliders;

    public static float WallHeight = 1.6f;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 size = GetComponent<SpriteRenderer>().bounds.size;

        colliders[0].offset = new Vector2(0f, +(size.y - WallHeight) / 2f);
        colliders[0].size = new Vector2(size.x, WallHeight);

        colliders[1].offset = new Vector2(0f, -(size.y - WallHeight) / 2f);
        colliders[1].size = new Vector2(size.x, WallHeight);
    }

    private void FixedUpdate()
    {
        
    }
}
