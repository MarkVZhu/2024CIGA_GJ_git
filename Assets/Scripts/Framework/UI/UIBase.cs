using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class UIBase : BasePanel
{
    //�������������¼��߼�
    void Start()
    {
        //�������˵�
        UIManager.Instance.ShowPanel<MapPanel>("MapPanel");
        //TODO����������
        Time.timeScale = (1);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                UIManager.Instance.ShowPanel<PausePanel>("PausePanel");
                Time.timeScale = (0);
        }
    }
}
