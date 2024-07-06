using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class HardnessEffector : MonoBehaviour
{
	public int hardness;
	float hardnessFloat;
	bool canCountDown; //TODO:更新canCountDown
	
	// Start is called before the first frame update
	void Start()
	{
		EventCenter.Instance.AddEventListener(E_EventType.E_Start_Level,EnableCountDown);
		//EventCenter.Instance.EventTrigger(E_EventType.E_Start_Level);//TODO:别的地方触发
		hardnessFloat = hardness;
		canCountDown = false;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			EventCenter.Instance.EventTrigger(E_EventType.E_Start_Level);
		}
		
		if(canCountDown)
		{
			if(hardnessFloat > 0)
			{
				hardnessFloat -= Time.deltaTime;
			}
			else
			{
				Destroy(transform.parent.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void EnableCountDown()
	{
		if (hardness < 8)
		{
			canCountDown = true;
		}
	}
}
