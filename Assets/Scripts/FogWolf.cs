using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FogWolf : MonoBehaviour
{
    private bool isGiveBuff = false;
    [SerializeField] GameObject InteractUI;

    [SerializeField] BuffManager buffManager;
    [SerializeField] Transform trsBuffUI;
    [SerializeField] GameObject objBuffUI;
    [SerializeField] GameObject objBuffEffect;
    [SerializeField] GameObject objBuffReadyEffect;

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

    private void Start()
    {
        buffManager = GetComponent<BuffManager>();
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
        }
        
    }
}
