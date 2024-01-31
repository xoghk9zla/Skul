using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public static EnemyHP Instance;

    private Transform trsEnemy;

    [SerializeField] Image imgFrontHp;
    [SerializeField] Image imgMiddleHp;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Enemy Sc = GetComponentInParent<Enemy>();
        trsEnemy = Sc.transform;       
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyPos();
        CheckEnemyHp();
    }

    private void CheckEnemyPos()
    {
        transform.position = trsEnemy.position - new Vector3(0.0f, 0.05f, 0.0f);

        Vector3 hpScale = transform.localScale;

        if(trsEnemy.localScale.x < 0)
        {
            hpScale.x = -Mathf.Abs(hpScale.x);
        }
        else
        {
            hpScale.x = Mathf.Abs(hpScale.x);
        }
        
        transform.localScale = hpScale;
    }

    private void CheckEnemyHp()
    {
        float amountFront = imgFrontHp.fillAmount;
        float amountMiddle = imgMiddleHp.fillAmount;

        if (amountFront < amountMiddle)
        {
            imgMiddleHp.fillAmount -= Time.deltaTime * 0.2f;

            if (imgMiddleHp.fillAmount <= imgFrontHp.fillAmount)
            {
                imgMiddleHp.fillAmount = imgFrontHp.fillAmount;
            }
        }
        else if (amountFront > amountMiddle)
        {
            imgMiddleHp.fillAmount = imgFrontHp.fillAmount;
        }
    }

    public void SetPlayerHp(float _curHp, float _maxHp)
    {        
        imgFrontHp.fillAmount = _curHp / _maxHp;
    }
}
