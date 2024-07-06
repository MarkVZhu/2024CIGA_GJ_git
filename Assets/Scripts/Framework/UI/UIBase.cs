using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;

public class UIBase : BasePanel
{
    //用来管理所有事件逻辑
    void Start()
    {
        //进入主菜单
        UIManager.Instance.ShowPanel<MapPanel>("MapPanel");
        //TODO：播放音乐

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
