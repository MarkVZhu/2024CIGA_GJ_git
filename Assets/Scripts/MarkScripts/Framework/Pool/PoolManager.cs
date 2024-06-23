using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Pool container
/// </summary>
public class PoolData
{
	public GameObject fatherObj;
	public List<GameObject> poolList;
	
	public PoolData(GameObject obj, GameObject poolObj)
	{
		fatherObj = new GameObject(obj.name);
		fatherObj.transform.parent = poolObj.transform;
		
		poolList = new List<GameObject>(){};
		PushObj(obj);
	}
	
	public void PushObj(GameObject obj)
	{
		obj.SetActive(false);
		poolList.Add(obj);
		obj.transform.parent = fatherObj.transform;
	}
	
	public GameObject GetObj()
	{
		GameObject obj = null;
		obj = poolList[0];
		poolList.RemoveAt(0);
		obj.SetActive(true);
		obj.transform.parent = null;
		
		return obj;
	}
}

/// <summary>
/// buffer pool module
/// 1. Dictionary List
/// 2. GameObject and Resources' API
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
	public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
	private GameObject poolObj;
	
	//Get a obj from the pool 
	public void GetObj(string name, UnityAction<GameObject> callBack)
	{
		if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
		{
			//obj = poolDic[name].GetObj();
			callBack(poolDic[name].GetObj());
		}
		else
		{
			ResMgr.Instance.LoadAsync<GameObject>(name, (o) =>
			{
				o.name = name;
				callBack(o);
			});
			
			// obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
			// obj.name = name;
		}
	}
	
	//Push obj into the pool
	public void PushObject(string name, GameObject obj)
	{
		if(poolObj == null)
		{
			poolObj = new GameObject("Pool");
		}
		
		obj.SetActive(false);
		if(poolDic.ContainsKey(name))
		{
			poolDic[name].PushObj(obj);
		}
		else
		{
			poolDic.Add(name, new PoolData(obj, poolObj));
		}	
	}
	
	public void Clear()
	{
		poolDic.Clear();
		poolObj = null;
	}
}
