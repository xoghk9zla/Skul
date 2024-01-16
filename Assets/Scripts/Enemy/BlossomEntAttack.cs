using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomEntAttack : MonoBehaviour
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

    [SerializeField] private GameObject objBloosomSmoke;
    [SerializeField] private Transform trsBloosom;
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

        RaycastHit2D recongnizeRange = Physics2D.BoxCast(pos, new Vector2(2.0f, 0.75f),
            0.0f, Vector3.down, 0.8f, LayerMask.GetMask("Player"));

        if (recongnizeRange.transform != null && canAttack)
        {
            IsAttack = true;
            CanAttack = false;
            animator.SetBool("IsAttack", true);
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
    private void StartAttack()
    {

    }

    private void EndAttack()
    {
        IsAttack = false;
        animator.SetBool("IsAttack", false);

        Instantiate(objBloosomSmoke, trsBloosom.position, Quaternion.identity, trsObjEffect);
    }
}
