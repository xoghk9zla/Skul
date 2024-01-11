using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomEntAttack : MonoBehaviour
{
    [SerializeField] PolygonCollider2D collider2d;

    private void Awake()
    {
        collider2d = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy enemySc = transform.GetComponentInParent<Enemy>();
            Player Sc = collision.GetComponent<Player>();

            Sc.Hit(4.0f);
        }
    }
}
