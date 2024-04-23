using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlast : MonoBehaviour
{
    private float damage = 15.0f;
    private bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player") && canAttack)
        {
            Player Sc = collision.gameObject.GetComponent<Player>();
            Sc.Hit(damage);
            canAttack = false;
        }
    }

    private void EndEnergyBlast()
    {
        Destroy(gameObject);
    }
}
