using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverSetHandPointingCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHandCursorSet;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.instance.SetHandPointingCursor();
        isHandCursorSet = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.instance.ResetHandPoitingCursor();
        isHandCursorSet = false;
    }

    void OnDisable()
    {
        if (isHandCursorSet == false)
            return;

        CursorManager.instance.ResetHandPoitingCursor();
        isHandCursorSet = false;
    }
}