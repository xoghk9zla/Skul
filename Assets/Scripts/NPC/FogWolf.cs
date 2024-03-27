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
    TextMeshProUGUI dialogUIText;
    string dialogText = "";
    bool isDialog = false;

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

        dialogUIText = objDialogUI.GetComponentInChildren<TextMeshProUGUI>();             

        if (bufftype == BuffManager.BuffList.AttackSpeed)
        {
            buffText.text = "공격 속도 증가";
            dialogText = "깊은 숲의 안개신이시여. 꼬마 스켈레톤이 누구보다 빠르게 적을 상대할 수 있도록, 날렵한 속도의 가호를 내려주소서.";
        }
        else if (bufftype == BuffManager.BuffList.CriticalChance)
        {
            buffText.text = "치명타 확률 증가";
            dialogText = "깊은 숲의 안개신이시여. 꼬마 스켈레톤이 적들의 약점을 누구보다 쉽게 찾을 수 있도록, 섬세한 집중의 가호를 내려주소서.";
        }
        else if (bufftype == BuffManager.BuffList.Health)
        {
            buffText.text = "최대 체력 증가";
            dialogText = "깊은 숲의 안개신이시여. 꼬마 스켈레톤이 적들의 공격을 쉽게 버틸 수 있도록, 단단한 활력의 가호를 내려주소서.";
        }
        else if (bufftype == BuffManager.BuffList.MeleeAttackDamage)
        {
            buffText.text = "평타 데미지 증가";
            dialogText = "깊은 숲의 안개신이시여. 꼬마 스켈레톤이 적들을 가볍게 제압할 수 있도록, 강력한 힘이 있는 전투의 가호를 내려주소서.";
        }
        else if (bufftype == BuffManager.BuffList.SkillAttackDamage)
        {
            buffText.text = "스킬 데미지 증가";
            dialogText = "깊은 숲의 안개신이시여. 꼬마 스켈레톤이 적들에게 강력한 주문을 외울 수 있도록, 그의 영혼에 강한 지혜의 가호를 내려주소서.";
        }
    }

    public void Dialog()
    {   
        if(!isDialog)
        {
            isDialog = true;
            objDialogUI.SetActive(true);
            if(isGiveBuff)
            {
                dialogText = "잘 부탁하네";
            }
            StartCoroutine(ShowText(dialogText));
        }        
    }

    IEnumerator ShowText(string _text)
    {
        dialogUIText.text = "";

        foreach (var text in _text)
        {
            dialogUIText.text += text;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.0f);
        objDialogUI.SetActive(false);
        isDialog = false;

        if (!isGiveBuff)
        {
            GiveBuff();
        }
    }

    private void GiveBuff()
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
