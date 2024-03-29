using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject followObject;

    public static FollowCamera Instance;

    private void Start()
    {
        Instance = this;

        GameManager manager = GameManager.Instance;
        followObject = manager.GetPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        if(followObject == null)
        {            
            return;
        }

        Vector3 pos = followObject.transform.position;
        pos.z -= 2.0f;
        transform.position = pos;
    }

    public void SetPlayer(GameObject _target)
    {
        followObject = _target;
    }

}
