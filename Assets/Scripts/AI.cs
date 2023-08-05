using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : ResetableEntity
{
    private Vector3 initialPosition;

    public Transform targetLocation;

    [HideInInspector]
    public bool Crouch;
    [HideInInspector]
    public bool JumpHold;

    protected bool isAlive = true;
    public bool IsAlive => isAlive;

    public bool CanControl = true;

    public virtual void Jump() { }
    public virtual void Interact() { }
    public virtual void Move(Vector2 movement) { }
    public virtual void OnFootFall() { }

    protected void Start()
    {
        initialPosition = transform.position;
    }

    protected void Update()
    {
    }

    protected void FixedUpdate()
    {
        if (IsAlive == false) return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(targetLocation.position.x, currentPosition.y);
        float distanceFromTarget = Vector2.Distance(currentPosition, targetPosition);
        if (distanceFromTarget > 0.2f)
        {
            Vector2 direction = targetPosition - currentPosition;

            direction.Normalize();
            print(direction);
            Move(direction * 30 * Time.deltaTime);

        }
        else 
        {
            Move(Vector2.zero);
        }
    }

    public virtual void Kill()
    {
        isAlive = false;
    }

    public void MakeAlive()
    {
        isAlive = true;
    }

    public virtual void ResetPlayer()
    {
        isAlive = true;
        transform.position = initialPosition;
    }

    public override void EntityReset()
    {

    }
}
