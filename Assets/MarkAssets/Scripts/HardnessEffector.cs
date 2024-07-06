using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class HardnessEffector : MonoBehaviour
{
	public int hardness;
	// Start is called before the first frame update
	void Start()
	{
		EventCenter.Instance.AddEventListener(E_EventType.E_Start_Level,DestroyCountDown);
		EventCenter.Instance.EventTrigger(E_EventType.E_Start_Level);//TODO:别的地方触发
	}

	// Update is called once per frame
	void DestroyCountDown()
	{
		if (hardness < 8)
		{
			Invoke("DestroyCube", hardness);
		}
	}
	
	void DestroyCube()
	{
		Destroy(gameObject.transform.parent.gameObject);
	}
}
