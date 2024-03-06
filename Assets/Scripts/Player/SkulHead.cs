using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulHead : MonoBehaviour
{
    [SerializeField] GameObject InteractUI;

    public enum SkulType
    {
        LittleBone, Hunter, GrimReaper,
    }

    public SkulType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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
}
