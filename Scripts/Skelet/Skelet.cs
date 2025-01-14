using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageble))]
public class Skelet : MonoBehaviour
{
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageble damageble;
    private float walkAcceleration = 50f;
    private float maxSpeed = 3f;
    private float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    
    public enum WalkableDirection { Right, Left };
    private WalkableDirection _walkDirection;
    private bool _hasTarget = false;
    private Vector2 walkDirectionVector = Vector2.right;
    public DetectionZone cliffDetectionZone;
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right) walkDirectionVector = Vector2.right;
                else if (value == WalkableDirection.Left) walkDirectionVector = Vector2.left;
            }

            _walkDirection = value;
        }
    }
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationsString.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get { return animator.GetBool(AnimationsString.canMove); }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationsString.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationsString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageble = GetComponent<Damageble>();
        
    }
    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0) AttackCooldown -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if (!damageble.LockVelocity)
        {
            if (CanMove)
            {

                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime),
                    -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right) WalkDirection = WalkableDirection.Left;
        else if (WalkDirection == WalkableDirection.Left) WalkDirection = WalkableDirection.Right;
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded) FlipDirection();
    }
}
