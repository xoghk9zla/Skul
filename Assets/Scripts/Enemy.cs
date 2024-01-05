using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D hitBox;
    private Rigidbody2D rigid;

    [SerializeField] LayerMask ground;
    [SerializeField] private BoxCollider2D checkGround;
    [SerializeField] private bool isGround = false;

    [SerializeField] private float maxHp = 20.0f;
    [SerializeField] private float curHp;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float damage;

    [SerializeField] private BoxCollider2D recognizeRange;
    [SerializeField] private BoxCollider2D boxCollider;
    private bool isAttack = false;
    private bool isHit = false;

    private Vector3 playerPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Attack();
            playerPos = collision.transform.localPosition;
        }
    }

    public enum enumEnemyType
    {
        ScareCrow, Ent, RootEnt, FlowerEnt, ForestKeeper,
    }

    public enumEnemyType enemyType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hitBox != null && collision.gameObject.layer == LayerMask.NameToLayer("Skill")) 
        {
            isHit = true;
            animator.SetBool("IsHit", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Moving();

        SetAnimationParameter();
    }

    private void FixedUpdate()
    {
        if (checkGround.IsTouchingLayers(ground) == false && isGround)
        {
            Turn();
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(hitBox.bounds.center, hitBox.bounds.size, 0.0f, Vector2.down, 0.025f, LayerMask.GetMask("Ground"));

        isGround = false;

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }

    }

    private void Moving()
    {
        if(isGround && enemyType != enumEnemyType.ForestKeeper && enemyType != enumEnemyType.ScareCrow && !isHit && !isAttack)
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        moveSpeed *= -1.0f;

        transform.localScale = scale;
    }

    private void SetAnimationParameter()
    {
        bool IsWalk = moveSpeed != 0.0f ? true : false;
        animator.SetBool("IsWalk", IsWalk);        
    }

    private void Attack()
    {
        if (enemyType == enumEnemyType.Ent && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !isAttack)
        {
            isAttack = true;
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsAttack", true);
        }
    }

    public void Hit(float _damage)
    {
        isHit = true;
        animator.SetBool("IsHit", true);

        if(enemyType != enumEnemyType.ScareCrow)
        {
            curHp -= _damage;
        }
    }

    // �ִϸ��̼� ���� �Լ���
    private void FindTarget()
    {
        Vector3 dir = playerPos - transform.position;
        Debug.Log(dir.normalized.x);
        if(dir.normalized.x * transform.localScale.x < 0.0f)
        {
            Turn();
        }
        rigid.velocity += new Vector2(moveSpeed * 2.5f, 0.0f);
    }

    private void StartAttack()
    {
        recognizeRange.enabled = false;
        boxCollider.enabled = true;
    }

    private void EndAttack()
    {
        recognizeRange.enabled = true;
        boxCollider.enabled = false;

        isAttack = false;
        animator.SetBool("IsAttack", false);
    }

    public void EndHit()
    {
        isHit = false;
        animator.SetBool("IsHit", false);
    }
}
