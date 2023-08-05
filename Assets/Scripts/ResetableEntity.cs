using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResetableEntity : MonoBehaviour
{
    private bool entityActive;
    public bool EntityActive => entityActive;
    public abstract void EntityReset();
    public void EntityStart() 
    {
        entityActive = true;
    }
    public void EntityStop() 
    {
        entityActive = false;
    }
}
