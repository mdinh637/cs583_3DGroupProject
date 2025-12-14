using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour //IDamagable <- need to add this interface later. might be from a prev section
{
    [SerializeField] private float currentShieldAmount;
    public void SetupShield(float shieldAmount)
    {
        currentShieldAmount = shieldAmount;
    }

    public void TakeDamage(float damage)
    {
        currentShieldAmount -= damage;
        if (currentShieldAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
