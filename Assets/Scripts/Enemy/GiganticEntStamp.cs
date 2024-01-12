using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticEntStamp : MonoBehaviour
{
    float damage = 15.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player Sc = collision.GetComponent<Player>();
            Sc.Hit(damage);
        }
    }
}
