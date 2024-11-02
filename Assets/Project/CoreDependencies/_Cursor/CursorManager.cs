using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance = null;
    public Texture2D handOpenedCursor = null;
    public Texture2D handClosedCursor = null;
    public Texture2D handPointingCursor = null;
    
    [Header("DO NOT TOUCH - FOR INSPECTION ONLY")]
    [SerializeField] private Texture2D previousCursor = null;
    [SerializeField] private Texture2D currentCursor = null;

    public void SetCustomCursor(Texture2D customCursor) => SetCursor(customCursor);
    public void SetCustomCursor(Texture2D customCursor, Vector2 hotspot) => SetCursor(customCursor, hotspot);
    public void SetHandOpenedCursor() { if (currentCursor != handOpenedCursor) SetCursor(handOpenedCursor); }
    public void SetClosedHandCursor() => SetCursor(handClosedCursor);
    public void SetHandPointingCursor() { if (currentCursor != handPointingCursor) SetCursor(handPointingCursor); }
    public void ResetCursor()  => SetCursor(null);
    public void ResetToPreviousCursor() => SetCursor(previousCursor);

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void SetCursor(Texture2D cursor)
    {
        previousCursor = currentCursor;
        currentCursor = cursor;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    private void SetCursor(Texture2D cursor, Vector2 hotspot)
    {
        previousCursor = currentCursor;
        currentCursor = cursor;
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    public void ResetCursorIfThisCursorIsSet(Texture2D cursorToMatch)
    {
        if (currentCursor == cursorToMatch)
            ResetCursor();
        else
            previousCursor = null;
    }

    public void ResetHandCursor()
    {
        if ((currentCursor == handOpenedCursor || currentCursor == handClosedCursor))
            ResetCursor();
        else
            previousCursor = null;
    }

    public void ResetHandPoitingCursor()
    {
        if ((currentCursor == handPointingCursor))
            ResetToPreviousCursor();
    }
}