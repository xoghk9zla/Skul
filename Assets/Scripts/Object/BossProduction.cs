using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProduction : MonoBehaviour
{
    [SerializeField] GameObject[] objStool;
    [SerializeField] GameObject objBoss;
    bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (!isActive)
        {
            isActive = true;

            foreach (var stool in objStool)
            {
                Animator anim = stool.GetComponent<Animator>();
                anim.SetBool("IsGrowUp", true);
            }

            Instantiate(objBoss);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
