using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class MapPanel : BasePanel {
	int btnnum = 3;//按钮数量
	int starcnt = 0;
	public LevelData curld;//当前玩家数据
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start () {
		//开始逻辑
		//Button bt = GetControl<Button>("btnLevel2");
		//bt.interactable = false;

		//Image im = GetControl<Image>("btnLevel2");
		//im.sprite = ResMgr.Instance.Load<Sprite>("Image/testLevel");
		Image btbg = GetControl<Image>("btnMain");
		btbg.sprite = ResMgr.Instance.Load<Sprite>("Image/btn");
		for (int i = 1; i <= btnnum; i++)
            {
				string btnName = "btnLevel"+i;
				Debug.Log(btnName);
				BtnInit(btnName,i);
            }
		curld.Levels[0].isUnlocked = true;
		//}
	}
	private void BtnInit(string btnName,int i)
    {
		i--;//关卡从第一关开始但是序列化以0为初始编号
        Debug.Log(curld.Levels[0].LevelName);
		Button bt = GetControl<Button>(btnName);
		bt.interactable = true;
		Image btbg = GetControl<Image>(btnName);
		btbg.sprite = ResMgr.Instance.Load<Sprite>("Image/btn");
		if (!curld.Levels[i].isUnlocked)//判断是否上锁，初始是上锁状态
		{
			//上锁状态
			bt.interactable = false;
			Image img = GetControl<Image>(btnName);
			img.sprite=ResMgr.Instance.LoadSpriteFromSheet("Image/GUI", "GUI_0");
		}
		else 
		{
			//判断获得了几颗星星
			starcnt = curld.Levels[i].StarCount;
			for(int j = 1; j <= starcnt; j++)
            {
				string starname = "star" + j ;
				Image im = GetControl<Image>(starname);
				Debug.Log(starname);
				im.sprite = ResMgr.Instance.Load<Sprite>("Image/star");
			}

		}

		
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
	void Update () {
		
	}

	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}

	protected override void OnClick(string btnName)
	{
		switch(btnName)
		{
			case "btnLevel1":
				UIManager.Instance.HidePanel("MapPanel");
				ScenesMgr.Instance.LoadScene("LevelSample_1", fun); //加载关卡
				UIManager.Instance.HidePanel("MapPanel");
				UIManager.Instance.ShowPanel<preResearchPanel>("preResearchPanel");
				break;
			case "btnLevel2":
				UIManager.Instance.HidePanel("MapPanel");
				ScenesMgr.Instance.LoadScene("Lev2", fun); //加载关卡
				UIManager.Instance.HidePanel("MapPanel");
				UIManager.Instance.ShowPanel<preResearchPanel>("preResearchPanel");
				break;
			case "btnLevel3":
				UIManager.Instance.HidePanel("MapPanel");
				ScenesMgr.Instance.LoadScene("Lev3", fun); //加载关卡
				UIManager.Instance.HidePanel("MapPanel");
				UIManager.Instance.ShowPanel<preResearchPanel>("preResearchPanel");
				break;
			case "btnMain":
				UIManager.Instance.HidePanel("MapPanel");
				UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
				break;
		}
	}
	
	void fun()
	{
		Debug.Log("加载完成");
	}

	protected override void OnValueChanged(string toggleName, bool value)
	{
		//在这来根据名字判断 到底是那一个单选框或者多选框状态变化了 当前状态就是传入的value
	}


	public void InitInfo()
	{
		Debug.Log("初始化数据");
	}

	//点击开始按钮的处理(可以放到switch里)
	public void ClickStart()
	{
	}

	//点击开始按钮的处理
	public void ClickQuit()
	{
		Debug.Log("Quit Game");
	}
}
