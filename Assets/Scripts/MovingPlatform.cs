using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform
{
    public float movementLimit = 2.5f;
    public float speed = 2.0f;
    public bool invert;
    
    
    private int direction = 1;
    private float min = 2f;
    private float max = 3f;

    private int initialDirection;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (invert) direction = -1;

        min = transform.position.x - movementLimit;
        max = transform.position.x + movementLimit;

        initialDirection = direction;
    }


    // Update is called once per frame
    void Update()
    {
        if (EntityActive == false) return;

        if (direction > 0)
        {
            if (transform.position.x > max)
            {
                direction = -1;
            }
        }
        else 
        {
            if (transform.position.x < min)
            {
                direction = 1;
            }
        }


        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
    }

    public override void EntityReset()
    {
        base.EntityReset();
        direction = initialDirection;
    }
}
