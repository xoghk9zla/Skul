using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static BuffUI Instance;

    [SerializeField] Image img_buff;
    [SerializeField] Image img_bgbuff;

    [SerializeField] Sprite[] buffSprites;

    private float maxBuffTime;
    private float remainBuffTime;

    private bool isActive = false;

    [SerializeField] TMP_Text txtBuffToolTip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        txtBuffToolTip.gameObject.SetActive(true);        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        txtBuffToolTip.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        BuffTimer();
    }

    public void SetBuff(BuffManager.BuffList _type, float _buffTime)
    {
        isActive = true;

        maxBuffTime = _buffTime;
        remainBuffTime = maxBuffTime;

        img_buff.sprite = buffSprites[(int)_type];
        img_bgbuff.sprite = buffSprites[(int)_type];

        switch (_type)
        {
            case BuffManager.BuffList.AttackSpeed:
                txtBuffToolTip.text = "���� �ӵ� 30% ����";
                break;
            case BuffManager.BuffList.CriticalChance:
                txtBuffToolTip.text = "ũ��Ƽ�� Ȯ�� 15% ����";
                break;
            case BuffManager.BuffList.Health:
                txtBuffToolTip.text = "�ִ� ü�� 50 ����";
                break;
            case BuffManager.BuffList.MeleeAttackDamage:
                txtBuffToolTip.text = "�⺻ ���ݷ� 30% ����";
                break;
            case BuffManager.BuffList.SkillAttackDamage:
                txtBuffToolTip.text = "��ų ���ݷ� 20% ����";
                break;
            default:
                break;
        }
    }

    private void BuffTimer()
    {
        if (isActive && maxBuffTime != 0.0f)
        {
            remainBuffTime -= Time.deltaTime;
            img_buff.fillAmount = remainBuffTime / maxBuffTime;
            
            if(remainBuffTime < 0)
            {
                remainBuffTime = 0.0f;
                isActive = false;
                Destroy(gameObject);
            }
        }
    }
}
