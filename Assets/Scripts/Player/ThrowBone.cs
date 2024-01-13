using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBone : MonoBehaviour
{
    private Rigidbody2D rigid;

    private Vector2 force;
    private bool isRight;
    private float cooldownTime;

    private float skillDamage = 3.0f;

    public float SkillDamage
    {
        set
        {
            skillDamage = value;
            if (player != null)
            {
                player.SkillDamage = value;
            }
        }
    }

    Player player;

    private void Awake()
    {
    }    

    public void SetPlayer(Player _player)
    {
        player = _player;        
    }

    private void GetSkillDamageValue()
    {
        SkillDamage = player.SkillDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x = isRight ? -1.0f : 1.0f;
        transform.localScale = scale;

        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(force, ForceMode2D.Impulse);
        rigid.AddTorque(isRight ? -1.0f : 1.0f);

        if (player != null)
        {
            player.SetPrepareAction(GetSkillDamageValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rigid != null && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Attack"))
        {
            rigid.gravityScale = 1.0f;
            rigid.velocity = Vector2.zero;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy Sc = collision.gameObject.GetComponent<Enemy>();
                Sc.Hit(skillDamage);
            }
        }

        if (rigid != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
            SkillManager.Instance.ResetCoolTime(SkillManager.SkillType.SkillA);
        }

    }

    private void SetTimer()
    {
        cooldownTime -= Time.deltaTime;

        if (cooldownTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SkillSetting(Vector2 _force, bool _isRight, float _cooldownTime)
    {
        force = _force;
        isRight = _isRight;
        cooldownTime = _cooldownTime;
    }
}
