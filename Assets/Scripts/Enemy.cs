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

    [SerializeField] private BoxCollider2D recognizeRange;

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

    private void SetAnimationParameter()
    {
        bool IsWalk = moveSpeed != 0.0f ? true : false;
        animator.SetBool("IsWalk", IsWalk);        
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
