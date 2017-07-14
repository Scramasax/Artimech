using UnityEngine;
using System.Collections;

public class stateWindowsNode
{
    Rect m_WinRect;
    string m_WindowTitle = "";
    int m_Id = -1;

    #region Accessors
    public Rect WinRect
    {
        get
        {
            return m_WinRect;
        }

        set
        {
            m_WinRect = value;
        }
    }

    public string WindowTitle
    {
        get
        {
            return m_WindowTitle;
        }

        set
        {
            m_WindowTitle = value;
        }
    }
    #endregion

    public stateWindowsNode(int id)
    {
        m_WinRect = new Rect();
        m_WindowTitle = "not filled in...";
        m_Id = id;
    }

    public void Set(string title,float x,float y,float width,float height)
    {
        m_WindowTitle = title;
        m_WinRect.x = x;
        m_WinRect.y = y;
        m_WinRect.width = width;
        m_WinRect.height = height;
    }

    public void SetPos(float x,float y)
    {
        m_WinRect.x = x;
        m_WinRect.y = y;
    }

    public void Update()
    {
        GUI.Window(m_Id, WinRect, DrawNodeWindow, WindowTitle);
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
    }
}
