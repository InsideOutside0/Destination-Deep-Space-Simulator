using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public Texture2D cursor;

    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // arrow
    }

    public void OnMouseOver()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware); // hand
    }
    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}
