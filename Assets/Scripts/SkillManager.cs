using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public enum SkillType
    {
        SkillA,
        SkillS,
    }

    public static SkillManager Instance;

    [SerializeField] Image imgSkul;
    [SerializeField] Image imgSkillA;
    [SerializeField] Image imgSkillS;
    [SerializeField] Image bgSkillA;
    [SerializeField] Image bgSkillS;

    private float curCooldownTimeA;
    private float curCooldownTimeS;

    private float maxCooldownTimeA;
    private float maxCooldownTimeS;

    private bool isActiveA;
    private bool isActiveS;

    [SerializeField] private GameObject objPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Player objSc = objPlayer.GetComponent<Player>();
        objSc.SetSkill(this);

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ActiveCoolTime();
    }

    public void SetSkill(float _cooldownTimeA, float _cooldownTimeS)
    {
        maxCooldownTimeA = _cooldownTimeA;
        maxCooldownTimeS = _cooldownTimeS;

        curCooldownTimeA = maxCooldownTimeA;
        curCooldownTimeS = maxCooldownTimeS;
    }

    public void ActiveSkill(SkillType _type)
    {
        if (_type == SkillType.SkillA)
        {
            isActiveA = true;
            curCooldownTimeA = maxCooldownTimeA;
        }
        else if (_type == SkillType.SkillS)
        {
            isActiveS = true;
            curCooldownTimeS = maxCooldownTimeS;
        }
    }

    private void ActiveCoolTime()
    {
        if (isActiveA)
        {
            curCooldownTimeA -= Time.deltaTime;
            bgSkillA.fillAmount = curCooldownTimeA / maxCooldownTimeA;

            if(curCooldownTimeA <= 0.0f)
            {
                bgSkillA.fillAmount = 0.0f;
                curCooldownTimeA = 0.0f;
                isActiveA = false;
            }
        }
        if (isActiveS)
        {
            curCooldownTimeS -= Time.deltaTime;
            bgSkillS.fillAmount = curCooldownTimeS / maxCooldownTimeS;

            if (curCooldownTimeS <= 0.0f)
            {
                bgSkillS.fillAmount = 0.0f;
                curCooldownTimeS = 0.0f;
                isActiveS = false;
            }
        }
    }

    public bool GetActiveSkill(SkillType _type)
    {
        if(_type == SkillType.SkillA)
        {
            return isActiveA;
        }
        else if(_type == SkillType.SkillS)
        {
            return isActiveS;
        }
        else
        {
            return false;
        }
    }

    public void ResetCoolTime(SkillType _type)
    {
        if(_type == SkillType.SkillA)
        {
            bgSkillA.fillAmount = 0.0f;
            curCooldownTimeA = 0.0f;
            isActiveA = false;

        }
        else if (_type == SkillType.SkillS)
        {
            bgSkillS.fillAmount = 0.0f;
            curCooldownTimeS = 0.0f;
            isActiveS = false;
        }
    }    
}
