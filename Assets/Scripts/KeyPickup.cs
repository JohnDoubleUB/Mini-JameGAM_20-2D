using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : ResetableEntity
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private GameObject keyObject;

    public override void EntityReset()
    {
        keyObject.SetActive(true);
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.WasWithPlayer())
        {
            keyObject.SetActive(false);
            boxCollider.enabled = false;
            GameManager.current.AddKey();
        }
    }
}
