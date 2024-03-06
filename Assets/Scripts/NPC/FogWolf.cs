using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FogWolf : MonoBehaviour
{
    private bool isGiveBuff = false;
    [SerializeField] GameObject InteractUI;
    [SerializeField] GameObject objBuffText;
    [SerializeField] TextMeshPro buffText;

    [SerializeField] BuffManager buffManager;
    [SerializeField] Transform trsBuffUI;
    [SerializeField] GameObject objBuffUI;
    [SerializeField] GameObject objBuffEffect;
    [SerializeField] GameObject objBuffReadyEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isGiveBuff)
        {
            InteractUI.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player sc = collision.gameObject.GetComponent<Player>();
            sc.objInteraction = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InteractUI.SetActive(false);
        }
    }

    private void Start()
    {
        buffManager = GetComponent<BuffManager>();
        GameObject objBuff = GameObject.Find("BuffUI");
        trsBuffUI = objBuff.GetComponent<Transform>();
    }

    public void GiveBuff(Player _player)
    {
        if (!isGiveBuff)
        {
            isGiveBuff = true;

            BuffManager.BuffList bufftype = (BuffManager.BuffList)UnityEngine.Random.Range(0, (Enum.GetNames(typeof(BuffManager.BuffList)).Length));
            _player.SetBuffStats(bufftype);

            Vector3 buffEffcetPos = _player.transform.localPosition;
            buffEffcetPos.y -= 1.5f;

            Vector3 buffEffectReadyPos = transform.localPosition;
            buffEffectReadyPos.x += 0.07f;
            buffEffectReadyPos.y -= 0.34f;

            Instantiate(objBuffEffect, buffEffcetPos, Quaternion.identity, _player.transform);
            Instantiate(objBuffReadyEffect, buffEffectReadyPos, Quaternion.identity, transform.transform);

            GameObject obj = Instantiate(objBuffUI, trsBuffUI.position, Quaternion.identity, trsBuffUI);            
            BuffUI.Instance.SetBuff(bufftype, 0.0f);

            objBuffText.gameObject.SetActive(true);

            if (bufftype == BuffManager.BuffList.AttackSpeed)
            {
                buffText.text = "공격 속도 증가";
            }
            else if (bufftype == BuffManager.BuffList.CriticalChance)
            {
                buffText.text = "치명타 확률 증가";
            }
            else if (bufftype == BuffManager.BuffList.Health)
            {
                buffText.text = "최대 체력 증가";
            }
            else if (bufftype == BuffManager.BuffList.MeleeAttackDamage)
            {
                buffText.text = "평타 데미지 증가";
            }
            else if (bufftype == BuffManager.BuffList.SkillAttackDamage)
            {
                buffText.text = "스킬 데미지 증가";
            }
            InteractUI.SetActive(false);
        }
        
    }
}
