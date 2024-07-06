using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewLevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public LevelWrapper[] Levels;
}
[Serializable]
public class LevelWrapper
{
    public int LevelIndex;
    public string LevelName;
    public int LevelHighestScore;
    public int StarCount;
    public bool isUnlocked;
}
