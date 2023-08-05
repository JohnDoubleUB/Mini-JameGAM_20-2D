using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerAI : ResetableEntity
{
    //Note: This class will be slightly different for the different type of game controllers, it might be worth making a parent class or interface so that some of the class structure is consistent throughout.
    [Header("Movement")]
    public float movementSpeed = 1;

    [Header("Jumping")]
    public float jumpVelocity = 2;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int maxJumpCount = 1;

    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private CollisionNotifier collisionNotifier;
    [SerializeField]
    private CollisionNotifier headCollisionNotifier;
    [SerializeField]
    private GameObject[] OtherObjectsToDisable = new GameObject[0];

    private Transform parentedPlatform;

    private bool isMoving;
    [SerializeField]
    private bool isJumping;

    private Vector3 initialPosition;

    [HideInInspector]
    public bool Crouch;
    [HideInInspector]
    public bool JumpHold;

    protected bool isAlive = true;
    public bool IsAlive => isAlive;

    public bool IsJumping => isJumping;

    //Multipliers just to make the initial values more reasonable
    private float movementSpeedMultiplier = 300f;
    private float jumpVelocityMultiplier = 5f;

    public int currentJumpCount; //To keep track of the amount of jumps since last standing on the ground

    private void Start()
    {
        initialPosition = transform.position;
    }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionNotifier.OnNotifyCollisionEnter += OnGroundCollideEnter;
        collisionNotifier.OnNotifyCollisionExit += OnGroundCollideExit;
        headCollisionNotifier.OnNotifyCollisionEnter += OnHeadCollideEnter;
        headCollisionNotifier.OnNotifyCollisionExit += OnHeadCollideExit;

    }
    protected new void Update()
    {
        JumpGravityScript();
    }

    protected void FixedUpdate()
    {
        UpdateAnimator();
    }

    private void OnGroundCollideEnter(Collision2D collision)
    {
        currentJumpCount = 0;
        isJumping = false;

        if (collision.WasWithPlatform())
        {
            print("Ground enter");
            parentedPlatform = collision.transform;
            transform.SetParent(parentedPlatform);
        }
    }

    private void OnGroundCollideExit(Collision2D collision)
    {
        if (collision.WasWithPlatform() && collision.transform == parentedPlatform)
        {
            print("Ground exit");
            parentedPlatform = null;
            transform.SetParent(parentedPlatform);
        }
    }

    private void OnHeadCollideEnter(Collision2D collision)
    {
        if (collision.WasWithPlayer()) 
        {
            SetKilled(true);
            GameManager.current.CurrentPlayer.Jump(false);
        }
    }

    private void OnHeadCollideExit(Collision2D collision)
    {

    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsDead", !IsAlive);
        }
    }

    private void JumpGravityScript()
    {
        //Artificially increase the velocity on downward arch of jump, also changes jump height based on length of jump button held
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !JumpHold)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Jump()
    {
        //trigger jumping but only when the player can jump I.e. has not reached the max jumps
        if (currentJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(0, jumpVelocity * jumpVelocityMultiplier);
            currentJumpCount++;
            isJumping = true;
        }
    }

    public void Move(Vector2 movement)
    {
        //if(movement.x != 0f) Debug.Log("move");
        //Do the movement thing
        Vector2 targetVelocity = movement * movementSpeed * movementSpeedMultiplier * Time.deltaTime;

        //Check whether targetVelocity includes y speed, if so then check if we can climb
        isMoving = targetVelocity.x != 0f;

        //Set sprite direction
        if (movement.x != 0)
        {
            bool directionIsNegative = movement.x < 0f;
            sr.flipX = !directionIsNegative;
        }

        rb.velocity = new Vector2(targetVelocity.x, rb.velocity.y);
    }

    private void OnDestroy()
    {
        collisionNotifier.OnNotifyCollisionEnter -= OnGroundCollideEnter;
        collisionNotifier.OnNotifyCollisionExit -= OnGroundCollideExit;
        headCollisionNotifier.OnNotifyCollisionEnter -= OnHeadCollideEnter;
        headCollisionNotifier.OnNotifyCollisionExit -= OnHeadCollideExit;
    }

    private void SetKilled(bool killed) 
    {
        if (killed) 
        {
            //Killed animation
            collisionNotifier.gameObject.SetActive(false);
            headCollisionNotifier.gameObject.SetActive(false);
            boxCollider.enabled = false;
            rb.isKinematic = true;
            isAlive = false;

            foreach (GameObject obj in OtherObjectsToDisable) obj.SetActive(false);
        }
        else
        {
            //reset
            collisionNotifier.gameObject.SetActive(true);
            headCollisionNotifier.gameObject.SetActive(true);
            boxCollider.enabled = true;
            rb.isKinematic = false;
            isAlive = true;
            foreach (GameObject obj in OtherObjectsToDisable) obj.SetActive(true);
        }
    }

    public override void EntityReset()
    {
        rb.position = initialPosition;
        SetKilled(false);
    }
}
