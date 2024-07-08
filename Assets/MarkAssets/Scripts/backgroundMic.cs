using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class backgroundMic : MonoBehaviour
{	void Awake() 
	{
		//DontDestroyOnLoad(this); //If do not need, delete this line
	}

    // Update is called once per frame
    
	private void Start() 
	{
		SoundMgr.Instance.PlayBKMusic("bgm1");	
	}
}
