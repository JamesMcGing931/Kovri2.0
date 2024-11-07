using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager atm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player is in contact with the enemy: " + other.name);  // Log when contact with enemy happens

            other.GetComponent<AttributesManager>().TakeDamage(atm.attack);
        }
    }
}
