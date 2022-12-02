using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay = 3;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyEffect",delay);
    }

    void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
