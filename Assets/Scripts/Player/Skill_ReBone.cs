using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ReBone : MonoBehaviour
{
    [SerializeField] float cooldownTime = 3.0f;
    [SerializeField] GameObject objReboneEffect;
    [SerializeField] Transform trsObjDynamic;

    [SerializeField] private SkillManager skillManager;
    private Animator animator;
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject objDynamic = GameObject.Find("ObjectDynamic");
        trsObjDynamic = objDynamic.transform;

        skillManager = SkillManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ReBone();
    }

    private void ReBone()
    {
        int count = trsObjDynamic.childCount;

        if (Input.GetKeyDown(KeyCode.S) && count != 0 && !animator.GetBool("IsRebone") && !skillManager.GetActiveSkill(SkillManager.SkillType.SkillS))
        {
            Vector3 effectPos = transform.position;
            effectPos.y += 0.2f;

            GameObject obj = Instantiate(objReboneEffect, effectPos, Quaternion.identity, trsObjDynamic);
            Transform trsThrowBorn = trsObjDynamic.GetChild(0);
            Vector3 movePos = trsThrowBorn.position;

            movePos.y += 0.05f;
            transform.position = movePos;

            Instantiate(objReboneEffect, transform.position, Quaternion.identity, trsObjDynamic);

            animator.SetBool("IsRebone", true);
            animator.SetBool("IsThrow", false);

            skillManager.ActiveSkill(SkillManager.SkillType.SkillS);
        }
    }

    // Animator 관련 함수들
    private void EndReborn()
    {
        animator.SetBool("IsRebone", false);
    }
}
