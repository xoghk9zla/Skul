using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ThrowBone : MonoBehaviour
{
    [SerializeField] float cooldownTime = 6.0f;

    [SerializeField] GameObject objThrowBone;
    [SerializeField] Transform trsHead;
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

            sc.SkillSetting(throwForce, isRight, cooldownTime);

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
