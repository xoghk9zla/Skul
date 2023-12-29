using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    private void EndEffect()
    {
        Destroy(gameObject);
    }
}
