using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticEntAttack : MonoBehaviour
{
    private bool isAttack = false;
    private bool canAttack = true;

    public bool IsAttack
    {
        set
        {
            isAttack = value;
            if (enemy != null)
            {
                enemy.IsAttack = value;
            }
        }
    }

    public bool CanAttack
    {
        set
        {
            canAttack = value;
            if (enemy != null)
            {
                enemy.CanAttack = value;
            }
        }
    }

    [SerializeField] private float attackSpeed;
    private float attackDelay;

    [SerializeField] private GameObject objBullet;
    [SerializeField] private GameObject objStamp;
    [SerializeField] private Transform trsMouse;
    [SerializeField] private Transform trsObjEffect;

    private Animator animator;

    private Enemy enemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        attackDelay = attackSpeed;
    }

    private void Start()
    {
        GameObject objEffect = GameObject.Find("ObjectEffect");
        trsObjEffect = objEffect.transform;

        if (enemy != null)
        {
            enemy.SetPrepareAction(GetIsAttackValue);
            enemy.SetPrepareAction(GetCanAttackValue);
        }
    }

    private void GetIsAttackValue()
    {
        IsAttack = enemy.IsAttack;
    }

    private void GetCanAttackValue()
    {
        CanAttack = enemy.CanAttack;
    }

    private void Update()
    {
        RecognizePlayer();
        CheckAttackTime();
    }

    private void RecognizePlayer()
    {
        Vector3 pos = transform.localPosition;
        pos.y += 0.375f;

        RaycastHit2D recongnizeBox = Physics2D.BoxCast(pos, new Vector2(2.0f, 0.75f),
            0.0f, Vector3.down, 0.8f, LayerMask.GetMask("Player"));

        RaycastHit2D recongnizeRange = Physics2D.CircleCast(pos, 1.5f, Vector2.zero, 2.0f, LayerMask.GetMask("Player"));

        if (recongnizeBox.transform != null && canAttack)
        {
            IsAttack = true;
            CanAttack = false;
            animator.SetBool("IsMeleeAttack", true);
        }
        else if(recongnizeRange.transform != null && canAttack)
        {
            IsAttack = true;
            CanAttack = false;
            animator.SetBool("IsRangeAttack", true);
        }
    }

    private void CheckAttackTime()
    {
        if (!canAttack)
        {
            attackDelay -= Time.deltaTime;

            if (attackDelay <= 0.0f)
            {
                CanAttack = true;
                attackDelay = attackSpeed;
            }
        }
    }

    // 애니메이션 관련 함수
    private void MeleeAttack()
    {
        Instantiate(objStamp, transform.position, Quaternion.identity, trsObjEffect);
    }

    private void EndMeleeAttack()
    {
        IsAttack = false;
        animator.SetBool("IsMeleeAttack", false);
    }

    private void RangeAttack()
    {
        for (int i = 0; i < 6; ++i)
        {
            Instantiate(objBullet, trsMouse.position, Quaternion.Euler(0.0f, 0.0f, 60.0f * i), trsObjEffect);
        }        
    }

    private void EndRangeAttack()
    {
        IsAttack = false;
        animator.SetBool("IsRangeAttack", false);
    }
}
