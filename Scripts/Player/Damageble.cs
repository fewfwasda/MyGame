using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Damageble : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    public int _maxHealth = 100;
    public int _health = 100;
    private bool _isAlive = true;
    private bool isInvicible = false;
    private float timeSinceHit = 0;
    private float inviciblityTime = 0.25f;
    Animator animator;
    [SerializeField]private AudioClip damageAudioClip;
    [SerializeField] private AudioClip deathAudioClip;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationsString.isAlive, value);
            if (value == false)
            {
                damageableDeath.Invoke();
                SFXManager.instance.PlaySFXClip(deathAudioClip, transform, 1f);
            }
        }
    }
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationsString.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationsString.lockVelocity, value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            SFXManager.instance.PlaySFXClip(damageAudioClip, transform, 1f);            
            isInvicible = true;
            animator.SetTrigger(AnimationsString.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            LockVelocity = true;
            return true;
        }
        
        return false;

    }
    private void Update()
    {
        if (isInvicible)
        {
            if (timeSinceHit > inviciblityTime)
            {
                isInvicible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
