using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InGameManager;

public class HUDPanel : BasePanel
{
    private GameModel model;

    public Image[] StarImages;
    public Sprite StarActivated;
    public Sprite StarUnactivated;

    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextTime;
    float curTime = 0;
    // Start is called before the first frame update

    void Start()
    {
        transform.parent.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        transform.parent.GetComponent<RectTransform>().anchorMax = Vector2.one;
        transform.parent.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        transform.parent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        model = GameModel.Instance;
        model.PropertyValueChanged += HandleModel;

    }
    private void HandleModel(object sender, Mine.PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "StarCount":
                for (int i = 0; i < StarImages.Length; i++)
                {
                    StarImages[i].sprite = StarUnactivated;
                }
                for (int j = 0; j < (int)e.Value; j++)
                {
                    StarImages[j].sprite = StarActivated;
                }
                break;
            case "Score":
                TextScore.text = $"Score: {(int)e.Value}";
                break;
            case "CurGameState":

                if((GameState)e.Value == GameState.Research)
                {
                    curTime = 0;
                    CalculateTime(curTime);
                }
                if((GameState)e.Value == GameState.Success)
                {
                    CalculateTimeScore();
                }
                break;
        }
    }
    
    void RecordValue()
    {
        if(GameModel.Instance.CurGameState == GameState.Test)
        {
            curTime += Time.deltaTime;
            CalculateTime(curTime);
        }
    }
    void CalculateTime(float time)
    {
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int miliseconds =(int) ((time - Mathf.Floor(time))*100);
        TimeSpan timeSpan = new TimeSpan(1,1, minutes, seconds, miliseconds);
        string formattedTimeSpan = string.Format("Time: {0:D2}:{1:D2}:{2:D2}",
        timeSpan.Minutes,
        timeSpan.Seconds,
        timeSpan.Milliseconds);
        TextTime.text = formattedTimeSpan;

    }
    void CalculateTimeScore()
    {
        GameModel.Instance.Score += (int)(1f / curTime * 1000f);
    }
    // Update is called once per frame
    void Update()
    {
        RecordValue();
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameModel.Instance.Score++;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameModel.Instance.CurGameState = InGameManager.GameState.Research;
        }
    }
}
