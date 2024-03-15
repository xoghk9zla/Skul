using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStat : MonoBehaviour
{
    public static ItemStat Instance;

    [SerializeField] float attackSpeed = 1.0f;
    [SerializeField] float critical = 0.0f;
    [SerializeField] float hp = 0.0f;
    [SerializeField] float attackDamage = 0.0f;
    [SerializeField] float skillDamage = 0.0f;

    private void Awake()
    {
        Instance = this;
    }

    public enum stat
    {
        attackSpeed, critical, hp, attackDamage, skillDamage,
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetCritical()
    {
        return critical;
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public float GetSkillDamage()
    {
        return skillDamage;
    }

    public void SetStat(stat _stat, float _value)
    {
        switch (_stat)
        {
            case stat.attackSpeed:
                attackSpeed += _value;
                break;
            case stat.critical:
                critical += _value;
                break;
            case stat.hp:
                hp += _value;
                break;
            case stat.attackDamage:
                attackDamage += _value;
                break;
            case stat.skillDamage:
                skillDamage += _value;
                break;
            default:
                break;

        }
    }
}
