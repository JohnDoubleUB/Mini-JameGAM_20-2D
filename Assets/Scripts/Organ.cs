using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Organ : ResetableEntity
{
    [SerializeField]
    private bool PlayerInRange;

    [SerializeField]
    private Text interactionText;

    public string PlaceKeyPrompt;
    public string PlayOrganPrompt;

    private void Start()
    {
        interactionText.text = PlaceKeyPrompt;
    }

    public override void EntityReset()
    {
        interactionText.text = PlaceKeyPrompt;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.WasWithPlayer()) 
        {
            PlayerInRange = true;
            interactionText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.WasWithPlayer())
        {
            PlayerInRange = false;
            interactionText.enabled = false;
        }
    }

    private void Update()
    {
        if (PlayerInRange && GameManager.current.CurrentPlayer.IsAlive) 
        {
            if (Input.GetButtonDown("Interact")) 
            {
                if (GameManager.current.KeysPlaced == GameManager.current.KeysNeeded) 
                {
                    GameManager.current.CompleteLevel(transform.position);
                }
                else if (GameManager.current.PlaceHeldKeys())
                {
                    interactionText.text = PlayOrganPrompt;

                }
            }
        }


    }
}
