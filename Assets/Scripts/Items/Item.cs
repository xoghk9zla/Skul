using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject InteractUI;
    private bool isEquip = false;

    public bool IsEquip 
    {
        get => isEquip;
        set
        {
            isEquip = value;
        }
    }

    private UnityAction prepareAction;
    public void SetPrepareAction(UnityAction _action)
    {
        prepareAction += _action;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player sc = collision.gameObject.GetComponent<Player>();
            sc.objInteraction = this.gameObject;

            InteractUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player sc = collision.gameObject.GetComponent<Player>();
            sc.objInteraction = null;

            InteractUI.SetActive(false);
        }
    }

    public void ActionInvoke()
    {
        if(prepareAction != null)
        {
            prepareAction.Invoke();
        }        
    }

    public void Interaction()
    {
        InventoryManager.Instance.AddItem(this);
        gameObject.SetActive(false);    // InventoryManager의 리스트에 들어가 있기 위함
        //Destroy(gameObject);
    }
}
