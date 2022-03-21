using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play(hoverSound, SoundManager.SoundPriority.NORMAL);
    }
}
