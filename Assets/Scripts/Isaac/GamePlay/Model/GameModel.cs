using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameModel : ViewModel
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
