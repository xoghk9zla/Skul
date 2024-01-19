using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Switch : MonoBehaviour
{
    private SkillManager skillManager;
    private Animator animator;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        skillManager = SkillManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndSwitch()
    {
        animator.SetBool("Switch", false);
    }
}
