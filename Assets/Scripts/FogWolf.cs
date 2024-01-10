using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWolf : MonoBehaviour
{
    private bool isGiveBuff = false;
    [SerializeField] GameObject InteractUI;

    [SerializeField] BuffManager buffManager;
    [SerializeField] BuffUI buffUI;

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
        buffUI = GetComponent<BuffUI>();
    }

    public void GiveBuff(Player _player)
    {
        if (!isGiveBuff)
        {
            isGiveBuff = true;

            BuffManager.BuffList bufftype = (BuffManager.BuffList)UnityEngine.Random.Range(0, (Enum.GetNames(typeof(BuffManager.BuffList)).Length));
            _player.SetBuffStats(bufftype);

            buffUI.SetBuffImgae();
        }
        
    }
}
