/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace artiMech
{
    public class stateWindowsNode
    {
        #region Variables

        Rect m_WinRect;
        string m_ClassName = "";
        string m_WindowStateAlias = "";
        int m_Id = -1;
        string m_PathAndFileOfClass = "";
        baseState m_State = null;

        IList<stateWindowsNode> m_ConditionLineList = new List<stateWindowsNode>();

        Rect m_CloseButtonRect;
        Rect m_MainBodyRectA;
        Rect m_MainBodyRectB;
        Rect m_ResizeBodyRect;
        Rect m_TitleRect;

        bool m_CloseButtonHover = false;
        bool m_MainBodyHover = false;
        bool m_ResizeBodyHover = false;
        bool m_TitleHover = false;

        #endregion
        #region Accessors

        /// <summary>  Returns true if the mouse cursor is hovering over the close button. </summary>
        public bool CloseButtonHover { get { return m_CloseButtonHover; } }

        /// <summary>  Returns true if the mouse cursor is hovering over the main body of the window. </summary>
        public bool MainBodyHover { get { return m_MainBodyHover; } }

        /// <summary>  Returns true if the mouse cursor is hovering over the resize widget. </summary>
        public bool ResizeBodyHover { get { return m_ResizeBodyHover; } }

        /// <summary>  Returns true if the mouse cursor is hovering over the window title area. </summary>
        public bool TitleHover { get { return m_TitleHover; } }

        /// <summary>  The entire window rectangle. </summary>
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

        /// <summary> Returns the name of the refrencing class the state. </summary>
        public string ClassName
        {
            get
            {
                return m_ClassName;
            }

            set
            {
                m_ClassName = value;
            }
        }

        /// <summary> Returns the path of the class. </summary>
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

        /// <summary> Returns a list of conditions. </summary>
        public IList<stateWindowsNode> ConditionLineList
        {
            get
            {
                return m_ConditionLineList;
            }

            set
            {
                m_ConditionLineList = value;
            }
        }

        /// <summary> Returns the name of the window in the title. </summary>
        public string WindowStateAlias
        {
            get
            {
                return m_WindowStateAlias;
            }

            set
            {
                m_WindowStateAlias = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public stateWindowsNode(int id)
        {
            m_WinRect = new Rect();
            m_ClassName = "not filled in...";
            m_WindowStateAlias = "not filled in yet...";
            m_Id = id;
        }

        /// <summary>
        /// Sets the various configuration varibles. 
        /// </summary>
        /// <param name="pathAndFileOfClass"></param>
        /// <param name="title"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Set(string pathAndFileOfClass, string classname, string title, float x, float y, float width, float height)
        {
            m_PathAndFileOfClass = pathAndFileOfClass;
            m_ClassName = classname;

            if(title!="nada")
                m_WindowStateAlias = title;
            else
                m_WindowStateAlias = classname;

            m_WinRect.x = x;
            m_WinRect.y = y;
            m_WinRect.width = width;
            m_WinRect.height = height;
        }

        /// <summary>
        /// Set the position of the window.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPos(float x, float y)
        {
            m_WinRect.x = x;
            m_WinRect.y = y;
        }

        /// <summary>
        /// Returns a Vector3 that contains the position of the window.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPos()
        {
            Vector3 tempVect = new Vector3();
            tempVect.x = m_WinRect.x;
            tempVect.y = m_WinRect.y;
            tempVect.z = 0;
            return tempVect;
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
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

            if(state is editorAddPostCondtionalState || state is editorMoveState || state is editorRenameState)
                GUI.Window(m_Id, WinRect, DrawNodeWindowNoDrag, m_WindowStateAlias);
            else
                GUI.Window(m_Id, WinRect, DrawNodeWindow, m_WindowStateAlias);

            //draw conditions
            Vector3 startPos = GetPos();
            startPos.x += WinRect.width * 0.5f;
            startPos.y += WinRect.height * 0.5f;

            for (int i = 0; i < this.ConditionLineList.Count; i++)
            {
                Vector3 endPos = ConditionLineList[i].GetPos();
                endPos.x += ConditionLineList[i].WinRect.width * 0.5f;
                endPos.y += ConditionLineList[i].WinRect.height * 0.5f;

                Color shadowCol = new Color(0, 0, 1, 0.06f);
                stateEditorDrawUtils.DrawArrow( startPos, endPos, WinRect, ConditionLineList[i].WinRect, 1, Color.black, 1, shadowCol,Color.white);
            }

        }
        /// <summary>
        /// Saves the positioning and other data in the comments via xml formatting in the refrencing state.
        /// </summary>
        public void SaveMetaData()
        {
            stateEditorUtils.SaveStateWindowsNodeData(m_PathAndFileOfClass, m_WindowStateAlias, (int)m_WinRect.x, (int)m_WinRect.y, (int)m_WinRect.width, (int)m_WinRect.height);
        }

        /// <summary>
        /// Draw the window without any funtionality.
        /// </summary>
        /// <param name="id"></param>
        void DrawNodeWindowNoDrag(int id)
        {
            //GUI.DragWindow();
        }

        /// <summary>
        /// Draws the "Display" window of this window.  Everything is active.
        /// </summary>
        /// <param name="id"></param>
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
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Edit Script"), false, dState.EditScriptCallback, this);
                        stateEditorUtils.SelectedNode = this;
                        menu.ShowAsContext();
                        Event.current.Use();
                    }
                    //Debug.Log("--------------------------------------");
                    return;
                }
            }

            //draw the exit button in the title bar
            Color shadowCol = new Color(0, 0, 0, 0.2f);
            float xOffset = 8.0f;
            float yOffset = 9.0f;
            float boxSize = 8;

            //create the close button rectangle
            m_CloseButtonRect = new Rect(this.WinRect.width  - (xOffset+(boxSize*0.5f)), yOffset - (boxSize * 0.5f), boxSize, boxSize);

            //EditorGUI.DrawRect(closeBoxRect, new Color(0.9f, 0.9f, 0.9f));

            if (m_CloseButtonRect.Contains(Event.current.mousePosition))
                stateEditorDrawUtils.DrawCubeFilled(new Vector3(WinRect.width - xOffset, yOffset, 0), boxSize, 1, Color.black, 1, shadowCol, Color.red);
            else
                stateEditorDrawUtils.DrawCubeFilled(new Vector3(WinRect.width-xOffset,yOffset,0), boxSize, 1, Color.black, 1, shadowCol, new Color(0.9f, 0.9f, 0.9f));

            stateEditorDrawUtils.DrawX(new Vector3(WinRect.width - (xOffset * 1.5f), yOffset * 0.5f, 0), boxSize - 1, boxSize - 1, 2, shadowCol);
            stateEditorDrawUtils.DrawX(new Vector3(WinRect.width - (xOffset*1.5f), yOffset*0.5f, 0), boxSize-1 , boxSize-1 , 1, Color.black);

            //draw the resizer
            const float initSizerSize = 15;
            float sizerSize = initSizerSize;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(WinRect.width-2 , this.WinRect.height-2,0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 10;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(WinRect.width - 2, this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 5;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(WinRect.width - 2, this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);

            const float titleHeight = 15;
            //create the main body rectangle.
            m_MainBodyRectA = new Rect(0, titleHeight, WinRect.width, WinRect.height - initSizerSize - titleHeight);
            m_MainBodyRectB = new Rect(0, titleHeight, WinRect.width - initSizerSize, WinRect.height);

            //main body of the window
            EditorGUIUtility.AddCursorRect(m_MainBodyRectA, MouseCursor.MoveArrow);
            EditorGUIUtility.AddCursorRect(m_MainBodyRectB, MouseCursor.MoveArrow);

            //resize
            m_ResizeBodyRect = new Rect(new Rect(WinRect.width - initSizerSize, WinRect.height - initSizerSize, initSizerSize, initSizerSize));
            EditorGUIUtility.AddCursorRect(m_ResizeBodyRect, MouseCursor.ResizeUpLeft);

            //title
            const float rightMarginSize = 15;
            m_TitleRect = new Rect(0, 0, WinRect.width - rightMarginSize, titleHeight);
            EditorGUIUtility.AddCursorRect(m_TitleRect, MouseCursor.Text);

            //close box
            EditorGUIUtility.AddCursorRect(m_CloseButtonRect, MouseCursor.ArrowMinus);

            UpdateMouseHover(Event.current.mousePosition);

            GUI.DragWindow();
        }

        /// <summary>
        /// Sets the various hover bools so that the selected node can tell an external 
        /// peice of code what mouse is hovering over.  That way contextual input can be
        /// achieved.
        /// </summary>
        /// <param name="mousePos"></param>
        void UpdateMouseHover(Vector2 mousePos)
        {
            m_CloseButtonHover = m_CloseButtonRect.Contains(mousePos);
            m_ResizeBodyHover = m_ResizeBodyRect.Contains(mousePos);
            m_TitleHover = m_TitleRect.Contains(mousePos);

            m_MainBodyHover = m_MainBodyRectA.Contains(mousePos);
            if (m_MainBodyHover)
                return;

            m_MainBodyHover = m_MainBodyRectB.Contains(mousePos);
        }
    }
}