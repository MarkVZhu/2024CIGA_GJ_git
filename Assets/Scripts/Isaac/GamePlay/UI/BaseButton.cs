using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SoundMgr.Instance.PlaySoundRandom(new string[] { "ButtonClick_page1", "ButtonClick_page2", "ButtonClick_page3", "ButtonClick_page4" });
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }


}
