using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class TestAddImage : MonoBehaviour
{
	public SpriteRenderer spriteRender;
	// Start is called before the first frame update
	void Start()
	{
		spriteRender.sprite = ResMgr.Instance.Load<Sprite>("Image/GUI_2");
	}
}
