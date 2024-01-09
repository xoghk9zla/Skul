using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWolf : MonoBehaviour
{
    private bool isGiveBuff = false;
    [SerializeField] GameObject InteractUI;

    public enum BuffList
    {
        AttackSpeed, CriticalChance, Health, MeleeAttackDamage, SkillAttackDamage,
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InteractUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InteractUI.SetActive(false);
        }
    }

    public void GiveBuff()
    {
        if (!isGiveBuff)
        {
            isGiveBuff = true;

            BuffList bufftype = (BuffList)UnityEngine.Random.Range(0, (Enum.GetNames(typeof(BuffList)).Length));
            //Debug.Log(bufftype.ToString());
            switch (bufftype)
            {
                case BuffList.AttackSpeed:
                    // 공격 속도 30% 증가
                    break;
                case BuffList.CriticalChance:
                    // 치명타 확률 15% 증가
                    break;
                case BuffList.Health: 
                    // 최대 체력 50 증가
                    break;
                case BuffList.MeleeAttackDamage: 
                    // 물리 공격력 20% 증폭
                    break;
                case BuffList.SkillAttackDamage:
                    // 마법 공격력 30% 증폭
                    break;
                default: 
                    break;
            }

        }
        
    }
}
