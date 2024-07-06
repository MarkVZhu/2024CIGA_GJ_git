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
		//һ�������� ��Ϊ��Ҫִ�и����awake����ʼ��һЩ��Ϣ �����ҿؼ� ���¼�����
		base.Awake();
		//�����洦���Լ���һЩ��ʼ���߼�
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
				UIManager.Instance.HidePanel("testPanel");
				break;
		}
	}
}
