using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    private float lifeTime = 10.0f;
    private Rigidbody2D rigid;
    private Vector2 force;
    private float rotateforce;

    private void Start()
    {
        force.x = Random.Range(-3.0f, 3.0f);
        force.y = Random.Range(-3.0f, 3.0f);
        rotateforce = Random.Range(-3.0f, 3.0f);

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
}
