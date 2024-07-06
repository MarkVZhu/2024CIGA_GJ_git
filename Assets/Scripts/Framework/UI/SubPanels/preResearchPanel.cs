using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class preResearchPanel : BasePanel {
	public GameObject ParticlePanel;
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start () {
		
		BtnInit("btnConfirm");
	}
	private void BtnInit(string btnName)
	{
		Image btbg = GetControl<Image>(btnName);
		btbg.sprite = ResMgr.Instance.LoadSpriteFromSheet("Image/GUI", "GUI_7");

	}
	
	void fun()
	{
	}
	
	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}

	protected override void OnClick(string btnName)
	{
		switch (btnName)
		{
			case "btnConfirm":
				//进入研究panel
				UIManager.Instance.HidePanel("preResearchPanel");
				//TODO:此处填入研究panel
				EventCenter.Instance.EventTrigger(E_EventType.E_Call_ParticlePanel);
				//UIManager.Instance.ShowPanel<ParticlePanel>("ParticlePanel");
				break;
		}
	}
}
