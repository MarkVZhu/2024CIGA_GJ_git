using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{

	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			PoolManager.Instance.GetObj("TestPool/Cube", (o) =>
			{
				Debug.Log("Test Pool: " + o.name);
			});
		}
		if(Input.GetMouseButtonDown(1))
		{
			PoolManager.Instance.GetObj("TestPool/Sphere", (o) =>
			{
				Debug.Log("Test Pool: " + o.name);
			});
		}
	}
}
