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
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    public class artVisualStateNode
    {
        public class conditionalSelection
        {
            public artVisualStateNode m_VisualStateNode;
            public int m_SelectedIndex;
            public conditionalSelection(artVisualStateNode node, int index)
            {
                m_VisualStateNode = node;
                m_SelectedIndex = index;
            }
        }

        #region Variables

        Rect m_WinRect;
        string m_ClassName = "";
        string m_WindowStateAlias = "";
        int m_Id = -1;
        string m_PathAndFileOfClass = "";
        baseState m_State = null;

        IList<artVisualStateNode> m_ConditionLineList = new List<artVisualStateNode>();

        Rect m_CloseButtonRect;
        Rect m_MainBodyRectA;
        Rect m_MainBodyRectB;
        Rect m_ResizeBodyRect;
        Rect m_TitleRect;

        bool m_CloseButtonHover = false;
        bool m_MainBodyHover = false;
        bool m_ResizeBodyHover = false;
        bool m_TitleHover = false;

        bool m_MoveBool = false;
        bool m_LeftMouseButton = false;
        bool m_Selected = false;
        bool m_Resize = false;
        bool m_RenameBool = false;
        bool m_RefactorBool = false;


        Vector2 m_ConditionOffset;

        Vector3 m_LinePos;

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
        public Rect WinRect { get { return m_WinRect; } set { m_WinRect = value; } }

        /// <summary> Returns the name of the refrencing class the state. </summary>
        public string ClassName { get { return m_ClassName; } set { m_ClassName = value; } }

        /// <summary> Returns the path of the class. </summary>
        public string PathAndFileOfClass { get { return m_PathAndFileOfClass; } set { m_PathAndFileOfClass = value; } }

        /// <summary> Returns a list of conditions. </summary>
        public IList<artVisualStateNode> ConditionLineList { get { return m_ConditionLineList; } set { m_ConditionLineList = value; } }

        /// <summary> Returns the name of the window in the title. </summary>
        public string WindowStateAlias { get { return m_WindowStateAlias; } set { m_WindowStateAlias = value; } }

        public Vector3 LinePos { get { return m_LinePos; } set { m_LinePos = value; } }

        public bool MoveBool { get { return m_MoveBool; } set { m_MoveBool = value; } }

        public bool LeftMouseButton { get { return m_LeftMouseButton; } set { m_LeftMouseButton = value; } }

        public bool Selected { get { return m_Selected; } set { m_Selected = value; } }

        public bool Resize { get { return m_Resize; } set { m_Resize = value; } }

        public bool RenameBool { get { return m_RenameBool; } set { m_RenameBool = value; } }

        public bool RefactorBool
        {
            get
            {
                return m_RefactorBool;
            }

            set
            {
                m_RefactorBool = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public artVisualStateNode(int id)
        {
            m_WinRect = new Rect();
            m_ClassName = "not filled in...";
            m_WindowStateAlias = "not filled in yet...";
            m_Id = id;
            m_ConditionOffset.x = 15f;
            m_ConditionOffset.y = 15f;
        }

        public void Reset()
        {
            m_MoveBool = false;
            m_LeftMouseButton = false;
            m_Selected = false;
            m_Resize = false;
            m_RenameBool = false;
            m_RefactorBool = false;
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

            if (title != "nada")
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
        /// returns the position of the window with the scroll/pan mtx transform.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetTransformedPos(utlMatrix34 mtx)
        {
            return mtx.Transform(GetPos());
        }

        /// <summary>
        /// Is within the window using the scroll transform.
        /// </summary>
        /// <param name="tranVect"></param>
        /// <returns></returns>
        public bool IsWithinUsingPanZoomTransform(Vector2 vect, utlMatrix34 mtx)
        {
            Vector3 transVect = new Vector3();
            transVect = mtx.UnTransform(vect);
            if (transVect.x >= m_WinRect.x && transVect.x < m_WinRect.x + m_WinRect.width)
            {
                if (transVect.y >= m_WinRect.y && transVect.y < m_WinRect.y + m_WinRect.height)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is within rect of the state window.
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

        public Vector3 GetClosetPointOnConditional(Vector3 pos)
        {
            Vector3 vectOut = new Vector3();
            Vector3 startPos = GetPos();
            startPos.x += WinRect.width * 0.5f;
            startPos.y += WinRect.height * 0.5f;

#pragma warning disable CS0162 // Unreachable code detected
            for (int i = 0; i < ConditionLineList.Count; i++)
#pragma warning restore CS0162 // Unreachable code detected
            {
                Vector3 endPos = ConditionLineList[i].GetPos();
                endPos.x += ConditionLineList[i].WinRect.width * 0.5f;
                endPos.y += ConditionLineList[i].WinRect.height * 0.5f;

                vectOut = utlMath.NearestPointOnLine(pos, startPos, endPos);
                //vectOut = utlMath.GetClosestPointOnLineSegment(pos, startPos, endPos);


                //Debug.Log("nearestPointOnLine = " + vectOut);
                //Debug.Log("startPos = " + startPos);
                //Debug.Log("endPos = " + endPos);
                //Debug.Log("distance = " + distance);

                return vectOut;
            }
            return vectOut;
        }

        public Vector3 GetStartPosOnCondition()
        {
            Vector3 startPos = GetPos();
            startPos.x += WinRect.width * 0.5f;
            startPos.y += WinRect.height * 0.5f;
            return startPos;
        }

        public Vector3 GetEndPosOnCondition()
        {
            Vector3 vectOut = new Vector3();
#pragma warning disable CS0162 // Unreachable code detected
            for (int i = 0; i < ConditionLineList.Count; i++)
#pragma warning restore CS0162 // Unreachable code detected
            {
                Vector3 endPos = ConditionLineList[i].GetPos();
                endPos.x += ConditionLineList[i].WinRect.width * 0.5f;
                endPos.y += ConditionLineList[i].WinRect.height * 0.5f;
                return endPos;
            }
            return vectOut;
        }

        /// <summary>
        /// Returns null if there isn't a conditional line segement within
        /// the distance threshold otherwise it returns the closest class name to the
        /// line segment.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distThreshold"></param>
        /// <returns>Name of the condition class.</returns>
        public string GetConditionalByPosition(Vector3 pos, float distThreshold)
        {
            //           Vector3 startPos = GetPos();
            //           startPos.x += WinRect.width * 0.5f;
            //           startPos.y += WinRect.height * 0.5f;
            Vector3 startPos = GetStartPosForConditional();
            for (int i = 0; i < ConditionLineList.Count; i++)
            {
                /* Vector3 endPos = ConditionLineList[i].GetPos();
                 endPos.x += ConditionLineList[i].WinRect.width * 0.5f;
                 endPos.y += ConditionLineList[i].WinRect.height * 0.5f;*/

                Vector3 endPos = GetEndPosForConditional(ConditionLineList[i]);

                Vector3 nearestPointOnLine = utlMath.NearestPointOnLine(pos, startPos, endPos);
                m_LinePos = nearestPointOnLine;
                float distance = Vector3.Distance(pos, nearestPointOnLine);
                //Debug.Log("nearestPointOnLine = " + nearestPointOnLine);
                //Debug.Log("startPos = " + startPos);
                //Debug.Log("endPos = " + endPos);
                //Debug.Log("distance = " + distance);
                if (distance < distThreshold)
                    return ClassName + "_To_" + ConditionLineList[i].ClassName;
            }

            return null;
        }

        public void Update(baseState state, utlMatrix34 mtx, artConfigurationData configData)
        {
            m_State = state;

            MoveBool = false;

            // input
            Event ev = Event.current;
            //stateEditorUtils.MousePos = ev.mousePosition;

            //if the mouse button is down.
            if (ev.type == EventType.MouseDown && ev.button == 0)
            {
                LeftMouseButton = true;
                if (m_MainBodyHover)
                {
                    MoveBool = true;
                    Selected = true;
                }

                if (m_ResizeBodyHover)
                {
                    this.Resize = true;
                }
                if (this.m_TitleHover)
                {
                    RenameBool = true;
                }
            }

            if (ev.type == EventType.MouseUp)
            {
                LeftMouseButton = false;
                Selected = false;
                Resize = false;
                //RenameBool = false;
            }

            //GUI.Window(m_Id, mtx.Transform(WinRect), DrawNodeWindow, m_WindowStateAlias);


            Vector3 startPos = GetStartPosForConditional();

            for (int i = 0; i < this.ConditionLineList.Count; i++)
            {

                Vector3 endPos = GetEndPosForConditional(ConditionLineList[i]);

                Color shadowCol = new Color(0, 0, 1, 0.06f);
                //artGUIUtils.DrawArrowTranformed(mtx, startPos, endPos, WinRect, ConditionLineList[i].WinRect, 1, Color.black, 1, shadowCol, Color.white);
                artGUIUtils.DrawArrowTranformed(mtx, startPos, endPos, WinRect, ConditionLineList[i].WinRect, configData.ArrowLineWidth, configData.ArrowLineColor, configData.ArrowFillColor);
            }
            DrawCurrentRunningState(configData);
            DrawNodeWindow(this.m_Id, configData);
            //DrawCurrentRunningState();
        }

        void DrawCurrentRunningState(artConfigurationData configData)
        {
            if (!Application.isPlaying)
                return;

            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;

            GameObject gameObj = null;
            iMachineBase stateMachine;
            if (theStateMachineEditor.SelectedObj is GameObject)
            {
                gameObj = (GameObject)theStateMachineEditor.SelectedObj;
                stateMachine = gameObj.GetComponent<iMachineBase>();
            }
            else
            {
                stateMachine = (iMachineBase)theStateMachineEditor.SelectedObj;
            }
            string currentClassName = stateMachine.GetCurrentState().GetType().ToString();
            currentClassName = utlDataAndFile.GetAfter(currentClassName, ".");

            if (currentClassName == this.ClassName)
            {
                Rect drawRect = new Rect(WinRect.x - configData.DebugBoxSize,
                                    WinRect.y - configData.DebugBoxSize,
                                    WinRect.width + (configData.DebugBoxSize * 2),
                                    WinRect.height + (configData.DebugBoxSize * 2));
                artGUIUtils.DrawRect(drawRect, configData.DebugBoxLineSize, configData.DebugBoxLineColor, configData.DebugBoxColor);
            }
            //theStateMachineEditor.EditorRepaint();

        }

        public bool CheckWinNodeToSeeIfItIsLinked(artVisualStateNode winNode)
        {
            for (int i = 0; i < winNode.ConditionLineList.Count; i++)
            {
                if (this == winNode.ConditionLineList[i])
                {
                    //Debug.Log(this.ClassName + "-> " + winNode.ClassName);
                    return true;
                }
            }
            return false;
        }

        Vector3 GetStartPosForConditional()
        {
            Vector3 tempVect = new Vector3();

            tempVect = GetPos();
            tempVect.x += WinRect.width * 0.5f;
            tempVect.y += WinRect.height * 0.5f;

            //loop through my conditional list
            for (int i = 0; i < this.ConditionLineList.Count; i++)
            {

                //check to see if there is another conditional linking back to this state.
                //if (CheckWinNodeToSeeIfItIsLinked(ConditionLineList[i]))
                if (this.ClassName == ConditionLineList[i].ClassName)
                {
                    //                   Debug.Log("*>" + this.ClassName);
                    //                   Debug.Log("->" + ConditionLineList[i].ClassName);
                    if (ConditionLineList[i].GetPos().x <= GetPos().x)
                        tempVect.x -= m_ConditionOffset.x;
                    else
                        tempVect.x += m_ConditionOffset.x;

                    if (ConditionLineList[i].GetPos().y <= GetPos().y)
                        tempVect.y -= m_ConditionOffset.y;
                    else
                        tempVect.y += m_ConditionOffset.y;

                    //Debug.Log(this.m_ClassName);

                    return tempVect;
                }
            }

            //Debug.Log(this.ClassName);

            return tempVect;
        }

        Vector3 GetEndPosForConditional(artVisualStateNode connectedNode)
        {
            Vector3 tempVect = new Vector3();

            tempVect = connectedNode.GetPos();
            tempVect.x += connectedNode.WinRect.width * 0.5f;
            tempVect.y += connectedNode.WinRect.height * 0.5f;

            if (this.CheckWinNodeToSeeIfItIsLinked(connectedNode))
            {
                if (connectedNode.GetPos().x < GetPos().x)
                    tempVect.x -= m_ConditionOffset.x;
                else
                    tempVect.x += m_ConditionOffset.x;

                if (connectedNode.GetPos().y < GetPos().y)
                    tempVect.y -= m_ConditionOffset.y;
                else
                    tempVect.y += m_ConditionOffset.y;

            }

            return tempVect;
        }

        /// <summary>
        /// Saves the positioning and other data in the comments via xml formatting in the refrencing state.
        /// </summary>
        public void SaveMetaData()
        {
            artEditorUtils.SaveStateMetaData(m_PathAndFileOfClass, m_WindowStateAlias, (int)m_WinRect.x, (int)m_WinRect.y, (int)m_WinRect.width, (int)m_WinRect.height);
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
        void DrawNodeWindow(int id, artConfigurationData configData)
        {
            if (Event.current.button == 0 && Event.current.isMouse && ResizeBodyHover)
            {

            }

            if (Event.current.button == 1 && Event.current.isMouse)
            {
                //if (m_State != null && m_State is editorDisplayWindowsState)
                ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
                if (IsWithinUsingPanZoomTransform(Event.current.mousePosition, theStateMachineEditor.TransMtx))
                {
                    artDisplayWindowsBaseState dState = (artDisplayWindowsBaseState)m_State;
                    if (dState != null && Event.current.type == EventType.MouseDown && dState.DrawMenuBool)
                    {
                        GenericMenu menu = new GenericMenu();
                        for (int i = 0; i < configData.ConditionalCopyInfo.Length; i++)
                        {



                            menu.AddItem(new GUIContent("Add Conditional/" + configData.ConditionalCopyInfo[i].m_MenuString),
                                false,
                                dState.AddConditionalCallback,
                                new conditionalSelection(this, i));
                        }
                        /* menu.AddItem(new GUIContent("Add Conditional/Subscription Conditional"),
                             false,
                             dState.AddConditionalCallback,
                             new editorDisplayWindowsState.menuData("Assets/Scripts/Common/statemachine/state_examples/stateCondSubExample.cs", "stateCondSubExample"));*/

                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Edit Script"), false, dState.EditScriptCallback, this);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Refactor State Class"), false, this.RefactorClassCallback, this);
                        //stateEditorUtils.SelectedNode = this;
                        menu.ShowAsContext();
                        Event.current.Use();
                    }
                    //Debug.Log("--------------------------------------");
                    return;
                }
            }

            //draws background and outline.
            //int lineWidth = 2;
            /*          Color lineColor = new Color(0, 0.3f, 0, 1);
                      Color backGroundColor = new Color(0.0f, 0.8f, 0.8f, 1.0f);*/

            Rect rect = new Rect(m_WinRect.x, m_WinRect.y, WinRect.width, WinRect.height);
            artGUIUtils.DrawRect(rect, configData.WindowOutlineLineWidth, configData.WindowLineColor, configData.StateWindowColor);


            DrawStateContent(configData);


            //draw the exit button in the title bar
            Color shadowCol = new Color(0, 0, 0, 0.2f);
            float xOffset = 8.0f;
            float yOffset = 9.0f;
            float boxSize = 8;

            //create the close button rectangle
            m_CloseButtonRect = new Rect(WinRect.x + WinRect.width - (xOffset + (boxSize * 0.5f)), WinRect.y + yOffset - (boxSize * 0.5f), boxSize, boxSize);
            if (m_CloseButtonRect.Contains(Event.current.mousePosition))
            {
                artGUIUtils.DrawRect(m_CloseButtonRect, 2, Color.black, Color.red);
                if (Event.current.button == 0 && Event.current.isMouse)
                {
                    ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
                    theStateMachineEditor.DeleteStateBool = true;
                    theStateMachineEditor.DeleteStateClass = ClassName;
                }
            }
            else
                artGUIUtils.DrawRect(m_CloseButtonRect, 2, Color.black, Color.clear);

            artGUIUtils.DrawX(new Vector3(WinRect.x + WinRect.width - (xOffset * 0.95f), WinRect.y + yOffset * 0.95f, 0), boxSize - 1, boxSize - 1, 2, shadowCol);
            artGUIUtils.DrawX(new Vector3(WinRect.x + WinRect.width - (xOffset * 1.0f), WinRect.y + yOffset * 1.0f, 0), boxSize - 1, boxSize - 1, 1, Color.black);

            //draw the resizer
            const float initSizerSize = 15;
            float sizerSize = initSizerSize;
            artGUIUtils.DrawWindowSizer(new Vector3(WinRect.x + WinRect.width - 2, WinRect.y + this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 10;
            artGUIUtils.DrawWindowSizer(new Vector3(WinRect.x + WinRect.width - 2, WinRect.y + this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);
            sizerSize = 5;
            artGUIUtils.DrawWindowSizer(new Vector3(WinRect.x + WinRect.width - 2, WinRect.y + this.WinRect.height - 2, 0), sizerSize - 1, sizerSize - 3, 2, Color.grey);

            const float titleHeight = 15;
            //create the main body rectangle.
            m_MainBodyRectA = new Rect(WinRect.x, WinRect.y + titleHeight, WinRect.width, WinRect.height - initSizerSize - titleHeight);
            //m_MainBodyRectB = new Rect(WinRect.x, WinRect.y + titleHeight, WinRect.width - initSizerSize, WinRect.y - WinRect.height);

            //main body of the window
            EditorGUIUtility.AddCursorRect(m_MainBodyRectA, MouseCursor.MoveArrow);
            // EditorGUIUtility.AddCursorRect(m_MainBodyRectB, MouseCursor.MoveArrow);

            //resize
            m_ResizeBodyRect = new Rect(new Rect(WinRect.x + WinRect.width - initSizerSize, WinRect.y + WinRect.height - initSizerSize, initSizerSize, initSizerSize));
            EditorGUIUtility.AddCursorRect(m_ResizeBodyRect, MouseCursor.ResizeUpLeft);

            //title
            const float rightMarginSize = 15;
            m_TitleRect = new Rect(WinRect.x, WinRect.y, WinRect.width - rightMarginSize, titleHeight);
            EditorGUIUtility.AddCursorRect(m_TitleRect, MouseCursor.Text);

            //close box
            EditorGUIUtility.AddCursorRect(m_CloseButtonRect, MouseCursor.ArrowMinus);

            //DrawStateContent();

            UpdateMouseHover(Event.current.mousePosition);

            //artGUIUtils.DrawX(m_LinePos, 10, 10, 1, Color.black);

            //GUI.DragWindow();
        }

        void DrawStateContent(artConfigurationData configData)
        {
            GUILayout.BeginArea(WinRect);

            var rect = EditorGUILayout.BeginVertical();
            GUI.Box(rect, GUIContent.none);

            //GUI.color = Color.white;

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.Label(this.WindowStateAlias);
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.EndArea();

            //Handles.EndGUI();
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

        public void MoveVisualNodeByMousePosition(Vector2 mousePos, Vector2 startOffset)
        {
            // this.SetPos(mousePos.x - startOffset.x, mousePos.y - startOffset.y);
            //this.SetPos(mousePos.x - (this.WinRect.width * 0.5f), mousePos.y - (this.WinRect.height * 0.5f));
            //Vector2 offset = startOffset
            this.SetPos(mousePos.x, mousePos.y);
        }

        public void RefactorClassCallback(object obj)
        {
            RefactorBool = true;
        }
    }
}
#endif