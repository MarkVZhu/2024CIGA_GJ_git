using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

public class LevelSelection : BasePanel { 
    [SerializeField] private bool unlocked;
    public Image unlockImage;
    public GameObject[] stars;
    ScenesMgr scenes;

    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus();
    }
    
    private void UpdateLevelStatus()
    {
        int previousLevelNum = int.Parse(gameObject.name) - 1;
        if(PlayerPrefs.GetInt("Lv"+previousLevelNum)>0)
        {
            unlocked = true;
        }

    }
    private void UpdateLevelImage()
    {
        //���¹ؿ��Ƿ��Ѿ�����
        if(!unlocked)//�ؿ���δ����,Ĭ����false
        {
            unlockImage.gameObject.SetActive(true);
            for(int i=0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            unlockImage.gameObject.SetActive(false);
            for(int i=0;i< stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }
    }
    private void func1() { }
    private void PressSelection(string _levelName)
    {
        if(unlocked)
        {
            scenes.LoadScene(_levelName,func1);
        }
    }
}

