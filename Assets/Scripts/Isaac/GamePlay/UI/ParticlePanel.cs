using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticlePanel : MonoBehaviour
{
    public Block[] blocks;
    private Block curBlock;

    public Transform HardnessStatusContainer;
    public Transform BouncenessStatusContainer;
    public Transform SmoothnessStatusContainer;

    public BlockButton[] BlockButtons;
    public Sprite NormalSprite;
    public Sprite ChosenSprite;

    public Image PanelBackground;

    public ParticleCreatePanel particleCreatePanel;

    private void Awake()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_Block_Update, UpdateBlockUI);
        UpdateBlockButtonColors();

        UpdatePanelBackgroundUI();
    }
    private void Start()
    {
    }
    private void OnEnable()
    {
        
    }

    public void OnClickBlockIndex(int index)
    {
        curBlock = blocks[index];
        UpdateBlockUI();
        UpdateBlockButtonUI(index);
        UpdatePanelBackgroundUI();

        particleCreatePanel.LoadBlock(curBlock);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_Block_Update, UpdateBlockUI);

    }
    private void UpdateBlockButtonUI(int index)
    {
        UpdateBlockButtonColors();
        for (int i = 0; i < BlockButtons.Length; i++)
        {

            if (index == i)
            {
                BlockButtons[i].transform.parent.GetComponent<Image>().enabled = true;
            }
            else
            {
                BlockButtons[i].transform.parent.GetComponent<Image>().enabled = false;
            }
        }

    }
    private void UpdateBlockButtonColors()
    {
        for (int i = 0; i < BlockButtons.Length; i++)
        {

            BlockButtons[i].GetComponent<Image>().color = GetColor(blocks[i].hardness, blocks[i].bounce, blocks[i].smooth);

        }
    }
    private void UpdatePanelBackgroundUI()
    {
        PanelBackground.color = GetColor(curBlock.hardness, curBlock.bounce, curBlock.smooth);
    }
    private void UpdateBlockUI()
    {

        UpdatePanelBackgroundUI();

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
    public Color GetColor(int hardness, float bounce, float smooth)
    {
        // Clamp the input values to their respective ranges
        hardness = Mathf.Clamp(hardness, 0, 10);
        bounce = Mathf.Clamp01(bounce);
        smooth = Mathf.Clamp01(smooth);

        // Map the values to 0~255 range
        int r = Mathf.RoundToInt(Map(hardness, 0, 10, 50, 150));
        int g = Mathf.RoundToInt(Map(bounce, 0, 1, 0, 255));
        int b = Mathf.RoundToInt(Map(smooth, 0, 1, 0, 255));

        // Create the color
        return new Color32((byte)r, (byte)g, (byte)b, 255);
    }
    private float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
