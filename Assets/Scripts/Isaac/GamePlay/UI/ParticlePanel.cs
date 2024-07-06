using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePanel : MonoBehaviour
{
    public Block[] blocks;
    private Block curBlock;

    public Transform HardnessStatusContainer;
    public Transform BouncenessStatusContainer;
    public Transform SmoothnessStatusContainer;

    public ParticleCreatePanel particleCreatePanel;

    private void Start()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_Block_Update, UpdateBlockUI);
    }
    private void OnEnable()
    {
        OnClickBlockIndex(0);
    }

    public void OnClickBlockIndex(int index)
    {
        curBlock = blocks[index];
        UpdateBlockUI();
        particleCreatePanel.LoadBlock(curBlock);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_Block_Update, UpdateBlockUI);

    }

    private void UpdateBlockUI()
    {
        foreach (Transform item in HardnessStatusContainer)
        {
            item.gameObject.SetActive(false);
        }
        foreach (Transform item in BouncenessStatusContainer)
        {
            item.gameObject.SetActive(false);
        }
        foreach (Transform item in SmoothnessStatusContainer)
        {
            item.gameObject.SetActive(false);
        }
        int hardness = (int)curBlock.hardness;
        int bounceness = (int)(curBlock.bounce*10f);
        int smoothness = (int)(curBlock.smooth * 10f);
        for (int i = 0; i < hardness; i++)
        {
            HardnessStatusContainer.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < bounceness; i++)
        {
            BouncenessStatusContainer.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < smoothness; i++)
        {
            SmoothnessStatusContainer.GetChild(i).gameObject.SetActive(true);
        }
    }
}
