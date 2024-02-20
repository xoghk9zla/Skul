using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public enum BuffList
    {
        AttackSpeed, CriticalChance, Health, MeleeAttackDamage, SkillAttackDamage,
    }

    public class BuffStats
    {
        public float AttackSpeed = 1.3f, Critical = 0.15f, Hp = 50.0f, AttackDamage = 1.3f, SkillDamage = 1.2f;
    }


}
