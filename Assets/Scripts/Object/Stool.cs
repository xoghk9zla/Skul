using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stool : MonoBehaviour
{
    private void Awake()
    {
        transform.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Active()
    {
        transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
