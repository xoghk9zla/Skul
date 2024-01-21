using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Switch : MonoBehaviour
{
    private SkillManager skillManager;
    private Animator animator;
    private Player player;

    [SerializeField] private BoxCollider2D attackBox;

    [SerializeField] private float cooldown_Switch = 10.0f;
    private bool isSwitching;

    public bool IsSwitching
    {
        set
        {
            isSwitching = value;
            if (player != null)
            {
                player.IsSwitching = value;
            }
        }
    }

    public float Cooldown_Switch
    {
        set
        {
            cooldown_Switch = value;
            if (player != null)
            {
                player.Cooldown_Switch = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        skillManager = SkillManager.Instance;

        Cooldown_Switch = cooldown_Switch;

        player.SetPrepareAction(GetIsSwitchingValue);
        player.SetPrepareAction(GetCooldown_SwitchValue);
    }

    private void GetIsSwitchingValue()
    {
        IsSwitching = player.IsSwitching;
    }

    private void GetCooldown_SwitchValue()
    {
        Cooldown_Switch = player.Cooldown_Switch;
    }
    // Update is called once per frame
    void Update()
    {
        LittleBoneSwitch();
    }

    private void LittleBoneSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& !skillManager.GetActiveSkill(SkillManager.SkillType.Switch))
        {
            GameManager.Instance.SwitchSkul();
            skillManager.ActiveSkill(SkillManager.SkillType.Switch);
        }
        if (isSwitching)
        {
            player.gameObject.transform.position += Vector3.right * Time.deltaTime * player.transform.localScale.x;
        }
        
    }

    private void StartSwitch()
    {
        attackBox.enabled = true;
    }

    private void EndSwitch()
    {
        attackBox.enabled = false;

        IsSwitching = false;
        animator.SetBool("Switch", false);
    }
}
