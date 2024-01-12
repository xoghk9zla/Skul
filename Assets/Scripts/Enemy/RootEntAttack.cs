using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RootEntAttack : MonoBehaviour
{
    private Vector3 playerPos;
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

    private Animator animator;

    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject objRootAttackSign;
    [SerializeField] Transform trsObjDynamic;

    private Vector2 rootSpawnPos;
    private RaycastHit2D CheckGround;

    private Enemy enemy;
    /*
    private void OnDrawGizmos()
    {
        RaycastHit2D recongnizeRange = Physics2D.BoxCast(transform.localPosition, new Vector2(2.5f, 0.75f),
            0.0f, Vector2.down, 0.8f, LayerMask.GetMask("Player"));
        Vector3 pos = transform.localPosition;
        pos.y += 0.375f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, new Vector2(2.5f, 0.75f));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerPos + new Vector3(0, 0.15f, 0), playerPos - new Vector3(0, 0.75f, 0));
    }
    */
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        attackDelay = attackSpeed;
    }

    private void Start()
    {
        GameObject objDynamic = GameObject.Find("ObjectDynamic");
        trsObjDynamic = objDynamic.transform;

        if(enemy != null) 
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

        RaycastHit2D recongnizeRange = Physics2D.BoxCast(pos, new Vector2(2.5f, 0.75f),
            0.0f, Vector3.down, 0.8f, LayerMask.GetMask("Player"));

        if (recongnizeRange.transform != null && recongnizeRange.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canAttack)
            {
                playerPos = recongnizeRange.transform.localPosition;
                CheckGround = Physics2D.Raycast(playerPos + new Vector3(0, 0.15f, 0), Vector2.down, 0.9f, LayerMask.GetMask("Ground"));
       
                if (CheckGround.transform != null && CheckGround.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {   
                    IsAttack = true;
                    CanAttack = false;
                    animator.SetBool("IsAttack", true);

                    rootSpawnPos = CheckGround.point;
                    rootSpawnPos.y -= 0.05f;                    
                }
            }
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

        if(CheckGround.transform != null)
        {
            Instantiate(objRootAttackSign, rootSpawnPos, Quaternion.identity, trsObjDynamic);
        }
    }
}
