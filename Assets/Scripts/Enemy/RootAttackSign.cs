using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttackSign : MonoBehaviour
{
    [SerializeField] private GameObject objRootAttack;
    [SerializeField] Transform trsObjEffect;

    private void Start()
    {
        GameObject objEffect = GameObject.Find("ObjectEffect");
        trsObjEffect = objEffect.GetComponent<Transform>();
    }

    private void EndSign()
    {
        Instantiate(objRootAttack, transform.position, Quaternion.identity, trsObjEffect);
        Destroy(gameObject);
    }
}
