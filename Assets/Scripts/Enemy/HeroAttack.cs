using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroAttack : MonoBehaviour
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

    private Animator animator;
    private Enemy enemy;

    [SerializeField] GameObject objEnergyBall;
    private Transform trsHand;

    [SerializeField] GameObject objEnergyBlast;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        attackDelay = attackSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enemy != null)
        {
            enemy.SetPrepareAction(GetIsAttackValue);
            enemy.SetPrepareAction(GetCanAttackValue);
        }
        GameObject objHand = GameObject.Find("trsHand");
        trsHand = objHand.transform;
    }

    private void GetIsAttackValue()
    {
        IsAttack = enemy.IsAttack;
    }

    private void GetCanAttackValue()
    {
        CanAttack = enemy.CanAttack;
    }

    // Update is called once per frame
    void Update()
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
            animator.SetBool("IsDash", false);
            animator.SetBool("Skill_EnergyExplosion", true);            
        }
        else if (recongnizeRange.transform != null && canAttack)
        {
            IsAttack = true;
            CanAttack = false;
            animator.SetBool("IsDash", false);
            animator.SetBool("Skill_EnergyBall", true);
        }
        else if(recongnizeBox.transform == null && recongnizeRange.transform == null && canAttack)
        {
            var height = Camera.main.orthographicSize / 2;
            var width = height * Camera.main.aspect;

            RaycastHit2D recongnize = Physics2D.BoxCast(Camera.main.gameObject.transform.position, new Vector2(width, height), 0.0f, Vector2.down, 0.8f, LayerMask.GetMask("Player"));
            transform.position = Vector3.Lerp(transform.position, recongnize.transform.position, 0.01f);            
            
            animator.SetBool("IsDash", true);
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

    private void Dash()
    {
        animator.SetBool("IsDash", true);
    }

    private void StartEnergyBallAttack()
    {
        GameObject objEBall = Instantiate(objEnergyBall, trsHand.position, Quaternion.identity);
        EnergyBall Sc = objEBall.GetComponent<EnergyBall>();
        Sc.SetEnemy(enemy);
    }

    private void StartEnergyExplosionAttack()
    {
        BoxCollider2D hitbox = enemy.GetComponent<BoxCollider2D>();
        Vector3 pos = transform.position;
        pos.y -= hitbox.bounds.size.y / 2;

        GameObject objEBlast = Instantiate(objEnergyBlast, pos, Quaternion.identity);
    }

    private void EndEnergyBallAttack()
    {
        IsAttack = false;
        animator.SetBool("Skill_EnergyBall", false);
    }

    private void EndEnergyExplosionAttack()
    {
        IsAttack = false;
        animator.SetBool("Skill_EnergyExplosion", false);
    }
}
