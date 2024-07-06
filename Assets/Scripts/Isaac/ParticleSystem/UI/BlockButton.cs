using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour
{
    private Image buttonImage;
    public Color ButtonNormalColor;
    public Color ButtonEnterColor;
    public Color ButtonClickColor;
    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }

}
