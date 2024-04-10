using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public DetectionZone biteDetectionZone;
    private float flightSpeed = 2f;
    private float waypointReachedDistance = 0.1f;
    public Collider2D deathCollider;
    public List<Transform> waypoints;
    Animator animator;
    Rigidbody2D rb;
    Damageble damageble;
    Transform nextWaypoint;
    private int waypointNum = 0;
    
    private bool _hasTarget = false;
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
        get
        {
            return animator.GetBool(AnimationsString.canMove);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageble = GetComponent<Damageble>();
    }
    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }
    private void OnEnable()
    {
        damageble.damageableDeath.AddListener(OnDeath);
    }
    private void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }
    private void FixedUpdate()
    {
        if (damageble.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else rb.velocity = Vector3.zero;
        }
        else
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if (waypointNum >= waypoints.Count) waypointNum = 0;
            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb.velocity.x < 0) transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
        }
        else
        {
            if (rb.velocity.x > 0) transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
        }
    }
    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}