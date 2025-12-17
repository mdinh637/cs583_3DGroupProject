using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Castle trigger hit by: " + other.name);

    if (other.CompareTag("Enemy"))
    {
        Debug.Log("It was an enemy!");
        other.GetComponent<Enemy>().TakeDamage(9999); // big damage
    }
}
}
