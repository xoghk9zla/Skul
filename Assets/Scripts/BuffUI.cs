using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [SerializeField] Image img_buff;
    [SerializeField] Image img_bgbuff;

    private float maxBuffTime;
    private float remainBuffTime;

    public void SetBuffImgae()
    {
        img_buff.sprite = Resources.Load<Sprite>("Sprites/NPC/FogWolf/UI/FogWolf_CriticalChance") as Sprite;
        img_bgbuff.sprite = Resources.Load<Sprite>("Sprites/NPC/FogWolf/UI/FogWolf_CriticalChance") as Sprite;
    }
}
