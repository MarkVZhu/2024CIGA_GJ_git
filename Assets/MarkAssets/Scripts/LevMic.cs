using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class LevMic : BasePanel
{
	void Awake()
	{
		//DontDestroyOnLoad(this); //If do not need, delete this line
	}

	// Update is called once per frame
	
	private void Start()
	{
		string name = ScenesMgr.Instance.GetSceneName();
		BGMControl.Pause();
		SoundMgr.Instance.PlayBKMusic(name);
	}
}
