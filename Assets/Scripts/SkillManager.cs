using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkillManager : MonoBehaviour
{
    public enum SkillType
    {
        SkillA, SkillS, Switch,
    }

    public static SkillManager Instance;

    [SerializeField] Image imgSkul;
    [SerializeField] Image imgSkillA;
    [SerializeField] Image imgSkillS;
    [SerializeField] Image bgSkul;
    [SerializeField] Image bgSkillA;
    [SerializeField] Image bgSkillS;

    [SerializeField] GameObject btnSpace;

    private float curCooldown_SkillA;
    private float curCooldown_SkillS;
    private float curCooldown_Switch;

    private float cooldown_SkillA;
    private float cooldown_SkillS;
    private float cooldown_Switch;

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

    public float Cooldown_Switch
    {
        set
        {
            cooldown_Switch = value;
            if (player != null)
            {
                player.Cooldown_Switch = value;
            }
        }
    }

    private bool isActiveA;
    private bool isActiveS;
    private bool isActiveSwitch;

    [SerializeField] private GameObject objPlayer;
    private Player player;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager manager = GameManager.Instance;
        objPlayer = manager.GetPlayerObject();
        player = objPlayer.GetComponent<Player>();
        player.SetSkill(this);

        SetImage(objPlayer);

        Instance = this;

        curCooldown_SkillA = cooldown_SkillA;
        curCooldown_SkillS = cooldown_SkillS;
        curCooldown_Switch = cooldown_Switch;
    }

    private void Start()
    {
        player.SetPrepareAction(GetSkillADamageValue);
        player.SetPrepareAction(GetSkillSDamageValue);
        player.SetPrepareAction(GetCooldownSkillAValue);
        player.SetPrepareAction(GetCooldownSkillSValue);
        player.SetPrepareAction(GetCooldownSwitchValue);
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
    
    private void GetCooldownSwitchValue()
    {
        Cooldown_Switch = player.Cooldown_Switch;
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
        else if(_type == SkillType.Switch)
        {
            isActiveSwitch = true;
            curCooldown_Switch = cooldown_Switch;
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
        if (isActiveSwitch)
        {
            curCooldown_Switch -= Time.deltaTime;
            bgSkul.fillAmount = curCooldown_Switch / cooldown_Switch;

            if (curCooldown_Switch <= 0.0f)
            {
                bgSkul.fillAmount = 0.0f;
                curCooldown_Switch = 0.0f;
                isActiveSwitch = false;
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
        else if(_type == SkillType.Switch)
        {
            return isActiveSwitch;
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
        else if (_type == SkillType.Switch)
        {
            bgSkul.fillAmount = 0.0f;
            curCooldown_Switch = 0.0f;
            isActiveSwitch = false;
        }
    }    

    public void SetImage(GameObject _player)
    { 
        player = _player.GetComponent<Player>();

        if(GameManager.Instance.CheckEmptySkulList() == -1)
        {
            btnSpace.gameObject.SetActive(true);
        }

        imgSkul.sprite = player.skulImg;
        imgSkillA.sprite = player.skillAImg;
        imgSkillS.sprite = player.skillSImg;
        bgSkul.sprite = player.skulImg;
        bgSkillA.sprite = player.skillAImg;
        bgSkillS.sprite = player.skillSImg;

        bgSkul.color = new Color(40.0f / 255.0f, 40.0f / 255.0f, 40.0f / 255.0f);
        bgSkillA.color = new Color(40.0f / 255.0f, 40.0f / 255.0f, 40.0f / 255.0f);
        bgSkillS.color = new Color(40.0f / 255.0f, 40.0f / 255.0f, 40.0f / 255.0f);
    }
}
