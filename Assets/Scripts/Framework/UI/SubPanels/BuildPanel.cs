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
		GameObject bkc = GameObject.FindGameObjectWithTag("BlockController");
		GetControl<TextMeshProUGUI>("BlockLeftText").text = bkc.GetComponent<CreateBlock>().limitNum.ToString();
	}
	private void BtnInit(string btnName)
	{
		Image btbg = GetControl<Image>(btnName);
		string loadBlock = btnName.Remove(0,3);
		Debug.Log("loadBlock name: " + loadBlock);
		Block bk = ResMgr.Instance.Load<Block>("SO/" + loadBlock);
		btbg.color = GetColor(bk.hardness, bk.bounce, bk.smooth);
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
	
	public Color GetColor(int hardness, float bounce, float smooth)
	{
		// Clamp the input values to their respective ranges
		hardness = Mathf.Clamp(hardness, 0, 10);
		bounce = Mathf.Clamp01(bounce);
		smooth = Mathf.Clamp01(smooth);

		// Map the values to 0~255 range
		int r = Mathf.RoundToInt(Map(hardness, 0, 10, 50, 150));
		int g = Mathf.RoundToInt(Map(bounce, 0, 1, 0, 255));
		int b = Mathf.RoundToInt(Map(smooth, 0, 1, 0, 255));

		// Create the color
		return new Color32((byte)r, (byte)g, (byte)b, 255);
	}
	
	private float Map(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
