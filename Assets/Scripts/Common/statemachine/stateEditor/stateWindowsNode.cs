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
        Rect m_WinRect;
        string m_WindowTitle = "";
        int m_Id = -1;
        string m_PathAndFileOfClass = "";
        baseState m_State = null;

        IList<stateWindowsNode> m_ConditionLineList = new List<stateWindowsNode>();

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
        #endregion

        public stateWindowsNode(int id)
        {
            m_WinRect = new Rect();
            m_WindowTitle = "not filled in...";
            m_Id = id;
        }

        public void Set(string pathAndFileOfClass, string title, float x, float y, float width, float height)
        {
            m_PathAndFileOfClass = pathAndFileOfClass;
            m_WindowTitle = title;
            m_WinRect.x = x;
            m_WinRect.y = y;
            m_WinRect.width = width;
            m_WinRect.height = height;
        }

        public void SetPos(float x, float y)
        {
            m_WinRect.x = x;
            m_WinRect.y = y;
        }

        public Vector3 GetPos()
        {
            Vector3 tempVect = new Vector3();
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

            if(state is editorAddPostCondtionalState)
                GUI.Window(m_Id, WinRect, DrawNodeWindowNoDrag, WindowTitle);
            else
                GUI.Window(m_Id, WinRect, DrawNodeWindow, WindowTitle);

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

        public void SaveMetaData()
        {
            stateEditorUtils.SetPositionAndSizeOfAStateFile(m_PathAndFileOfClass, (int)m_WinRect.x, (int)m_WinRect.y, (int)m_WinRect.width, (int)m_WinRect.height);
        }

        void DrawNodeWindowNoDrag(int id)
        {
            //GUI.DragWindow();
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
            Color shadowCol = new Color(0, 0, 0, 0.2f);
            float xOffset = 8.0f;
            float yOffset = 9.0f;
            float boxSize = 8;
            stateEditorDrawUtils.DrawCubeFilled(new Vector3(this.WinRect.width-xOffset,yOffset,0), boxSize, 1, Color.black, 1, shadowCol, Color.clear);

            stateEditorDrawUtils.DrawX(new Vector3(this.WinRect.width - (xOffset * 1.5f), yOffset * 0.5f, 0), boxSize - 1, boxSize - 1, 2, shadowCol);
            stateEditorDrawUtils.DrawX(new Vector3(this.WinRect.width - (xOffset*1.5f), yOffset*0.5f, 0), boxSize-1 , boxSize-1 , 1, Color.black);

            float sizerSize = 15;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(this.WinRect.width-2 , this.WinRect.height-2,0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 10;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(this.WinRect.width - 2, this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 5;
            stateEditorDrawUtils.DrawWindowSizer(new Vector3(this.WinRect.width - 2, this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);

            GUI.DragWindow();
        }

        void DrawConditionCurve(Vector3 startPos, Vector3 endPos)
        {
            Vector3 startTan = new Vector3();
            Vector3 endTan = new Vector3();
            float inSize = 50;
            float outSize = 75;
            if (startPos.x < endPos.x)
            {
                startTan = startPos + Vector3.right * inSize;
                endTan = endPos + Vector3.left * outSize;
            }
            else
            {
                startTan = startPos + Vector3.right * -inSize;
                endTan = endPos + Vector3.left * -outSize;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

            //draw shadow
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 4);
        }
    }
}