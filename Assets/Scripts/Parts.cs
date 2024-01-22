using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    float lifeTime = 3.0f;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.time;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
