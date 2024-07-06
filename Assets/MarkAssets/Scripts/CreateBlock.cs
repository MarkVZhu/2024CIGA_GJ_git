using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
	public GameObject customizedBlockPref;
	public E_BlockNum e_BlockNum;
	public int limitNum = 10;
	
	// Start is called before the first frame update
	void Start()
	{
		EventCenter.Instance.AddEventListener<Vector3>(E_EventType.E_Build_Block, BuildBlock);
		EventCenter.Instance.AddEventListener(E_EventType.E_Delete_Block, LimitNumRestore);
	}

	void BuildBlock(Vector3 cellPosition)
	{
		if(limitNum > 0)
		{
			customizedBlockPref.GetComponent<BlockControl>().blockNum = e_BlockNum;
			Instantiate(customizedBlockPref, cellPosition, Quaternion.identity);
			limitNum--;
			Debug.Log("limitNum " + limitNum);
		}
		else
		{
			Debug.Log("no limitNum remain: " + limitNum);
		}		
		
	}
	
	void LimitNumRestore()
	{
		limitNum++;
	}
}
