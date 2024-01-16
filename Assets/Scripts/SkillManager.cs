using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public enum SkillType
    {
        SkillA, SkillS,
    }

    public static SkillManager Instance;

    [SerializeField] Image imgSkul;
    [SerializeField] Image imgSkillA;
    [SerializeField] Image imgSkillS;
    [SerializeField] Image bgSkillA;
    [SerializeField] Image bgSkillS;

    private float curCooldown_SkillA;
    private float curCooldown_SkillS;

    private float cooldown_SkillA;
    private float cooldown_SkillS;

    private float skillADamage;
    private float skillSDamage;

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

    private bool isActiveA;
    private bool isActiveS;

    [SerializeField] private GameObject objPlayer;
    private Player player;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager manager = GameManager.Instance;
        objPlayer = manager.GetPlayerObject();
        player = objPlayer.GetComponent<Player>();
        player.SetSkill(this);

        Instance = this;

        curCooldown_SkillA = cooldown_SkillA;
        curCooldown_SkillS = cooldown_SkillS;
    }

    private void Start()
    {
        player.SetPrepareAction(GetSkillADamageValue);
        player.SetPrepareAction(GetSkillSDamageValue);
        player.SetPrepareAction(GetCooldownSkillAValue);
        player.SetPrepareAction(GetCooldownSkillSValue);
    }

    private void GetSkillADamageValue()
    {
        SkillADamage = player.SkillADamage;
    }

    private void GetSkillSDamageValue()
    {
        SkillSDamage = player.SkillSDamage;
    }

    private void GetCooldownSkillAValue()
    {
        Cooldown_SkillA = player.Cooldown_SkillA;
    }    

    private void GetCooldownSkillSValue()
    {
        Cooldown_SkillS = player.Cooldown_SkillS;
    }

    // Update is called once per frame
    void Update()
    {
        ActiveCoolTime();
    }

    public void ActiveSkill(SkillType _type)
    {
        if (_type == SkillType.SkillA)
        {
            isActiveA = true;
            curCooldown_SkillA = cooldown_SkillA;
        }
        else if (_type == SkillType.SkillS)
        {
            isActiveS = true;
            curCooldown_SkillS = cooldown_SkillS;
        }
    }

    private void ActiveCoolTime()
    {
        if (isActiveA)
        {
            curCooldown_SkillA -= Time.deltaTime;
            bgSkillA.fillAmount = curCooldown_SkillA / cooldown_SkillA;

            if(curCooldown_SkillA <= 0.0f)
            {
                bgSkillA.fillAmount = 0.0f;
                curCooldown_SkillA = 0.0f;
                isActiveA = false;
            }
        }
        if (isActiveS)
        {
            curCooldown_SkillS -= Time.deltaTime;
            bgSkillS.fillAmount = curCooldown_SkillS / cooldown_SkillS;

            if (curCooldown_SkillS <= 0.0f)
            {
                bgSkillS.fillAmount = 0.0f;
                curCooldown_SkillS = 0.0f;
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
            curCooldown_SkillA = 0.0f;
            isActiveA = false;

        }
        else if (_type == SkillType.SkillS)
        {
            bgSkillS.fillAmount = 0.0f;
            curCooldown_SkillS = 0.0f;
            isActiveS = false;
        }
    }    
}
