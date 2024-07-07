using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static InGameManager;

public class GameModel : SingletonMono<GameModel>
{
    #region Event
    protected bool ChangePropertyAndNotify<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = null)
    {
        if (newValue == null && currentValue == null)
        {
            return false;
        }

        if (newValue != null && newValue.Equals(currentValue))
        {
            return false;
        }

        currentValue = newValue;

        RaisePropertyChanged(propertyName, newValue);

        return true;
    }

    protected virtual void RaisePropertyChanged(string propertyName, object value = null)
    {

        PropertyValueChanged?.Invoke(this, new Mine.PropertyValueChangedEventArgs(propertyName, value));
    }

    public event Mine.PropertyValueChangedEventHandler PropertyValueChanged;
    #endregion

    private int starCount;
    public int StarCount
    {
        get => starCount;
        set
        {
            ChangePropertyAndNotify<int>(ref starCount, value);
        }
    }
    private int score;
    public int Score
    {
        get => score;
        set
        {
            ChangePropertyAndNotify<int>(ref score, value);
        }
    }
    private float totalTime;
    public float TotalTime
    {
        get => totalTime;
        set
        {
            ChangePropertyAndNotify<float>(ref totalTime, value);
        }
    }
    private GameState gameState;
    public GameState CurGameState
    {
        get => gameState;
        set
        {
            if(value == GameState.Research)
            {
                Score = 0;
                StarCount = 0;
                TotalTime = 0;
            }
            ChangePropertyAndNotify<GameState>(ref gameState, value);
        }
    }

    private int curLevel;
    public int CurLevel
    {
        get => curLevel;
        set 
        {
            ChangePropertyAndNotify<int>(ref curLevel, value);
        }
    }

    public void ResetModel()
    {

    }
}
namespace Mine
{
    public class PropertyValueChangedEventArgs : EventArgs
    {
        public string PropertyName;
        public object Value;

        public PropertyValueChangedEventArgs(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }

    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
}

