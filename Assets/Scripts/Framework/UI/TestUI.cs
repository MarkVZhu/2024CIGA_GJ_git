using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestUI : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		//UIManager.Instance.ShowPanel<MapPanel>("MapPanel");
		//UIManager.Instance.ShowPanel<PausePanel>("PausePanel");
		//Invoke("HidePanel",1f);
		//UIManager.Instance.ShowPanel<ResultPanel>("ResultPanel");
		//UIManager.Instance.ShowPanel<LosePanel>("LosePanel");
		UIManager.Instance.ShowPanel<testPanel>("testPanel");
	}

}
