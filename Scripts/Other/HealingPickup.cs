using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    private int healthRestore = 10;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageble damageble = collision.GetComponent<Damageble>();
        if (damageble && damageble.Health < damageble.MaxHealth)
        {
            bool wasHealed = damageble.Heal(healthRestore);
            if (wasHealed)
            {
                
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}