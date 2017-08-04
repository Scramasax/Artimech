using UnityEngine;
using System.Collections;
using artiMech;

public class stateWindowsNode
{
    Rect m_WinRect;
    string m_WindowTitle = "";
    int m_Id = -1;
    string m_PathAndFileOfClass = "";

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

    public string PathAndFileOfClass
    {
        get
        {
            return m_PathAndFileOfClass;
        }

        set
        {
            m_PathAndFileOfClass = value;
        }
    }
    #endregion

    public stateWindowsNode(int id)
    {
        m_WinRect = new Rect();
        m_WindowTitle = "not filled in...";
        m_Id = id;
    }

    public void Set(string pathAndFileOfClass,string title,float x,float y,float width,float height)
    {
        m_PathAndFileOfClass = pathAndFileOfClass;
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

    public void SaveMetaData()
    {
        stateEditorUtils.SetPositionAndSizeOfAStateFile(m_PathAndFileOfClass, (int)m_WinRect.x,(int) m_WinRect.y, (int)m_WinRect.width, (int)m_WinRect.height);
    }
    
    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
        //GUI.FocusWindow(id);
    }
}
