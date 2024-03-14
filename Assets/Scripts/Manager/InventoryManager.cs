using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private List<Item> listItems = new List<Item>();

    public class ItemStat
    {
        public float AttackSpeed = 0.0f, Critical = 0.0f, Hp = 0.0f, AttackDamage = 0.0f, SkillDamage = 0.0f;
    }
    ItemStat stat = new ItemStat();

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

    public void SetStat(ItemStat _stat)
    {
        stat.AttackSpeed += _stat.AttackSpeed;
        stat.Critical += _stat.Critical;
        stat.Hp += _stat.Hp;
        stat.AttackDamage += _stat.AttackDamage;
        stat.SkillDamage += _stat.SkillDamage;
    }
}
