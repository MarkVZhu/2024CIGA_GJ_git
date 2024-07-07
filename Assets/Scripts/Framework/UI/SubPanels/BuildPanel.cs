using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
using TMPro;
public class BuildPanel : BasePanel
{
	protected override void Awake()
	{
		base.Awake();
		EventCenter.Instance.AddEventListener<int>(E_EventType.E_LimitNum_Change, LimitNumChange);
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
	
	void LimitNumChange(int v)
	{
		TextMeshProUGUI textMeshPro = GetControl<TextMeshProUGUI>("BlockLeftText");
		textMeshPro.text = v.ToString();
	}

	// Update is called once per frame
	void Update()
	{

	}
	
	void fun()
	{
		Debug.Log("�������");
	}
	public override void ShowMe()
	{
		base.ShowMe();
		//��ʾ���ʱ ��Ҫִ�е��߼� ������� ��UI�������� ���Զ������ǵ���
		//ֻҪ��д����  �ͻ�ִ��������߼�
	}

	protected override void OnClick(string btnName)
	{
		// TODO: �л�ש��

		switch (btnName)
		{
			case "btnBlock1":
				EventCenter.Instance.EventTrigger<E_BlockNum>(E_EventType.E_Change_Block_For_Building, E_BlockNum.E_Block_1);
				break;
			case "btnBlock2":
				EventCenter.Instance.EventTrigger<E_BlockNum>(E_EventType.E_Change_Block_For_Building, E_BlockNum.E_Block_2);
				break;
			case "btnBlock3":
				EventCenter.Instance.EventTrigger<E_BlockNum>(E_EventType.E_Change_Block_For_Building, E_BlockNum.E_Block_3);
				break;
			case "btnBlock4":
				EventCenter.Instance.EventTrigger<E_BlockNum>(E_EventType.E_Change_Block_For_Building, E_BlockNum.E_Block_4);
				break;
			//FIXME:buildPanel UI bug
			case "btnConfirm":
				EventCenter.Instance.EventTrigger(E_EventType.E_Start_Level);
				EventCenter.Instance.EventTrigger(E_EventType.E_Enter_Next_State);
				UIManager.Instance.HidePanel("BuildPanel");
				UIManager.Instance.ShowPanel<testPanel>("testPanel");
			break;
		}
	}
	public void InitInfo()
	{
		Debug.Log("��ʼ������");
	}
}
