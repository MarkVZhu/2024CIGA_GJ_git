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
		EventCenter.Instance.AddEventListener<E_BlockNum>(E_EventType.E_Change_Block_For_Building, ChangeBlock);
	}

	void BuildBlock(Vector3 cellPosition)
	{
		BlockControl bc = customizedBlockPref.GetComponent<BlockControl>();
		//if(!bc.IsBlockValid(e_BlockNum)) Debug.Log("Invalid block");
		if(bc.IsBlockValid(e_BlockNum) && limitNum > 0)
		{
			bc.blockNum = e_BlockNum;
			Instantiate(customizedBlockPref, cellPosition, Quaternion.identity);
			limitNum--;
			EventCenter.Instance.EventTrigger<int>(E_EventType.E_LimitNum_Change, limitNum);
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
		EventCenter.Instance.EventTrigger<int>(E_EventType.E_LimitNum_Change, limitNum);
	}
	
	void ChangeBlock(E_BlockNum e)
	{
		e_BlockNum = e;
	}
	
	void OnDestroy() 
	{
		EventCenter.Instance.RemoveEventListener<Vector3>(E_EventType.E_Build_Block, BuildBlock);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Delete_Block, LimitNumRestore);
		EventCenter.Instance.RemoveEventListener<E_BlockNum>(E_EventType.E_Change_Block_For_Building, ChangeBlock);
	}
}
