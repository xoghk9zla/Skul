using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    [SerializeField] Image imgFrontHp;
    [SerializeField] Image imgMiddleHp;
    [SerializeField] Text textHp;

    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameManager.Instance;
        GameObject obj = manager.GetPlayerObject();
        Player objSc = obj.GetComponent<Player>();
        objSc.SetPlayerHp(this);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHp();
    }

    private void CheckPlayerHp()
    {
        float amountFront = imgFrontHp.fillAmount;
        float amountMiddle = imgMiddleHp.fillAmount;

        if(amountFront < amountMiddle) 
        {
            imgMiddleHp.fillAmount -= Time.deltaTime * 0.05f; 

            if(imgMiddleHp.fillAmount <= imgFrontHp.fillAmount)
            {
                imgMiddleHp.fillAmount = imgFrontHp.fillAmount;
            }
        }
        else if(amountFront > amountMiddle)
        {
             imgMiddleHp.fillAmount = imgFrontHp.fillAmount;
        }
    }

    public void SetPlayerHp(float _curHp, float _maxHp)
    {
        textHp.text = $"{_curHp} / {_maxHp}";
        imgFrontHp.fillAmount = _curHp / _maxHp;
    }
}
