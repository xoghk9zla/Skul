using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using static UnityEditor.PlayerSettings;

public class Skill_Harvest : MonoBehaviour
{
    [SerializeField] float cooldown_SkillA = 15.0f;
    [SerializeField] float skillADamage = 30.0f;

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

    [SerializeField] GameObject objHarvestHit;

    private Transform trsObjDynamic;

    [SerializeField] private SkillManager skillManager;
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
        Harvest();
    }

    private void Harvest()
    {
        if (Input.GetKeyDown(KeyCode.A) && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillA))
        {           
            animator.SetBool("SkillHarvest", true);

            skillManager.ActiveSkill(SkillManager.SkillType.SkillA);
        }
    }

    // Animator 관련 함수들
    private void StartHarvest()
    {
        // 범위 내 적 찾아서 데미지
        var height = Camera.main.orthographicSize / 2;
        var width = height * Camera.main.aspect;
        RaycastHit2D[] recongnizeBox = Physics2D.BoxCastAll(Camera.main.gameObject.transform.position, new Vector2(width, height), 0.0f, Vector2.down, 0.8f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit2D enemy in recongnizeBox)
        {
            Enemy Sc = enemy.collider.gameObject.GetComponent<Enemy>();
            Sc.Hit(skillADamage);

            Vector3 effectPos = Sc.transform.gameObject.GetComponent<BoxCollider2D>().bounds.center;
            effectPos.x += objHarvestHit.transform.gameObject.GetComponent<BoxCollider2D>().size.x / 4 * -Sc.gameObject.transform.localScale.x;

            Instantiate(objHarvestHit, effectPos, Quaternion.identity, Sc.transform);
        }
    }

    
    private void EndHarvest()
    {
        animator.SetBool("SkillHarvest", false);
    }
}
