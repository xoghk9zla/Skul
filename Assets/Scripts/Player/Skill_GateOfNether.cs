using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_GateOfNether : MonoBehaviour
{
    [SerializeField] float cooldown_SkillS = 35.0f;
    [SerializeField] float skillSDamage = 15.0f;
    private float remainSkillTime = 12.0f;
    private float curSkillTime;

    public float SkillSDamage
    {
        set
        {
            skillSDamage = value;
            if (player != null)
            {
                player.SkillSDamage = value;
            }
        }
    }

    public float Cooldown_SkillS
    {
        set
        {
            cooldown_SkillS = value;
            if (player != null)
            {
                player.Cooldown_SkillS = value;
            }
        }
    }

    [SerializeField] GameObject objGateOfNether;
    [SerializeField] Transform trsObjDynamic;

    [SerializeField] private SkillManager skillManager;
    private Animator animator;
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        SkillSDamage = skillSDamage;
        Cooldown_SkillS = cooldown_SkillS;
        
        curSkillTime = remainSkillTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject objDynamic = GameObject.Find("ObjectDynamic");
        trsObjDynamic = objDynamic.transform;

        skillManager = SkillManager.Instance;

        player.SetPrepareAction(GetSkillSDamageValue);
        player.SetPrepareAction(GetCooldownSkillSValue);
    }

    private void GetSkillSDamageValue()
    {
        SkillSDamage = player.SkillSDamage;
    }

    private void GetCooldownSkillSValue()
    {
        Cooldown_SkillS = player.Cooldown_SkillS;
    }

    // Update is called once per frame
    void Update()
    {
        GateOfNether();
    }

    private void GateOfNether() 
    {
        if (Input.GetKeyDown(KeyCode.S) && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillS))
        {
            Vector3 playerPos = transform.localPosition;
            playerPos.y += transform.gameObject.GetComponent<BoxCollider2D>().size.y / 2.0f;

            RaycastHit2D checkGround = Physics2D.Raycast(playerPos, Vector2.down, 0.9f, LayerMask.GetMask("Ground"));
            if (checkGround.transform != null && checkGround.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Vector3 spawnPos = checkGround.point;
                spawnPos.y -= 0.15f;

                Instantiate(objGateOfNether, spawnPos, Quaternion.identity, trsObjDynamic);

                animator.SetBool("SkillGateOfNether", true);

                skillManager.ActiveSkill(SkillManager.SkillType.SkillS);
            }            
        }
    }

    private void EndGateOfNether()
    {
        animator.SetBool("SkillGateOfNether", false);
    }
}
