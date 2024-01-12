using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomAttack : MonoBehaviour
{
    private Animator animator;
    private PolygonCollider2D collider2d;

    private float damage = 4.0f;
    private float attackDelay = 1.0f;
    private bool canDamage = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && canDamage)
        {
            Player Sc = collision.GetComponent<Player>();
            Sc.Hit(damage);
            canDamage = false;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider2d = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        CheckAttackDelay();
    }

    private void CheckAttackDelay()
    {
        if (!canDamage) 
        {
            attackDelay -= Time.deltaTime;

            if (attackDelay < 0.0f)
            {
                canDamage = true;
                attackDelay = 1.0f;
            }
        }        
    }

    // 애니메이션 관련 함수
    private void SetAnimationSpeed()
    {
        animator.SetFloat("BlossomSpeed", 0.25f);
    }
}
