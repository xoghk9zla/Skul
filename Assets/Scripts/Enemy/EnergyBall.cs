using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnergyBall : MonoBehaviour
{
    private float damage = 7.0f;
    private float bulletSpeed = 2.0f;

    private Player player;

    private void OnDrawGizmos()
    {
        RaycastHit2D recongnizeRange = Physics2D.CircleCast(transform.position, 0.7f, Vector2.up, 0.4f, LayerMask.GetMask("Player"));
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player Sc = collision.GetComponent<Player>();
            Sc.Hit(damage);

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    private void Moving()
    {     
        RaycastHit2D recongnizeRange = Physics2D.CircleCast(transform.position, 0.7f, Vector2.zero, 0.7f, LayerMask.GetMask("Player"));
        
        if(recongnizeRange.transform != null)
        {
            player = recongnizeRange.transform.GetComponent<Player>();            
        }
          
        if(player != null)
        {
            float distance = Mathf.Abs((transform.position - player.transform.position).magnitude);
            
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * bulletSpeed);
        }
        else
        {
            transform.localPosition += transform.right * Time.deltaTime * bulletSpeed;
        }
    } 

}
