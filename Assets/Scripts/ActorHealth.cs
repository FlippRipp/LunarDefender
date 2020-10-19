using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorHealth : MonoBehaviour
{
    public  float maxHealth = 10;
    public float currentHealth;

    public bool destroyOnDeath = false;
    public UnityEvent onDeath;
    public UnityEvent onHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        onHealthChanged?.Invoke();
        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
        }
    }
}
