using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnergyBall : MonoBehaviour
{
    private float damage = 7.0f;
    private float bulletSpeed = 2.0f;
    private float timeToMove = 0.75f;

    private Player player;
    private Enemy enemy;    

    private void OnDrawGizmos()
    {
        RaycastHit2D recongnizeRange = Physics2D.CircleCast(transform.position, 1.0f, Vector2.up, 1.0f, LayerMask.GetMask("Player"));
        
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
        RaycastHit2D recongnizeRange = Physics2D.CircleCast(transform.position, 1.0f, Vector2.zero, 1.0f, LayerMask.GetMask("Player"));

        if (recongnizeRange.transform != null)
        {
            player = recongnizeRange.transform.GetComponent<Player>();
        }

        if (timeToMove > 0.0f)
        {
            transform.localPosition += transform.right * Time.deltaTime * bulletSpeed * enemy.transform.localScale.x;
        }
        else
        {
            if (player != null)
            {
                float distance = Mathf.Abs((transform.position - player.transform.position).magnitude);
                float rate = Mathf.Clamp(distance, 0.0f, 1.5f);
                // 베지에 곡선으로 변경 고려 중
                transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * bulletSpeed * (2.5f - rate));
                
            }
            else
            {                
                transform.localPosition += transform.right * Time.deltaTime * bulletSpeed * enemy.transform.localScale.x;
            }
        }

        timeToMove -= Time.deltaTime;        
    }

    public void SetEnemy(Enemy _enemy)
    {
        enemy = _enemy;
    }
}
