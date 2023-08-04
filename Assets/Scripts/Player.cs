using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Player : MonoBehaviour
{
    private Vector3 initialPosition;

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
        initialPosition= transform.position;
    }

    protected void Update()
    {
        if (CanControl == false)
        {
            JumpHold = false;
            return;
        }

        if (Input.GetButton("Jump"))
        {
            JumpHold = true;
            Jump();
        }
        else
        {
            JumpHold = false;
        }

    }

    protected void FixedUpdate()
    {
        if (CanControl == false) return;
        Vector2 movementInput = GetMovementInput();
        Move(movementInput);
    }

    private Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
}