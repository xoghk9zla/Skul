using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy enemySc = transform.GetComponentInParent<Enemy>();            
            Player Sc = collision.GetComponent<Player>();

            Sc.Hit(enemySc.GetDamage());
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
