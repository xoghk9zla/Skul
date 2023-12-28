using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector3 moveDir;
    private float horizontal;
    private BoxCollider2D boxCollider2d;
    private bool isGround = false;

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float maxHp = 150.0f;
    [SerializeField] private float curHp;

    private bool isJump = false;
    private float verticalVelocity; 
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5.0f;

    private Animator animator;
    private bool isAttack;
    private bool isJumpAttack;

    [SerializeField] GameObject objThrowBone;
    [SerializeField] GameObject objReboneEffect;
    [SerializeField] Transform trsHead;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] float cooldownTimeA = 6.0f;
    [SerializeField] float cooldownTimeS = 3.0f;

    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] PlayerHp playerHp;
    [SerializeField] SkillManager skillManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy Sc = collision.GetComponent<Enemy>();
            Sc.Hit();
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
        SetAnimationParameter();

        Attack();

        SkillA();
        SkillS();
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0.0f, Vector2.down, 0.025f, LayerMask.GetMask("Ground"));

        isGround = false;

        if(verticalVelocity > 0.0f)
        {
            return;
        }

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
        
    }

    private void Moving()
    {
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            isJump = true;
        }        
    }

    private void CheckGravity()
    {
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
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void SetAnimationParameter()
    {
        animator.SetInteger("Horizontal", (int)moveDir.x);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetBool("IsGround", isGround);
    }

    private void SkillA()
    {
        int count = trsObjDynamic.childCount;

        if (Input.GetKeyDown(KeyCode.A) && count == 0 && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillA))
        {
            GameObject obj = Instantiate(objThrowBone, trsHead.position, Quaternion.identity, trsObjDynamic);
            ThrowBone sc = obj.GetComponent<ThrowBone>();

            bool isRight = (transform.localScale.x == 1.0f);
            Vector2 throwForce = isRight ? new Vector2(5.0f, 0.0f) : new Vector2(-5.0f, 0.0f);

            sc.SkillSetting(throwForce, isRight, cooldownTimeA);

            animator.SetBool("IsThrow", true);

            skillManager.ActiveSkill(SkillManager.SkillType.SkillA);
        }
    }

    private void SkillS()
    {
        int count = trsObjDynamic.childCount;

        if (Input.GetKeyDown(KeyCode.S) && count != 0 && !animator.GetBool("IsRebone") && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillS))
        {
            Vector3 effectPos = transform.position;
            GameObject obj = Instantiate(objReboneEffect, effectPos, Quaternion.identity, trsObjDynamic);
            Transform trsThrowBorn = trsObjDynamic.GetChild(0);
            Vector3 movePos = trsThrowBorn.position;

            movePos.y += 0.05f;
            transform.position = movePos;

            animator.SetBool("IsRebone", true);
            animator.SetBool("IsThrow", false);
            transform.Find("RebornEffect").gameObject.SetActive(true);

            curHp -= 5.0f;
            playerHp.SetPlayerHp(curHp, maxHp); // test
            
            skillManager.ActiveSkill(SkillManager.SkillType.SkillS);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isAttack && isGround)
        {
            isAttack = true;
            animator.SetBool("IsAttack", true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && isAttack && isGround)
        {
            animator.SetBool("ComboAttack", true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && !isJumpAttack && !isGround)
        {
            isJumpAttack = true;
            animator.SetBool("IsJumpAttack", true);
        }

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

    // Animator 관련 함수들
    private void StartAttack()
    {
        boxCollider.enabled = true;
    }

    private void EndAttack()
    {
        boxCollider.enabled = false;

        isAttack = false;
        isJumpAttack = false;
        animator.SetBool("IsAttack", false);
        animator.SetBool("ComboAttack", false);
        animator.SetBool("IsJumpAttack", false);
    }

    private void EndThrow()
    {
        animator.SetBool("IsThrow", false);
    }

    private void EndReborn()
    {
        animator.SetBool("IsRebone", false);        
        transform.Find("RebornEffect").gameObject.SetActive(false);

        foreach(Transform child in trsObjDynamic)
        {
            Destroy(child.gameObject);
        }
    }
}
