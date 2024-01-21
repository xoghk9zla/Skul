using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GateOfNether : MonoBehaviour
{
    private float remainTime = 12.0f;
    private float skillDamage = 15.0f;
    private BoxCollider2D boxcoll;
    public float SkillDamage
    {
        set
        {
            skillDamage = value;
            if (player != null)
            {
                player.SkillSDamage = value;
            }
        }
    }

    private float attackDelay = 1.0f;
    private bool canDamage = true;

    Player player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && canDamage)
        {
            // canDamage가 collision별로 따로 적용이 안되고 있음
            Enemy Sc = collision.GetComponent<Enemy>();
            Sc.Hit(skillDamage);
            canDamage = false;
        }
    }

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            player.SetPrepareAction(GetSkillADamageValue);
        }
    }

    private void GetSkillADamageValue()
    {
        SkillDamage = player.SkillADamage;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttackDelay();
        SetTimer();
        //tickDamage();
    }

    private void CheckAttackDelay()
    {
        if (!canDamage)
        {
            attackDelay -= Time.deltaTime;

            if (attackDelay < 0.0f)
            {
                canDamage = true;
                attackDelay = 1.0f;
            }
        }
    }

    private void SetTimer()
    {
        remainTime -= Time.deltaTime;

        if (remainTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void tickDamage()
    {
        boxcoll = GetComponent<BoxCollider2D>();
        Collider2D[] enemys = Physics2D.OverlapBoxAll(boxcoll.bounds.center, boxcoll.bounds.size, 0f, LayerMask.GetMask("Enemy"));
    }
}
