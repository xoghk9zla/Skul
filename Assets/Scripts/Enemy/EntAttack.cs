using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using static Enemy;

public class EntAttack : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator animator;
    private Enemy enemy;

    [SerializeField] private float attackSpeed;
    private float attackDelay;

    [SerializeField] private BoxCollider2D attackBox;
    [SerializeField] private float damage;

    private bool isAttack = false;
    private bool canAttack = true;

    public bool IsAttack
    {
        set
        {
            isAttack = value;
            if (enemy != null)
            {
                enemy.IsAttack = value;
            }
        }
    }

    public bool CanAttack
    {
        set
        {
            canAttack = value;
            if (enemy != null)
            {
                enemy.CanAttack = value;
            }
        }
    }

    private Vector3 playerPos;

    public void TriggerEnter(enumColliders _type, Collider2D _collision)
    {
        if (_type == enumColliders.Attack && _collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Enemy enemySc = transform.GetComponentInParent<Enemy>();
            Player Sc = _collision.GetComponent<Player>();

            Sc.Hit(damage);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        attackDelay = attackSpeed;
    }

    private void Update()
    {
        RecognizePlayer();
        CheckAttackTime();
    }

    private void RecognizePlayer()
    {
        RaycastHit2D recongnizeRange = Physics2D.BoxCast(transform.localPosition, new Vector2(0.6f, 0.5f),
            0.0f, Vector2.up, 0.3f, LayerMask.GetMask("Player"));
        
        if (recongnizeRange.transform != null && recongnizeRange.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canAttack)
            {
                Attack();
                playerPos = recongnizeRange.transform.localPosition;
            }            
        }
    }
    private void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !isAttack)
        {
            IsAttack = true;
            CanAttack = false;
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsAttack", true);
        }
    }

    private void CheckAttackTime()
    {
        if (!canAttack)
        {
            attackDelay -= Time.deltaTime;

            if (attackDelay <= 0.0f)
            {
                CanAttack = true;
                attackDelay = attackSpeed;
            }
        }
    }

    // 애니메이션 관련 함수
    private void FindTarget()
    {
        Vector3 dir = playerPos - transform.position;
        Enemy Sc = transform.GetComponent<Enemy>();

        if (dir.normalized.x * transform.localScale.x < 0.0f)
        {            
            Sc.Turn();
        }
        rigid.velocity += new Vector2(Sc.GetMoveSpeed() * 2.5f, 0.0f);
    }


    private void StartAttack()
    {
        attackBox.enabled = true;
    }

    private void EndAttack()
    {
        attackBox.enabled = false;

        isAttack = false;
        animator.SetBool("IsAttack", false);
    }
}
