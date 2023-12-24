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
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] int Hp = 150;

    private bool isJump = false;
    private float verticalVelocity;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5.0f;

    private Animator animator;

    [SerializeField] GameObject objThrowBorn;
    [SerializeField] Transform trsHead;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] float _cooldownTimeA = 6.0f;
    [SerializeField] float _cooldownTimeB = 0.0f;

    [SerializeField] BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
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

        SkillA();
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
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyDown(KeyCode.A) && count == 0)
        {
            GameObject obj = Instantiate(objThrowBorn, trsHead.position, Quaternion.identity, trsObjDynamic);
            ThrowBorn sc = obj.GetComponent<ThrowBorn>();

            bool isRight = (transform.localScale.x == 1.0f);
            Vector2 throwForce = isRight ? new Vector2(5.0f, 0.0f) : new Vector2(-5.0f, 0.0f);

            sc.SkillSetting(throwForce, isRight, _cooldownTimeA);
        }
    }

    private void StartAttack()
    {
        Debug.Log("Start");
        boxCollider.enabled = true;
    }

    private void EndAttack()
    {
        Debug.Log("End");
        boxCollider.enabled = false;
    }
}
