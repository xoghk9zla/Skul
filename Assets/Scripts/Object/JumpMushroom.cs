using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMushroom : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.transform != null && collision.transform.gameObject.layer == LayerMask.NameToLayer("Player") && !player.IsGround)
        {
            animator.SetBool("IsActive", true);
        }
    }

    private void EndActive()
    {
        animator.SetBool("IsActive", false);
    }
}
