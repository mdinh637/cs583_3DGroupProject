using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : Enemy
{
    [Header("Enemy Details")]
    [SerializeField] private float shield = 50f;
    [SerializeField] private EnemyShield shieldObject;
    //can add after everything works
    //[SerializeField] private float damageReductionPercent = 20f; //dmg taken without shield is reduced by 20%

    protected override void Awake()
    {
        base.Awake();
        if(shieldObject != null)
        {
            // upon Awake, setup the shield 
            shieldObject.gameObject.SetActive(true);
            shieldObject.SetupShield(shield);
        }
    }
}
