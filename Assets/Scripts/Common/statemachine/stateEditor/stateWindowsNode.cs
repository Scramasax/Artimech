using UnityEngine;
using System.Collections;

public class stateWindowsNode
{
    Rect m_WinRect;
    string m_WindowTitle = "";

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
}
