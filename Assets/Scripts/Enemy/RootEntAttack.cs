using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEntAttack : MonoBehaviour
{
    private Vector3 playerPos;
    private bool isAttack = false;
    private bool canAttack = true;
    [SerializeField] private float attackSpeed;
    private float attackDelay;


    private Animator animator;

    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject objRootAttackSign;
    [SerializeField] Transform trsObjDynamic;

    private void OnDrawGizmos()
    {
        RaycastHit2D recongnizeRange = Physics2D.BoxCast(transform.localPosition, new Vector2(2.5f, 0.75f),
            0.0f, Vector2.down, 0.3f, LayerMask.GetMask("Player"));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * recongnizeRange.distance, new Vector2(2.5f, 0.75f));
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackDelay = attackSpeed;
    }

    private void Update()
    {
        RecognizePlayer();
        CheckAttackTime();
    }

    private void RecognizePlayer()
    {
        RaycastHit2D recongnizeRange = Physics2D.BoxCast(transform.localPosition, new Vector2(2.5f, 0.75f),
            0.0f, Vector3.down, 0.3f, LayerMask.GetMask("Player"));

        if (recongnizeRange.transform != null && recongnizeRange.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canAttack)
            {
                playerPos = recongnizeRange.transform.localPosition;
                RaycastHit2D CheckGround = Physics2D.Raycast(playerPos, Vector2.down, 0.75f, LayerMask.GetMask("Ground"));
       
                if (CheckGround.transform != null && CheckGround.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {   
                    isAttack = true;
                    canAttack = false;
                    animator.SetBool("IsAttack", true);

                    Vector2 rootSpawnPos = CheckGround.point;
                    rootSpawnPos.y -= 0.05f;
                    Instantiate(objRootAttackSign, rootSpawnPos, Quaternion.identity, trsObjDynamic);
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
                canAttack = true;
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
        isAttack = false;
        animator.SetBool("IsAttack", false);
    }
}
