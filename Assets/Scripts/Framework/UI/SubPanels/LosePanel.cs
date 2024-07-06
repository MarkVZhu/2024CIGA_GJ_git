using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class LosePanel : BasePanel
{
	//关卡结束后调出该panel
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start()
	{
		BtnInit("btnResume");
		BtnInit("btnMain");
		// UIManager.AddCustomEventListener(GetControl<Button>("btnStart"), EventTriggerType.PointerEnter, (data)=>{
		//     Debug.Log("进入");
		// });
		// UIManager.AddCustomEventListener(GetControl<Button>("btnStart"), EventTriggerType.PointerExit, (data) => {
		//     Debug.Log("离开");
		// });
	}
	private void BtnInit(string btnName)
	{
		//Button bt = GetControl<Button>(btnName);
		//bt.interactable = true;
		Image btbg = GetControl<Image>(btnName);
		btbg.sprite = ResMgr.Instance.LoadSpriteFromSheet("Image/GUI", "GUI_7");

	}
	private void Drag(BaseEventData data)
	{
		//拖拽逻辑
	}

	private void PointerDown(BaseEventData data)
	{
		//PointerDown逻辑
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}
	void fun() { }
	protected override void OnClick(string btnName)
	{
		int id = ScenesMgr.Instance.GetSceneInd();
		Debug.Log(id);
		switch (btnName)
		{
			case "btnResume":
				Debug.Log("btnResume被点击");
				ScenesMgr.Instance.LoadScene(id, fun);
				UIManager.Instance.HidePanel("LosePanel");
				break;
			case "btnMain":
				Debug.Log("btnMain被点击");
				UIManager.Instance.HidePanel("LosePanel");
				UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
				break;
		}
	}

	protected override void OnValueChanged(string toggleName, bool value)
	{
		//在这来根据名字判断 到底是那一个单选框或者多选框状态变化了 当前状态就是传入的value
	}


	public void InitInfo()
	{
		Debug.Log("初始化数据");
	}

	//点击开始按钮的处理（可以放到switch里）
	public void ClickStart()
	{
	}

	//点击开始按钮的处理
	public void ClickQuit()
	{
		Debug.Log("Quit Game");
	}
}
