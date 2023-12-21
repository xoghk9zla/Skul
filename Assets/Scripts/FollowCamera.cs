using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject objPlayer;

    // Update is called once per frame
    void Update()
    {
        if(objPlayer == null)
        {
            return;
        }

        Vector3 pos = objPlayer.transform.position;
        pos.z -= 2.0f;
        transform.position = pos;
    }
}
