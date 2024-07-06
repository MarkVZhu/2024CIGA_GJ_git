using MarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


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

