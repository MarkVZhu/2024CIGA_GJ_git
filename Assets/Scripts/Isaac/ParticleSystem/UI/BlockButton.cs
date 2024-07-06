using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour
{
    [SerializeField]
    private Image buttonImage;


    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }
    public void SetSprite(Sprite sprite)
    {
        buttonImage.sprite = sprite;
    }


}
