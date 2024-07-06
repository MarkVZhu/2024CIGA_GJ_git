using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
public class BuildPanel : BasePanel
{
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start()
	{
		for (int i = 1; i <= 4; i++)
		{
			BtnInit("btnBlock" + i);
		}
	}
	private void BtnInit(string btnName)
	{
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
	void fun()
	{
		Debug.Log("加载完成");
	}
	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}

	protected override void OnClick(string btnName)
	{
        // TODO: 切换砖块

        switch (btnName)
        {
            case "btnBlock1":
            case "btnBlock2":
            case "btnBlock3":
            case "btnBlock4":
            //TODO:将场景切换到test状态
            case "btnConfirm":
                UIManager.Instance.HidePanel("BuildPanel");
				UIManager.Instance.ShowPanel<testPanel>("testPanel");
            break;
        }
    }
	public void InitInfo()
	{
		Debug.Log("初始化数据");
	}
}
