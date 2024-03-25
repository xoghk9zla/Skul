using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FogWolf : MonoBehaviour
{
    private bool isGiveBuff = false;
    [SerializeField] GameObject InteractUI;
    [SerializeField] GameObject objBuffText;
    [SerializeField] TextMeshPro buffText;

    [SerializeField] Transform trsBuffUI;
    [SerializeField] GameObject objBuffUI;
    [SerializeField] GameObject objBuffEffect;
    [SerializeField] GameObject objBuffReadyEffect;

    BuffManager.BuffList bufftype;
    [SerializeField] GameObject objDialogUI;
    TextMeshProUGUI dialogText;
    string sampleText = "";

    Player player;

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
        GameObject objPlayer = GameManager.Instance.GetPlayerObject();
        player = objPlayer.GetComponent<Player>();
        
        GameObject objBuff = GameObject.Find("BuffUI");
        trsBuffUI = objBuff.GetComponent<Transform>();

        bufftype = (BuffManager.BuffList)UnityEngine.Random.Range(0, (Enum.GetNames(typeof(BuffManager.BuffList)).Length));

        dialogText = objDialogUI.GetComponentInChildren<TextMeshProUGUI>();             

        if (bufftype == BuffManager.BuffList.AttackSpeed)
        {
            buffText.text = "���� �ӵ� ����";
            sampleText = "���� ���� �Ȱ����̽ÿ�. ���� ���̷����� �������� ������ ���� ����� �� �ֵ���, ������ �ӵ��� ��ȣ�� �����ּҼ�.";
        }
        else if (bufftype == BuffManager.BuffList.CriticalChance)
        {
            buffText.text = "ġ��Ÿ Ȯ�� ����";
            sampleText = "���� ���� �Ȱ����̽ÿ�. ���� ���̷����� ������ ������ �������� ���� ã�� �� �ֵ���, ������ ������ ��ȣ�� �����ּҼ�.";
        }
        else if (bufftype == BuffManager.BuffList.Health)
        {
            buffText.text = "�ִ� ü�� ����";
            sampleText = "���� ���� �Ȱ����̽ÿ�. ���� ���̷����� ������ ������ ���� ��ƿ �� �ֵ���, �ܴ��� Ȱ���� ��ȣ�� �����ּҼ�.";
        }
        else if (bufftype == BuffManager.BuffList.MeleeAttackDamage)
        {
            buffText.text = "��Ÿ ������ ����";
            sampleText = "���� ���� �Ȱ����̽ÿ�. ���� ���̷����� ������ ������ ������ �� �ֵ���, ������ ���� �ִ� ������ ��ȣ�� �����ּҼ�.";
        }
        else if (bufftype == BuffManager.BuffList.SkillAttackDamage)
        {
            buffText.text = "��ų ������ ����";
            sampleText = "���� ���� �Ȱ����̽ÿ�. ���� ���̷����� ���鿡�� ������ �ֹ��� �ܿ� �� �ֵ���, ���� ��ȥ�� ���� ������ ��ȣ�� �����ּҼ�.";
        }
    }

    public void Dialog()
    {   
        objDialogUI.SetActive(true);
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        dialogText.text = "";

        foreach (var text in sampleText)
        {
            dialogText.text += text;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.0f);
        objDialogUI.SetActive(false);

        GiveBuff();
    }

    private void GiveBuff()
    {
        if (!isGiveBuff)
        {
            isGiveBuff = true;
            
            player.SetBuffStats(bufftype);

            Vector3 buffEffcetPos = player.transform.localPosition;
            buffEffcetPos.y -= 1.5f;

            Vector3 buffEffectReadyPos = transform.localPosition;
            buffEffectReadyPos.x += 0.07f;
            buffEffectReadyPos.y -= 0.34f;

            Instantiate(objBuffEffect, buffEffcetPos, Quaternion.identity, player.transform);
            Instantiate(objBuffReadyEffect, buffEffectReadyPos, Quaternion.identity, transform.transform);

            GameObject obj = Instantiate(objBuffUI, trsBuffUI.position, Quaternion.identity, trsBuffUI);            
            BuffUI.Instance.SetBuff(bufftype, 0.0f);

            objBuffText.gameObject.SetActive(true);
            Destroy(objBuffText, 2.0f);
            
            InteractUI.SetActive(false);
        }
        
    }
}
