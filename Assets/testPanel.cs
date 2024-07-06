using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
public class testPanel : BasePanel
{
	// Start is called before the first frame update
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start()
	{
		BtnInit("btnReset");
		BtnInit("btnPause");
	}
	private void BtnInit(string btnName)
	{
		Image btbg = GetControl<Image>(btnName);
		btbg.sprite = ResMgr.Instance.LoadSpriteFromSheet("Image/GUI", "GUI_7");

	}
	void fun() { }
	protected override void OnClick(string btnName)
	{
		int id = ScenesMgr.Instance.GetSceneInd();
		Debug.Log(id);
		switch (btnName)
		{
			case "btnPause":
				UIManager.Instance.HidePanel("testPanel");
				UIManager.Instance.ShowPanel<PausePanel>("PausePanel");
				break;
			case "btnReset":
				ScenesMgr.Instance.LoadScene(id, fun);
				UIManager.Instance.HidePanel("PausePanel");
				break;
		}
	}
}
