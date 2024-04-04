using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProduction : MonoBehaviour
{
    [SerializeField] GameObject[] objStool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ġ ���� �ʿ�(���� ��ȭ ������ ���� ���� ��)
        foreach(var stool in objStool)
        {
            Animator anim = stool.GetComponent<Animator>();
            anim.SetBool("IsGrowUp", true);
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
