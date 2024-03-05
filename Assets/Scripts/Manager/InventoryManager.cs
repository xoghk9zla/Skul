using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private List<Item> listItems = new List<Item>();

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in listItems)
        {
            item.IsEquip = true;
            item.ActionInvoke();
        }
    }

    public void AddItem(Item _item)
    {        
        listItems.Add(_item);
    }
}
