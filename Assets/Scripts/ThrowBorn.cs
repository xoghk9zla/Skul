using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBorn : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] private float returnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(5.0f, 0.0f), ForceMode2D.Impulse);
        rigid.AddTorque(-1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        SetTimer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigid != null && collision.gameObject.layer != LayerMask.GetMask("Ground")) 
        {
            rigid.gravityScale = 1.0f;
        }
    }

    private void SetTimer()
    {
        returnTime -= Time.deltaTime;

        if(returnTime <= 0.0f )
        {
            returnTime = 5.0f;            
        }
    }
}
