using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2d;
    private Rigidbody2D rigid;

    [SerializeField] LayerMask ground;
    [SerializeField] private BoxCollider2D trigger;
    [SerializeField] private bool isGround = false;

    [SerializeField] private float maxHp = 20.0f;
    [SerializeField] private float curHp;
    [SerializeField] private float moveSpeed = 0.3f;

    public enum enumEnemyType
    {
        ScareCrow, Ent, RootEnt, FlowerEnt, ForestKeeper,
    }

    public enumEnemyType enemyType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(boxCollider2d != null && collision.gameObject.layer == LayerMask.NameToLayer("Skill")) 
        {
            animator.SetBool("IsHit", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Moving();
    }

    private void FixedUpdate()
    {
        if (trigger.IsTouchingLayers(ground) == false && isGround)
        {
            Turn();
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0.0f, Vector2.down, 0.025f, LayerMask.GetMask("Ground"));

        isGround = false;

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }

    }

    private void Moving()
    {
        if(isGround && enemyType != enumEnemyType.ForestKeeper && enemyType != enumEnemyType.ScareCrow)
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
