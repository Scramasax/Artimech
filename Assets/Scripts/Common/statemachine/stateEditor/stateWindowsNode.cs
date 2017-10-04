using UnityEngine;
using System.Collections;
using artiMech;
using UnityEditor;

public class stateWindowsNode
{
    Rect m_WinRect;
    string m_WindowTitle = "";
    int m_Id = -1;
    string m_PathAndFileOfClass = "";
    baseState m_State = null;

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

    public Vector3 GetPos()
    {
        Vector3 tempVect= new Vector3();
        tempVect.x = m_WinRect.x;
        tempVect.y = m_WinRect.y;
        tempVect.z = 0;
        return tempVect;
    }

    public bool IsWithin(Vector2 vect)
    {
        if (vect.x >= m_WinRect.x && vect.x < m_WinRect.x + m_WinRect.width)
        {
            if (vect.y >= m_WinRect.y && vect.y < m_WinRect.y + m_WinRect.height)
            {
                return true;
            }
        }
        return false;
    }

    public void Update(baseState state)
    {
        m_State = state;
        GUI.Window(m_Id, WinRect, DrawNodeWindow, WindowTitle);
    }

    public void SaveMetaData()
    {
        stateEditorUtils.SetPositionAndSizeOfAStateFile(m_PathAndFileOfClass, (int)m_WinRect.x,(int) m_WinRect.y, (int)m_WinRect.width, (int)m_WinRect.height);
    }
    
    void DrawNodeWindow(int id)
    {
        if (Event.current.button == 1 && Event.current.isMouse)
        {
            if (m_State != null && m_State is editorDisplayWindowsState)
            {
                editorDisplayWindowsState dState = (editorDisplayWindowsState)m_State;
                if (dState != null && Event.current.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add Conditional"), false, dState.AddConditionalCallback, this);
                    stateEditorUtils.SelectedNode = this;
                    menu.ShowAsContext();
                    Event.current.Use();
                }
                //Debug.Log("--------------------------------------");
                return;
            }
        }
        GUI.DragWindow();
    }
}
