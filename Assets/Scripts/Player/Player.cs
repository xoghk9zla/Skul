using System.Collections;
using System.Collections.Generic;
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
    private float criticalChance = 10.0f;
    private float attackDamage;
    private float skillADamage;
    private float skillSDamage;

    private bool isJump = false;
    private bool canJump;
    private float verticalVelocity; 
    private float gravity = 9.81f;
    private float jumpForce = 5.0f;

    private bool isDash = false;
    private float dashTimer = 0.0f;
    private float dashLimit = 0.2f;

    [SerializeField] GameObject objDashEffect;
    [SerializeField] GameObject objJumpEffect;
    [SerializeField] Transform trsDash;
    [SerializeField] Transform trsJump;

    private Animator animator;
    private bool isAttack;
    private bool canAttack;
    private bool isJumpAttack;    

    [SerializeField] Transform trsObjEffect;
    [SerializeField] float cooldown_SkillA;
    [SerializeField] float cooldown_SkillS;

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

    public float SkillADamage
    {
        get => skillADamage;
        set
        {
            skillADamage = value;
        }
    }
    
    public float SkillSDamage
    {
        get => skillSDamage;
        set
        {
            skillSDamage = value;
        }
    }
    
    public float Cooldown_SkillA
    {
        get => cooldown_SkillA;
        set
        {
            cooldown_SkillA = value;
        }
    }
    
    public float Cooldown_SkillS
    {
        get => cooldown_SkillS;
        set
        {
            cooldown_SkillS = value;
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
        curHp = maxHp;

        GameObject objEffect = GameObject.Find("ObjectEffect");
        trsObjEffect = objEffect.transform;
    }

    private void Start()
    {
        playerHp = PlayerHp.Instance;
        skillManager = SkillManager.Instance;
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

    public void Hit(float _damage)
    {
        curHp -= _damage;
        playerHp.SetPlayerHp(curHp, maxHp);
    }

    // �÷��̾� ���� ���� ���� �Լ�
    public void SetPlayerHp(PlayerHp _value)
    {
        playerHp = _value;
        playerHp.SetPlayerHp(curHp, maxHp);
        playerHp.SetPlayerHp(curHp, maxHp);
    }

    public void SetSkill(SkillManager _value)
    {
        skillManager = _value;
        
        if (prepareAction != null)
        {
            prepareAction.Invoke();
        }
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
            SkillADamage *= buffStats.SkillDamage;
            SkillSDamage *= buffStats.SkillDamage;
        }

        // ���� ���� �� ���� �ʱ�ȭ
        if (prepareAction != null)
        {
            prepareAction.Invoke();
        }
    }
}
