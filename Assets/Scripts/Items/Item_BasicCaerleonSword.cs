using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BasicCaerleonSword : MonoBehaviour
{
    private bool isEquip;
    private Item item;

    float attackDamage = 0.2f;

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
        if (!isEquip)
        {
            EquipItem();
        }
    }

    public void EquipItem()
    {
        ItemStat.Instance.SetStat(ItemStat.stat.attackDamage, attackDamage);
    }
}
