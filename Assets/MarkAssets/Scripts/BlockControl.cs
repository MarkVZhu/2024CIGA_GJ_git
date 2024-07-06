using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
	public E_BlockNum blockNum;
	SpriteRenderer sr;
	int hardness;
	float bounce;
	float smooth;
	
	public HardnessEffector hardnessEffector;
	public AreaEffector2D bounceEffector;
	public AreaEffector2D smoothEffector;
	
	public int bounceMultiplier = 1500;
	public int smoothMultiplier = 1000;
	
	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr != null) SetBlockColor();
	}
	
	void OnEnable() 
	{
		SetProperty();
		hardnessEffector.hardness = hardness;
		bounceEffector.forceMagnitude = bounce * bounceMultiplier;
		smoothEffector.forceMagnitude = smooth * smoothMultiplier;
	}
	
	void SetProperty()
	{
		switch (blockNum)
		{
			case E_BlockNum.E_Block_1:
				Block b1 = ResMgr.Instance.Load<Block>("SO/Block1");
				SetPropertyValue(b1);
			 	break;
			case E_BlockNum.E_Block_2:
				Block b2 = ResMgr.Instance.Load<Block>("SO/Block2");
				SetPropertyValue(b2);
			 	break;
			case E_BlockNum.E_Block_3:
				Block b3 = ResMgr.Instance.Load<Block>("SO/Block3");
				SetPropertyValue(b3);
			 	break;
			case E_BlockNum.E_Block_4:
				Block b4 = ResMgr.Instance.Load<Block>("SO/Block4");
				SetPropertyValue(b4);
			 	break;
		}
	}
	
	void SetPropertyValue(Block block)
	{
		hardness = block.hardness;
		bounce = block.bounce;
		smooth = block.smooth;
	}
	
	void SetBlockColor()
	{
		sr.color = GetColor(hardness, bounce, smooth);
	}
	
	public Color GetColor(int hardness, float bounce, float smooth)
	{
		// Clamp the input values to their respective ranges
		hardness = Mathf.Clamp(hardness, 0, 10);
		bounce = Mathf.Clamp01(bounce);
		smooth = Mathf.Clamp01(smooth);

		// Map the values to 0~255 range
		int r = Mathf.RoundToInt(Map(hardness, 0, 10, 100, 255));
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
