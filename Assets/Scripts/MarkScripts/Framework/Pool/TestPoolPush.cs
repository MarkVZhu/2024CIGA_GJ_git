using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoolPush : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Push",1);
    }

    void Push()
    {
    	PoolManager.Instance.PushObject(name, gameObject);
    }
}
