using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BasicCaerleonSword : MonoBehaviour
{
    private bool isEquip;
    private Item item;

    [SerializeField] InventoryManager.ItemStat stat;

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
        InventoryManager.Instance.SetStat(stat);
    }

}
