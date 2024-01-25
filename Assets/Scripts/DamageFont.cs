using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageFont : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
    private float lifeTime = 1.0f;

    public enum damageType
    {
        player, enemy
    }

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        transform.position += Vector3.up *Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(float _damage, damageType _type)
    {
        damageText.text = _damage.ToString();

        if (_type == damageType.player)
        {
            damageText.color = Color.white;
        }
        else if (_type == damageType.enemy)
        {
            damageText.color = Color.red;
        }
    }
}
