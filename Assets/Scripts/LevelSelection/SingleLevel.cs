using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

public class SingleLevel : BasePanel
{
    private int currentStarsNum = 0;
    public int levelIndex;
    ScenesMgr scenes;
    private void func1() { }
    public void BackButton()
    {
        scenes.LoadScene("", func1);//加载主界面场景
    }
    public void PressStarsButton(int _starNum)
    {
        currentStarsNum = _starNum;
        if (currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, _starNum);
        }

        Debug.Log('1');
    }
}
