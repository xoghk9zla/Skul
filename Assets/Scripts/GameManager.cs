using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject objPlayer;
    [SerializeField] private GameObject[] skulprefab;

    [SerializeField] private GameObject[] listSkul = new GameObject[2];

    private void Awake()
    {
        Instance = this;        
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
    }

    public GameObject GetPlayerObject()
    {
        return objPlayer;
    }

    private int CheckEmptySkulList()
    {
        for(int i = 0; i < listSkul.Length; i++)
        {
            if (listSkul[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}
