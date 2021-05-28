using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsToggle;
    public bool pressed;
    public  void OnPointerDown(PointerEventData eventData)
    {
        if (IsToggle)
            pressed = !pressed;
        else
            pressed = true;
    }

    public  void OnPointerUp(PointerEventData eventData)
    {
        if (IsToggle)
            return;
        pressed = false;
    }
}
