using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : ResetableEntity
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private GameObject keyObject;
    
    public string UniqueID;

    public override void EntityReset()
    {
        keyObject.SetActive(true);
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.WasWithPlayer())
        {
            AudioManager.current.AK_PlayClipOnObject("PlayPickUp", gameObject);
            keyObject.SetActive(false);
            boxCollider.enabled = false;
            GameManager.current.AddKey(UniqueID);
        }
    }
}
