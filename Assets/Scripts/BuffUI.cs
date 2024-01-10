using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public static BuffUI Instance;

    [SerializeField] Image img_buff;
    [SerializeField] Image img_bgbuff;

    [SerializeField] Sprite[] buffSprites;

    private float maxBuffTime;
    private float remainBuffTime;

    private bool isActive = false;

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
