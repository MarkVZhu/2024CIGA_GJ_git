using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class BGMControl : MonoBehaviour
{
	public static void Pause()
	{
		SoundMgr.Instance.PauseBKMusic();
	}
	public static void MapMusic()
	{
		SoundMgr.Instance.PlayBKMusic("bgm 2");
	}
}
