using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public AttributesManager playerAtm;
    public AttributesManager enemyAtm;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerAtm.DealDamage(enemyAtm.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            enemyAtm.DealDamage(playerAtm.gameObject);
        }
    }
}
