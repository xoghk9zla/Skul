using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isGround = false;

    private bool isAttack = false;
    private bool canAttack = true;
    private bool isJumpAttack = false;

    public bool IsGround
    {
        set
        {
            isGround = value;
            if (player != null)
            {
                player.IsGround = value;
            }
        }
    }

    public bool IsAttack
    {
        set
        {
            isAttack = value;
            if (player != null)
            {
                player.IsAttack = value;
            }
        }
    }

    public bool CanAttack
    {
        set
        {
            canAttack = value;
            if (player != null)
            {
                player.CanAttack = value;
            }
        }
    }

    public bool IsJumpAttack
    {
        set
        {
            isJumpAttack = value;
            if (player != null)
            {
                player.IsJumpAttack = value;
            }
        }
    }

    [SerializeField] private float attackDamage;
    [SerializeField] private float criticalChance;

    [SerializeField] private BoxCollider2D attackCollider;

    private Animator animator;
    private Player player;

    public float AttackDamage
    {
        set
        {
            attackDamage = value;
            if (player != null)
            {
                player.AttackDamage = value;
            }
        }
    }

    public float CriticalChance
    {
        set
        {
            criticalChance = value;
            if (player != null)
            {
                player.CriticalChance = value;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            float damage = CheckCritical();
            Enemy Sc = collision.GetComponent<Enemy>();
            Sc.Hit(damage);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (player != null)
        {
            player.SetPrepareAction(GetIsGroundValue);
            player.SetPrepareAction(GetIsAttackValue);
            player.SetPrepareAction(GetCanAttackValue);
            player.SetPrepareAction(GetIsJumpAttackValue);
            player.SetPrepareAction(GetAttackDamageValue);
            player.SetPrepareAction(GetCriticalChanceValue);
        }
    }

    private void GetIsGroundValue()
    {
        IsGround = player.IsGround;
    }

    private void GetIsAttackValue()
    {
        IsAttack = player.IsAttack;
    }

    private void GetCanAttackValue()
    {
        CanAttack = player.CanAttack;
    }

    private void GetIsJumpAttackValue()
    {
        IsJumpAttack = player.IsJumpAttack;
    }

    private void GetAttackDamageValue()
    {
        AttackDamage = player.AttackDamage;
    }

    private void GetCriticalChanceValue()
    {
        CriticalChance = player.CriticalChance;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isAttack && isGround)
        {
            IsAttack = true;
            attackCollider.enabled = false; // 애니메이션 처리 함수에서 다시 켜줌
            animator.SetInteger("Horizontal", 0);
            animator.SetBool("IsAttack", true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && isAttack && isGround && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackA"))
        {
            attackCollider.enabled = false;
            animator.SetBool("ComboAttack", true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && !isJumpAttack && !isGround)
        {
            isJumpAttack = true;
            animator.SetBool("IsJumpAttack", true);
        }
    }

    private float CheckCritical()
    {
        int isCritical = Random.Range(0, 100);
        float result = attackDamage;

        if (isCritical <= criticalChance)
        {
            result *= 1.5f;
        }
        return result;
    }

    // Animator 관련 함수들
    private void StartAttack()
    {
        attackCollider.enabled = true;
    }

    private void EndAttack()
    {
        attackCollider.enabled = false;

        IsAttack = false;
        animator.SetBool("IsAttack", false);
    }

    private void EndJumpAttack()
    {
        attackCollider.enabled = false;

        isJumpAttack = false;
        animator.SetBool("IsJumpAttack", false);
    }

    private void EndComboAttack()
    {
        attackCollider.enabled = false;

        IsAttack = false;
        animator.SetBool("IsAttack", false);
        animator.SetBool("ComboAttack", false);
    }
}
