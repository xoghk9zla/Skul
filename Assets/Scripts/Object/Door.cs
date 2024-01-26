using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject objInteractUI;

    public enum DoorType
    {
        Normal, Skul, Item,
    }
    
    public DoorType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.CheckEmptyEnemyList())
        {
            objInteractUI.SetActive(true);
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && collision.gameObject.layer == LayerMask.NameToLayer("Player") && GameManager.Instance.CheckEmptyEnemyList())
        {
            if(type == DoorType.Normal)
            {

            }
            else if (type == DoorType.Skul)
            {

            }
            else if (type == DoorType.Item)
            {

            }
            SceneManager.LoadScene("Test");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objInteractUI.SetActive(false);
    }
}
