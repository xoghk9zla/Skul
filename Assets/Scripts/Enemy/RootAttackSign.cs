using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttackSign : MonoBehaviour
{
    [SerializeField] private GameObject objRootAttack;
    [SerializeField] Transform trsObjDynamic;


    private void Start()
    {
        GameObject objDynamic = GameObject.Find("ObjectDynamic");
        trsObjDynamic = objDynamic.GetComponent<Transform>();
    }

    private void EndSign()
    {
        Instantiate(objRootAttack, transform.position, Quaternion.identity, trsObjDynamic);
        Destroy(gameObject);
    }
}
