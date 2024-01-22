using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEntSpawn : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject[] objEnt;
    private Transform trsEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < 4; ++i)
            {
                float rangeX = boxCollider.bounds.size.x;
                rangeX = Random.Range((rangeX / 2) * -1.0f, rangeX / 2) + transform.position.x;

                int type = Random.Range(0, objEnt.Length);

                Vector3 SpawnPos = new Vector3(rangeX, boxCollider.bounds.center.y, 0.0f);

                Instantiate(objEnt[type], SpawnPos, Quaternion.identity, trsEnemy);
            }

            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        GameObject objEnemy = GameObject.Find("ObjectEnemy");
        trsEnemy = objEnemy.transform;
    }
}
