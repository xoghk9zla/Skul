using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumColliders
{
    Attack,
}

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] enumColliders type;
    EntAttack entAttack;

    private void Awake()
    {
        entAttack = GetComponentInParent<EntAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        entAttack.TriggerEnter(type, collision);
    }
}
