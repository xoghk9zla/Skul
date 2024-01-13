using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Events;
using static Enemy;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 moveDir;
    private float horizontal;
    private BoxCollider2D boxCollider2d;
    private bool isGround = false;

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float maxHp = 150.0f;
    [SerializeField] private float curHp;
    private float attackDamage = 5.0f;
    private float skillDamage = 3.0f;
    private float criticalChance = 10.0f;

    private bool isJump = false;
    private bool canJump;
    private float verticalVelocity; 
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5.0f;

    private bool isDash = false;
    private float dashTimer = 0.0f;
    [SerializeField] private float dashLimit = 0.2f;

    [SerializeField] GameObject objDashEffect;
    [SerializeField] GameObject objJumpEffect;
    [SerializeField] Transform trsDash;
    [SerializeField] Transform trsJump;

    private Animator animator;
    private bool isAttack;
    private bool canAttack;
    private bool isJumpAttack;    

    [SerializeField] GameObject objThrowBone;
    [SerializeField] GameObject objReboneEffect;
    [SerializeField] Transform trsHead;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] Transform trsObjEffect;
    [SerializeField] float cooldownTimeA = 6.0f;
    [SerializeField] float cooldownTimeS = 3.0f;

    [SerializeField] PlayerHp playerHp;
    [SerializeField] SkillManager skillManager;

    public bool IsGround
    {
        get => isGround;
        set
        {
            isGround = value;
        }
    }

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

    public bool IsJumpAttack
    {
        get => isJumpAttack;
        set
        {
            isJumpAttack = value;
        }
    }

    public float AttackDamage
    {
        get => attackDamage;
        set
        {
            attackDamage = value;
        }
    }

    public float SkillDamage
    {
        get => skillDamage;
        set
        {
            skillDamage = value;
        }
    }

    public float CriticalChance
    {
        get => criticalChance;
        set
        {
            criticalChance = value;
        }
    }

    private UnityAction prepareAction;
    public void SetPrepareAction(UnityAction _action)
    {
        prepareAction += _action;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FogWolf Sc = collision.GetComponent<FogWolf>();
                Sc.GiveBuff(this);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerHp = GetComponent<PlayerHp>();
        skillManager = GetComponent<SkillManager>();
        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Moving();
        Turing();
        Jumping();
        CheckGravity();
        CheckDash();
        SetAnimationParameter();

        SkillS();
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0.0f, Vector2.down, 0.025f, LayerMask.GetMask("Ground"));

        IsGround = false;

        if(verticalVelocity > 0.0f)
        {
            return;
        }

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsGround = true;
            canJump = true;
        }
        
        prepareAction.Invoke();
    }

    private void Moving()
    {
        if (isDash)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1.0f;            
        }
        else
        {
            horizontal = 0.0f;
        }

        moveDir.x = horizontal * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    private void Turing()
    {
        Vector3 scale = transform.localScale;

        if (moveDir.x > 0 && transform.localScale.x == -1.0f)
        {
            scale.x *= -1.0f;
        }
        else if (moveDir.x < 0 && transform.localScale.x == 1.0f)
        {
            scale.x *= -1.0f;
        }

        transform.localScale = scale;
    }

    private void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.C) && canJump)
        {
            isJump = true;
        }        
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isDash)
        {
            bool isRight = (transform.localScale.x == 1.0f);
            
            isDash = true;
            verticalVelocity = 0.0f;
            rigid.velocity = new Vector2(isRight ? 3.0f : -3.0f, 0.0f);
            animator.SetBool("IsDash", true);
            GameObject objDash = Instantiate(objDashEffect, trsDash.position, Quaternion.identity, trsDash);
            objDash.transform.parent = trsObjEffect;
        }
        else if(isDash)
        {
            dashTimer += Time.deltaTime;
            
            if(dashTimer >= dashLimit)
            {
                dashTimer = 0.0f;
                isDash = false;
                animator.SetBool("IsDash", false);
            }
            
        }
    }
    private void CheckGravity()
    {
        if (isDash)
        {
            return;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        
        if (verticalVelocity < -10.0f)
        {
            verticalVelocity = -10.0f;
        }
        else
        {
            verticalVelocity = Mathf.Lerp(verticalVelocity, 0.0f, Time.deltaTime * 5.0f);
        }
        
        if(isJump)
        {
            isJump = false;
            verticalVelocity = jumpForce;

            if(canJump && !isGround)
            {
                canJump = false;
                GameObject objJump = Instantiate(objJumpEffect, trsJump.position, Quaternion.identity, trsJump);
                objJump.transform.parent = trsObjEffect;
            }
        }
        

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void SetAnimationParameter()
    {
        animator.SetInteger("Horizontal", (int)moveDir.x);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetBool("IsGround", isGround);
    }

    private void SkillS()
    {
        int count = trsObjDynamic.childCount;

        if (Input.GetKeyDown(KeyCode.S) && count != 0 && !animator.GetBool("IsRebone") && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillS))
        {
            Vector3 effectPos = transform.position;
            effectPos.y += 0.2f;

            GameObject obj = Instantiate(objReboneEffect, effectPos, Quaternion.identity, trsObjDynamic);
            Transform trsThrowBorn = trsObjDynamic.GetChild(0);
            Vector3 movePos = trsThrowBorn.position;

            movePos.y += 0.05f;
            transform.position = movePos;

            Instantiate(objReboneEffect, transform.position, Quaternion.identity, trsObjDynamic);            

            animator.SetBool("IsRebone", true);
            animator.SetBool("IsThrow", false);                     

            curHp -= 5.0f;
            playerHp.SetPlayerHp(curHp, maxHp); // test
            
            skillManager.ActiveSkill(SkillManager.SkillType.SkillS);
        }
    }

    public void Hit(float _damage)
    {
        curHp -= _damage;
        playerHp.SetPlayerHp(curHp, maxHp);
    }

    // 플레이어 상태 설정 관련 함수
    public void SetPlayerHp(PlayerHp _value)
    {
        playerHp = _value;
        playerHp.SetPlayerHp(curHp, maxHp);
    }

    public void SetSkill(SkillManager _value)
    {
        skillManager = _value;
        skillManager.SetSkill(cooldownTimeA, cooldownTimeS);
    }

    public void SetBuffStats(BuffManager.BuffList _buffType)
    {
        BuffManager.BuffStats buffStats = new BuffManager.BuffStats();

        if (_buffType == BuffManager.BuffList.AttackSpeed)
        {
            animator.SetFloat("AttackSpeed", buffStats.AttackSpeed);
        }
        else if (_buffType == BuffManager.BuffList.CriticalChance)
        {
            criticalChance += buffStats.Critical;
        }
        else if (_buffType == BuffManager.BuffList.Health)
        {
            maxHp += buffStats.Hp;
            curHp += buffStats.Hp;
            playerHp.SetPlayerHp(curHp, maxHp);
        }
        else if(_buffType == BuffManager.BuffList.MeleeAttackDamage)
        {
            attackDamage *= buffStats.AttackDamage;
        }
        else if(_buffType == BuffManager.BuffList.SkillAttackDamage)
        {
            skillDamage *= buffStats.SkillDamage;
        }

        // 버프 적용 후 변수 초기화
        if (prepareAction != null)
        {
            prepareAction.Invoke();
        }
    }

    // Animator 관련 함수들
    private void EndReborn()
    {
        animator.SetBool("IsRebone", false);                
    }
}
