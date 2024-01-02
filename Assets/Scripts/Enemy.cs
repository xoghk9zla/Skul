using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    private float maxHp;
    private float curHp;

    public enum enumEnemyType
    {
        ScareCrow, Ent, RootEnt, FlowerEnt, ForestKeeper,
    }

    public enumEnemyType enemyType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(boxCollider2D != null && collision.gameObject.layer == LayerMask.NameToLayer("Skill")) 
        {
            animator.SetBool("IsHit", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float _damage)
    {
        animator.SetBool("IsHit", true);

        if(enemyType != enumEnemyType.ScareCrow)
        {
            curHp -= _damage;
        }
    }

    // 애니메이션 관련 함수들
    public void EndHit()
    {
        animator.SetBool("IsHit", false);
    }
}
