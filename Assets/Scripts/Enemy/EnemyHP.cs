using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private Transform trsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Enemy Sc = GetComponentInParent<Enemy>();
        trsEnemy = Sc.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = trsEnemy.position - new Vector3(0.0f, 0.05f, 0.0f);
    }
}
