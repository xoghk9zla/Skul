using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    private float lifeTime = 10.0f;
    private Rigidbody2D rigid;
    private Vector2 force;
    private float rotateforce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform != null && collision.transform.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (player != null)
            {
                AddForce(player);
            }            
        }
    }

    private void Start()
    {
        force.x = Random.Range(-3.0f, 3.0f);
        force.y = Random.Range(-3.0f, 3.0f);
        rotateforce = Random.Range(-1.0f, 1.0f);

        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(force, ForceMode2D.Impulse);
        rigid.AddTorque(rotateforce);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void AddForce(Player _player)
    {
        Vector3 dir = transform.position - _player.gameObject.transform.position;

        force.x = dir.normalized.x * Random.Range(0.0f, 3.0f);
        force.y = Random.Range(0.0f, 1.5f);
        rotateforce = Random.Range(-1.0f, 1.0f);

        rigid.AddForce(force, ForceMode2D.Impulse);
        rigid.AddTorque(rotateforce);
    }
}
