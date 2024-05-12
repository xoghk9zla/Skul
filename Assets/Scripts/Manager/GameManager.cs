using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameoverUI;

    [SerializeField] private GameObject objPlayer;
    [SerializeField] private GameObject[] skulprefab;

    [SerializeField] private GameObject objEnemy;

    [SerializeField] private GameObject[] listSkul = new GameObject[2];
    private Player player;

    private bool isSwitching;

    [SerializeField] GameObject objHead;
    [SerializeField] Transform trsEffect;

    [SerializeField] private GameObject objEndingText;

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

    private void Awake()
    {
        Instance = this;
        player = objPlayer.GetComponent<Player>();
    }

    private void Start()
    {
        listSkul[0] = objPlayer;
    }

    public void ChangeSkul(SkulHead.SkulType _type, Transform _transform)
    {
        GameObject beforeSkul = objPlayer;
        if (_type == SkulHead.SkulType.LittleBone)
        {
            objPlayer.SetActive(false); // Destory는 이 함수가 전부 실행 되고 난 뒤 작동
            objPlayer = Instantiate(skulprefab[0], _transform.position, Quaternion.identity);

            if (CheckEmptySkulList() != -1)
            {
                listSkul[1] = beforeSkul;
                listSkul[0] = objPlayer;
            }
        }
        else if (_type == SkulHead.SkulType.GrimReaper)
        {
            objPlayer.SetActive(false);
            objPlayer = Instantiate(skulprefab[1], _transform.position, Quaternion.identity);

            if (CheckEmptySkulList() != -1)
            {
                listSkul[1] = beforeSkul;
                listSkul[0] = objPlayer;
            }
        }
        FollowCamera.Instance.SetPlayer(objPlayer);
        SkillManager.Instance.SetImage(objPlayer);
        SkillManager.Instance.SetSkill(objPlayer);
    }

    public void SwitchSkul()
    {
        listSkul[1].transform.position = objPlayer.transform.position;
        listSkul[1].transform.localScale = objPlayer.transform.localScale;

        GameObject temp = listSkul[0];
        listSkul[0] = listSkul[1];
        listSkul[1] = temp;

        objPlayer.SetActive(false);

        objPlayer = listSkul[0];
        objPlayer.SetActive(true);

        IsSwitching = true;

        FollowCamera.Instance.SetPlayer(objPlayer);
        SkillManager.Instance.SetImage(objPlayer);
        SkillManager.Instance.SetSkill(objPlayer);
    }

    public GameObject GetPlayerObject()
    {
        return objPlayer;
    }

    public int CheckEmptySkulList()
    {
        for (int i = 0; i < listSkul.Length; i++)
        {
            if (listSkul[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public bool CheckEmptyEnemyList()
    {
        if (objEnemy.transform.childCount != 0)
        {
            return false;
        }
        return true;
    }

    public void GameOver()
    {
        StartCoroutine(SetTimeScale());
    }

    IEnumerator SetTimeScale()
    {
        Time.timeScale = 0.15f;

        GameObject deathPosition = new GameObject();
        deathPosition.transform.position = objPlayer.transform.position - new Vector3(1.1f, 0.0f, 0.0f);

        Destroy(objPlayer);
        GameObject head = Instantiate(objHead, objPlayer.transform.position + new Vector3(0.0f, 0.3f), Quaternion.identity);      
        Rigidbody2D rigid = head.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 2.0f));
        float rot = Random.Range(-3.0f, 3.0f);

        rigid.AddForce(dir, ForceMode2D.Impulse);
        rigid.AddTorque(rot);

        yield return new WaitForSeconds(0.15f);

        Time.timeScale = 0.0f;

        GameObject objEffect = GameObject.Find("ObjectEffect");
        trsEffect = objEffect.transform;

        DamageFont[] fonts = trsEffect.GetComponentsInChildren<DamageFont>();

        for(int i = 0; i < fonts.Length; i++)
        {
            fonts[i].gameObject.SetActive(false);
        }        
        
        gameoverUI.SetActive(true);

        FollowCamera.Instance.SetPlayer(deathPosition);        
    }

    public void GameEnding()
    {
        objEndingText.SetActive(true);

        StartCoroutine(EndingProduction());
    }

    IEnumerator EndingProduction()
    {
        while (objEndingText.transform.position.y > 0.0f)
        {
            objEndingText.transform.position -= new Vector3(0.0f, 2.0f, 0.0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}