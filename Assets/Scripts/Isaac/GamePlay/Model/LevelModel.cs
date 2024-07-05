using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelModel : ViewModel
{
    private int starCount;
    public int StarCount
    {
        get => starCount;
        set
        {
            ChangePropertyAndNotify<int>(ref starCount, value);
        }
    }
}
