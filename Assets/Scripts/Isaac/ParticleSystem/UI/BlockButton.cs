using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    private Image buttonImage;
    public Color ButtonNormalColor;
    public Color ButtonEnterColor;
    public Color ButtonClickColor;
    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = ButtonEnterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = ButtonEnterColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.color = ButtonNormalColor;
    }
}
