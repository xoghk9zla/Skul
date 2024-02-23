using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathShadowSpirit : MonoBehaviour
{
    [SerializeField] Player player;

    // Update is called once per frame
    void Update()
    {
        Moving();
        Turn();
    }

    private void Moving()
    {
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        player = objPlayer.GetComponent<Player>();

        if (player != null)
        { 
            Vector3 pos = player.transform.position + new Vector3(-0.5f * player.transform.localScale.x, 0.5f);
            float distance = Mathf.Abs((transform.position - player.transform.position).magnitude);
            if (distance > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, pos, distance * Time.deltaTime);
            }
        }
    }

    private void Turn()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (dir.normalized.x * transform.localScale.x < 0.0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1.0f;
            transform.localScale = scale;
        }
    }
}
