using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ResetableEntity
{
    private Vector3 initialPosition;

    protected void Start() 
    {
        initialPosition = transform.position;
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    print("collisioon " + collision.gameObject.name);
    //    print("collision " + collision.gameObject.layer);
    //    if (collision.WasWithPlayer()) 
    //    {
            
    //        GameManager.current.CurrentPlayer.transform.SetParent(transform);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.WasWithPlayer())
    //    {
    //        GameManager.current.CurrentPlayer.transform.SetParent(null);
    //    }
    //}

    public override void EntityReset()
    {
        transform.position = initialPosition;
    }
}
