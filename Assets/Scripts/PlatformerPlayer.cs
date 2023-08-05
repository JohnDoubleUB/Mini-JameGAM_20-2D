using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerPlayer : Player
{
    //Note: This class will be slightly different for the different type of game controllers, it might be worth making a parent class or interface so that some of the class structure is consistent throughout.
    [Header("Movement")]
    public float movementSpeed = 1;

    [Header("Jumping")]
    public float jumpVelocity = 2;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int maxJumpCount = 1;

    [Header("Climbing")]
    public LayerMask ladderLayerMask;
    public Transform ladderRaycastTarget;
    public float raycastDistance; //This is to check for ladders

    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private CollisionNotifier collisionNotifier;

    private Transform parentedPlatform;

    private float bottomBoundsOffset;

    private Vector3 lastFramePosition;
    private float perFrameFallingDistance = 0.15f; //Distance moved per frame in y to be considered falling

    //State variables
    private bool isClimbing;
    private bool isMoving;
    [SerializeField]
    private bool isJumping;
    private bool isCrouching;
    private bool isPlayingOrgan;
    private bool isFalling;

    public bool IsJumping => isJumping;

    public Transform debug;
    private bool IsClimbing
    {
        get { return isClimbing; }
        set
        {
            isClimbing = value;

            if (isClimbing)
            {
                currentJumpCount = 0;
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }
        }
    }
    private bool canClimb;

    //Multipliers just to make the initial values more reasonable
    private float movementSpeedMultiplier = 300f;
    private float jumpVelocityMultiplier = 5f;

    public int currentJumpCount; //To keep track of the amount of jumps since last standing on the ground

    protected new void Start()
    {
        base.Start();

        bottomBoundsOffset = boxCollider.bounds.extents.y;
    }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastFramePosition = transform.position;
        collisionNotifier.OnNotifyCollisionEnter += OnGroundCollideEnter;
        collisionNotifier.OnNotifyCollisionExit += OnGroundCollideExit;

    }
    protected new void Update()
    {
        base.Update();
        JumpGravityScript();
        CheckForLadder();
        isCrouching = Crouch && !IsClimbing;

        //debug.position = new Vector2(transform.position.x, transform.position.y - bottomBoundsOffset);

        //Check if falling
        if (transform.position.y < lastFramePosition.y && Mathf.Abs(transform.position.y - lastFramePosition.y) > perFrameFallingDistance) isFalling = true;
        lastFramePosition = transform.position;
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateAnimator();
    }

    private void OnGroundCollideEnter(Collision2D collision)
    {
        currentJumpCount = 0;
        isJumping = false;
        isFalling = false;

        if (isAlive == false) 
        {
            rb.velocity = Vector2.zero;
        }

        if (collision.WasWithPlatform())
        {
            print("Ground enter");
            parentedPlatform = collision.transform;
            transform.SetParent(parentedPlatform);
        }
    }

    private void OnGroundCollideExit(Collision2D collision)
    {
        isFalling = true;
        if (collision.WasWithPlatform() && collision.transform == parentedPlatform)
        {
            print("Ground exit");
            parentedPlatform = null;
            transform.SetParent(parentedPlatform);
        }
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsClimbing", IsClimbing);
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsDead", !IsAlive);
            animator.SetBool("IsPlayingOrgan", isPlayingOrgan);
        }
    }

    private void CheckForLadder()
    {
        //RaycastHit2D hitInfo = Physics2D.Raycast(ladderRaycastTarget.position, Vector2.up, raycastDistance, ladderLayerMask);
        //canClimb = hitInfo.collider != null;
        //if (!canClimb) IsClimbing = false;
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

    public override void Jump(bool countAsJump = true)
    {
        //trigger jumping but only when the player can jump I.e. has not reached the max jumps
        if (currentJumpCount < maxJumpCount || countAsJump == false)
        {

            rb.velocity = new Vector2(0, jumpVelocity * jumpVelocityMultiplier);

            if (countAsJump)
            {
                AudioManager.current.AK_PlayClipOnObject("PlayPlayerJump", gameObject);
                currentJumpCount++;
                isJumping = true;
            }
        }
    }

    public override void Interact()
    {

    }

    public override void Move(Vector2 movement)
    {
        //if(movement.x != 0f) Debug.Log("move");
        //Do the movement thing
        Vector2 targetVelocity = movement * movementSpeed * movementSpeedMultiplier * Time.deltaTime;

        //Check whether targetVelocity includes y speed, if so then check if we can climb
        if (canClimb && targetVelocity.y != 0f) IsClimbing = true;
        isMoving = targetVelocity.x != 0f || (IsClimbing && targetVelocity.y != 0f);

        //Set sprite direction
        if (movement.x != 0)
        {
            bool directionIsNegative = movement.x < 0f;
            sr.flipX = directionIsNegative;
        }

        //rb.AddForce(targetVelocity, ForceMode2D.Force);
        //rb.AddRelativeForce(targetVelocity, ForceMode2D.Force);

        rb.velocity = IsClimbing ? targetVelocity : new Vector2(targetVelocity.x, rb.velocity.y);
    }

    public override void OnFootFall()
    {

    }

    public override void ResetPlayer()
    {
        base.ResetPlayer();
        rb.velocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        collisionNotifier.OnNotifyCollisionEnter -= OnGroundCollideEnter;
        collisionNotifier.OnNotifyCollisionExit -= OnGroundCollideExit;
    }

    public void SetPosition(Vector2 position)
    {
        rb.velocity = Vector2.zero;
        rb.position = position;
        isMoving = false;
    }

    public void PlayOrgan(bool play)
    {
        isPlayingOrgan = play;
    }
}
