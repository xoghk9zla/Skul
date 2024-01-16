using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ThrowBone : MonoBehaviour
{
    [SerializeField] float cooldown_SkillA = 6.0f;
    [SerializeField] float skillADamage = 3.0f;

    public float SkillADamage
    {
        set
        {
            skillADamage = value;
            if (player != null)
            {
                player.SkillADamage = value;
            }
        }
    }
    
    public float Cooldown_SkillA
    {
        set
        {
            cooldown_SkillA = value;
            if (player != null)
            {
                player.Cooldown_SkillA = value;
            }
        }
    }

    [SerializeField] GameObject objThrowBone;
    [SerializeField] Transform trsHead;
    private Transform trsObjDynamic;

    private SkillManager skillManager;
    private Animator animator;
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        SkillADamage = skillADamage;
        Cooldown_SkillA = cooldown_SkillA;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject objDynamic = GameObject.Find("ObjectDynamic");
        trsObjDynamic = objDynamic.transform;
        
        skillManager = SkillManager.Instance;

        player.SetPrepareAction(GetSkillADamageValue);
        player.SetPrepareAction(GetCooldownSkillAValue);
    }

    private void GetSkillADamageValue()
    {
        SkillADamage = player.SkillADamage;
    }

    private void GetCooldownSkillAValue()
    {
        Cooldown_SkillA = player.Cooldown_SkillA;
    }

    // Update is called once per frame
    void Update()
    {
        ThrowBone();
    }

    private void ThrowBone()
    {
        int count = trsObjDynamic.childCount;

        if (Input.GetKeyDown(KeyCode.A) && count == 0 && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillA))
        {
            GameObject obj = Instantiate(objThrowBone, trsHead.position, Quaternion.identity, trsObjDynamic);
            ThrowBone sc = obj.GetComponent<ThrowBone>();
            sc.SetPlayer(player);

            bool isRight = (transform.localScale.x == 1.0f);
            Vector2 throwForce = isRight ? new Vector2(5.0f, 0.0f) : new Vector2(-5.0f, 0.0f);

            sc.SkillSetting(throwForce, isRight, cooldown_SkillA);

            animator.SetBool("IsThrow", true);

            skillManager.ActiveSkill(SkillManager.SkillType.SkillA);
        }
    }

    // Animator 관련 함수들
    private void EndThrow()
    {
        animator.SetBool("IsThrow", false);
    }
}
