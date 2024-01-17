using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D hitBox;
    private Rigidbody2D rigid;

    [SerializeField] LayerMask ground;
    [SerializeField] private BoxCollider2D checkGround;
    [SerializeField] private bool isGround = false;

    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    [SerializeField] private float moveSpeed;

    private bool isHit = false;
    private bool isAttack = false;
    private bool canAttack = true;

    // property ����
    public bool IsAttack
    {
        get => isAttack;
        set 
        {
            isAttack = value;
        }
    }

    public bool CanAttack
    {
        get => canAttack;
        set
        {
            canAttack = value;
        }
    }
    // UnityAction ���� �̺�Ʈ ����-> isAttack, canAttack ���� �ٸ� ��ũ��Ʈ��� ����ȭ(��������Ʈ)
    private UnityAction prepareAction;  // ����� ����
    public void SetPrepareAction(UnityAction _action)   // ������ ����
    {
        prepareAction += _action;
    }

    public enum enumEnemyType
    {
        ScareCrow, Ent, RootEnt, FlowerEnt, GiganticEnt,
    }

    public enumEnemyType enemyType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();

        curHp = maxHp;
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
        if(isGround && !isHit && !IsAttack)
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
    }    

    private void SetAnimationParameter()
    {
        bool IsWalk = moveSpeed != 0.0f ? true : false;
        animator.SetBool("IsWalk", IsWalk);        
    }

    public void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        moveSpeed *= -1.0f;

        transform.localScale = scale;
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

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // �ִϸ��̼� ���� �Լ���
    public void EndHit()
    {
        isHit = false;
        animator.SetBool("IsHit", false);

        if(enemyType != enumEnemyType.ScareCrow)
        {
            IsAttack = false;
            if (prepareAction != null)
            { 
                prepareAction.Invoke();
            }
            //preapreAction?.Invoke();
            animator.SetBool("IsAttack", false);
        }
        
    }
}
