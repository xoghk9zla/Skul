using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DeathShadowSpirit : MonoBehaviour
{
    private bool isEquip;
    private Item item;

    [SerializeField] GameObject prefabDeathShadowSpirit;
    GameObject objDeathShadowSpirit;


    public bool IsEquip
    {
        set
        {
            isEquip = value;
            if (item != null)
            {
                item.IsEquip = value;
            }
        }
    }

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {
        item.SetPrepareAction(GetIsEquipValue);        
    }

    private void GetIsEquipValue()
    {
        IsEquip = item.IsEquip;
        SummonDeathShadowSpirit();
    }

    public void SummonDeathShadowSpirit()
    {
        if(isEquip)
        {
            if(objDeathShadowSpirit == null)
            {
                objDeathShadowSpirit = Instantiate(prefabDeathShadowSpirit);
            }
            else
            {
                objDeathShadowSpirit.SetActive(true);
            }
        }
        else
        {
            if(objDeathShadowSpirit != null)
            {
                objDeathShadowSpirit.SetActive(false);
            }
        }
    }
}
