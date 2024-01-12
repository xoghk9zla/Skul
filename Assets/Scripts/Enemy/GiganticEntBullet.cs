using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticEntBullet : MonoBehaviour
{
    private float damage = 7.0f;
    private float bulletSpeed = 2.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player Sc = collision.GetComponent<Player>();
            Sc.Hit(damage);

            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    private void Moving()
    {
        transform.localPosition += transform.right * Time.deltaTime * bulletSpeed;
    }
}
