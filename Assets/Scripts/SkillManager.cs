using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] Image imgSkul;
    [SerializeField] Image imgSkillA;
    [SerializeField] Image imgSkillS;

    private float cooldownTimeA;
    private float cooldownTimeS;

    [SerializeField] private GameObject objPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Player objSc = objPlayer.GetComponent<Player>();
        objSc.SetSkill(this);
    }

    // Update is called once per frame
    void Update()
    {
        Color alpha =  new Color(1.0f, 1.0f, 1.0f, 0.3f);
        imgSkillA.color = alpha;
    }

    public void SetSkill(float _cooldownTimeA, float _cooldownTimeS)
    {
        cooldownTimeA = _cooldownTimeA;
        cooldownTimeS = _cooldownTimeS;
    }
}
