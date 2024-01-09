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
                    // ���� �ӵ� 30% ����
                    break;
                case BuffList.CriticalChance:
                    // ġ��Ÿ Ȯ�� 15% ����
                    break;
                case BuffList.Health: 
                    // �ִ� ü�� 50 ����
                    break;
                case BuffList.MeleeAttackDamage: 
                    // ���� ���ݷ� 20% ����
                    break;
                case BuffList.SkillAttackDamage:
                    // ���� ���ݷ� 30% ����
                    break;
                default: 
                    break;
            }

        }
        
    }
}
