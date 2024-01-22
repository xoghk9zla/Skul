using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticEntSpawn : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject objGiganticEnt;
    private Transform trsEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            Vector3 SpawnPos = new Vector3(transform.position.x, boxCollider.bounds.center.y - 0.4f, 0.0f);
             
            Instantiate(objGiganticEnt, SpawnPos, Quaternion.identity, trsEnemy);            

            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        GameObject objEnemy = GameObject.Find("ObjectEnemy");
        trsEnemy = objEnemy.transform;
    }
}
