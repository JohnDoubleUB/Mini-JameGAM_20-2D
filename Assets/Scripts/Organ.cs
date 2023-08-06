using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Organ : ResetableEntity
{
    public static Organ current;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool PlayerInRange;

    [SerializeField]
    private Text interactionText;

    public string PlaceKeyPrompt;
    public string PlayOrganPrompt;
    public string ContinueToNextLevelPrompt;

    private bool pianoOpen;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

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
        if (pianoOpen)
        {
            PlayerInRange = true;
            interactionText.enabled = true;
            if (Input.GetButtonDown("Interact"))
            {
                GameManager.current.ContinueToNextLevel();
            }
        }
        else if (PlayerInRange && GameManager.current.CurrentPlayer.IsAlive)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (GameManager.current.KeysPlaced == GameManager.current.KeysNeeded)
                {
                    GameManager.current.CompleteLevel(transform.position);
                }
                else
                {

                    animator.Play(GameManager.current.KeysFound == 0 ? "InteractBad" : "InteractGood");


                    if (GameManager.current.PlaceHeldKeys())
                    {
                        interactionText.text = PlayOrganPrompt;

                    }
                }
            }
        }


    }

    public void SetPianoDoorOpen(bool open)
    {
        animator.Play(open ? "Open" : "Idle", 0);
    }

    protected void OnPianoOpened()
    {
        pianoOpen = true;
        interactionText.text = ContinueToNextLevelPrompt;
    }
}
