using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D hitBox;
    private Rigidbody2D rigid;

    private EnemyHP enemyHP;

    [SerializeField] LayerMask ground;
    [SerializeField] private BoxCollider2D checkGround;
    [SerializeField] private bool isGround = false;

    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    [SerializeField] private float moveSpeed;

    private bool isHit = false;
    private bool isAttack = false;
    private bool canAttack = true;

    [SerializeField] private GameObject damageFont;
    private Transform trsObjEffect;

    private float turnDelay = 1.0f;
    private bool canTurn = true;

    [SerializeField] private GameObject[] parts;

    // property 변수
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
    // UnityAction 으로 이벤트 예약-> isAttack, canAttack 변수 다른 스크립트들과 동기화(델리게이트)
    private UnityAction prepareAction;  // 기능을 예약
    public void SetPrepareAction(UnityAction _action)   // 예약을 받음
    {
        prepareAction += _action;
    }

    public enum enumEnemyType
    {
        ScareCrow, Ent, RootEnt, FlowerEnt, GiganticEnt, Boss
    }

    public enumEnemyType enemyType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();        

        GameObject objEffect = GameObject.Find("ObjectEffect");
        trsObjEffect = objEffect.transform;

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

    private void Start()
    {
        enemyHP = GetComponentInChildren<EnemyHP>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Moving();
        CheckPlayer();

        TurnTimer();
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

        RaycastHit2D wallCheck = Physics2D.Raycast(hitBox.bounds.center, Vector2.right * transform.localScale.x, 0.35f, LayerMask.GetMask("Ground"));

        if (wallCheck.transform != null && wallCheck.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Turn();
        }

    }

    private void CheckPlayer()
    {
        RaycastHit2D recongnizeRange = Physics2D.BoxCast(transform.localPosition, new Vector2(2.0f, 0.5f),
            0.0f, Vector2.up, 0.3f, LayerMask.GetMask("Player"));
        
        if(recongnizeRange.transform != null && recongnizeRange.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 dir = recongnizeRange.transform.position - transform.localPosition;
            if(dir.normalized.x * transform.localScale.x < 0.0f && enemyType != enumEnemyType.ScareCrow && enemyType != enumEnemyType.GiganticEnt && canTurn)
            {
                Turn();
                canTurn = false;
            }
        }
    }

    private void TurnTimer()
    {
        if (!canTurn)
        {
            turnDelay -= Time.deltaTime;

            if (turnDelay < 0)
            {
                turnDelay = 1.0f;
                canTurn = true;
            }
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
        if(enemyType == enumEnemyType.Boss)
        {
            return;
        }
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
            enemyHP.SetPlayerHp(curHp, maxHp);

            if (curHp <= 0)
            {
                Death();
            }
        }

        DamageFont Sc = damageFont.GetComponent<DamageFont>();
        Sc.SetText(_damage, DamageFont.damageType.enemy);

        Vector3 SpawnPos = hitBox.bounds.center;
        float rangeX = hitBox.bounds.size.x;
        rangeX = Random.Range((rangeX / 2) * -1.0f, rangeX / 2) + hitBox.bounds.center.x;
        SpawnPos.x = rangeX;

        Instantiate(damageFont, SpawnPos, Quaternion.identity, trsObjEffect);
    }

    private void Death()
    {
        foreach(var part in parts)
        {
            Instantiate(part, hitBox.bounds.center, Quaternion.identity, trsObjEffect);
        }

        Destroy(gameObject);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // 애니메이션 관련 함수들
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
