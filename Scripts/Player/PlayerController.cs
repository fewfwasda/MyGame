using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageble))]
public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private float walkSpeed = 5f;
    private float runSpeed = 8f;
    [SerializeField]private float jumpInpulse = 5f;
    private float airWalkSpeed = 5f;
    TouchingDirections touchingDirections;
    Rigidbody2D rb;
    Animator animator;
    Damageble damageble;
    private bool _isRunning = false;
    private bool _isMoving = false;
    public bool _isFacingRight = true;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning) return runSpeed;
                        else return walkSpeed;
                    }
                    else return airWalkSpeed;
                }
                else return 0;
            }
            else return 0;
        }
    }
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationsString.isMoving, value);            
        }
    }
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationsString.isRunning, value);
        }
    }
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value) transform.localScale *= new Vector2(-1, 1);
            _isFacingRight = value;
        }
    }
    public bool IsAlive
    {
        get { return animator.GetBool(AnimationsString.isAlive); }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationsString.canMove);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageble = GetComponent<Damageble>();        
    }
    private void FixedUpdate()
    {
        if (!damageble.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
        animator.SetFloat(AnimationsString.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
            //ÃÎËÎÂÀ ÑÂÅÑÒÈÒ
            //SFXManager.instance.PlaySFXClip(stepSoundClip, transform, 1f);
        }
        else
        {            
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;            
        }
        else if (context.canceled) IsRunning = false;
        
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationsString.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpInpulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationsString.attackTriger);
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    private void Update()
    {
        Respawn();
    }
    private void Respawn()
    {
        if(!IsAlive) SceneManager.LoadScene(2);
    }
}