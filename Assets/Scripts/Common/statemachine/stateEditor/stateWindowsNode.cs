using UnityEngine;
using System.Collections;

public class stateWindowsNode
{
    Rect m_WinRect;
    string m_WindowTitle = "";

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

    public stateWindowsNode()
    {
        m_WinRect = new Rect();
        m_WindowTitle = "not filled in...";
    }

    public void Set(string title,float x,float y,float width,float height)
    {
        m_WindowTitle = title;
        m_WinRect.x = x;
        m_WinRect.x = y;
        m_WinRect.width = width;
        m_WinRect.height = height;
    }
}
