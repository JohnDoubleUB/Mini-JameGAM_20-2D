using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionNotifier : MonoBehaviour
{
    public delegate void NotifyCollision(Collision2D collision);

    public NotifyCollision OnNotifyCollisionEnter;
    public NotifyCollision OnNotifyCollisionExit;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnNotifyCollisionEnter?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnNotifyCollisionExit?.Invoke(collision);
    }



}
