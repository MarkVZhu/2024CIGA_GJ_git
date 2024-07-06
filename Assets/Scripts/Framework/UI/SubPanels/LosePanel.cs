using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class LosePanel : BasePanel
{
	//�ؿ������������panel
	protected override void Awake()
	{
		//һ�������� ��Ϊ��Ҫִ�и����awake����ʼ��һЩ��Ϣ �����ҿؼ� ���¼�����
		base.Awake();
		//�����洦���Լ���һЩ��ʼ���߼�
	}

	// Use this for initialization
	void Start()
	{
		BtnInit("btnResume");
		BtnInit("btnMain");
		// UIManager.AddCustomEventListener(GetControl<Button>("btnStart"), EventTriggerType.PointerEnter, (data)=>{
		//     Debug.Log("����");
		// });
		// UIManager.AddCustomEventListener(GetControl<Button>("btnStart"), EventTriggerType.PointerExit, (data) => {
		//     Debug.Log("�뿪");
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
		//��ק�߼�
	}

	private void PointerDown(BaseEventData data)
	{
		//PointerDown�߼�
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void ShowMe()
	{
		base.ShowMe();
		//��ʾ���ʱ ��Ҫִ�е��߼� ������� ��UI�������� ���Զ������ǵ���
		//ֻҪ��д����  �ͻ�ִ��������߼�
	}
	void fun() { }
	protected override void OnClick(string btnName)
	{
		int id = ScenesMgr.Instance.GetSceneInd();
		Debug.Log(id);
		switch (btnName)
		{
			case "btnResume":
				Debug.Log("btnResume�����");
				ScenesMgr.Instance.LoadScene(id, fun);
				UIManager.Instance.HidePanel("LosePanel");
				break;
			case "btnMain":
				Debug.Log("btnMain�����");
				UIManager.Instance.HidePanel("LosePanel");
				UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
				break;
		}
	}

	protected override void OnValueChanged(string toggleName, bool value)
	{
		//���������������ж� ��������һ����ѡ����߶�ѡ��״̬�仯�� ��ǰ״̬���Ǵ����value
	}


	public void InitInfo()
	{
		Debug.Log("��ʼ������");
	}

	//�����ʼ��ť�Ĵ������Էŵ�switch�
	public void ClickStart()
	{
	}

	//�����ʼ��ť�Ĵ���
	public void ClickQuit()
	{
		Debug.Log("Quit Game");
	}
}
