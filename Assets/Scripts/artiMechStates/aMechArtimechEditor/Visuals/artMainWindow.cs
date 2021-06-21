using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Artimech
{
    /// <summary>
    /// Base window for the Artimech state editor system
    /// </summary>
    public class artMainWindow : artWindowBase
    {

        public class ConditionArrowTemp
        {
            Vector3 m_StartPos;
            Vector3 m_EndPos;
            Rect m_RectStart;
            Rect m_RectEnd;
            int m_LineWidth;
            Color m_LineColor;
            int m_ShadowWidth;
            Color m_ShadowColor;
            Color m_BodyColor;
            public ConditionArrowTemp(Vector3 startPos, Vector3 endPos, Rect winRectStart, 
                                        Rect winRectEnd, int lineWidth, Color lineColor, 
                                        int shadowWidth, Color shadowColor, Color bodyColor)
            {
                m_StartPos = startPos;
                m_EndPos = endPos;
                m_RectStart = winRectStart;
                m_RectEnd = winRectEnd;
                m_LineWidth = lineWidth;
                m_LineColor = lineColor;
                m_ShadowWidth = shadowWidth;
                m_ShadowColor = shadowColor;
                m_BodyColor = bodyColor;
            }

            public void DrawArrow()
            {
                artGUIUtils.DrawArrow(m_StartPos,m_EndPos,m_RectStart,m_RectEnd,m_LineWidth,m_LineColor,m_ShadowWidth,m_ShadowColor, m_BodyColor);
            }
        }

        #region Variables
        baseState m_State;
        Vector2 m_MousePos;

        static int m_Count = 0;
        bool m_AlreadyDrawn = false;
        int m_MyCount = 0;

        int m_GameStateSelectionIndex = 0;

        ConditionArrowTemp m_ArrowTemp;


        #endregion
        #region Gets Sets
        public Vector2 MousePos
        {
            get
            {
                return m_MousePos;
            }

            set
            {
                m_MousePos = value;
            }
        }

        public bool AlreadyDrawn { get => m_AlreadyDrawn; set => m_AlreadyDrawn = value; }
        public ConditionArrowTemp ArrowTemp { get => m_ArrowTemp; set => m_ArrowTemp = value; }
        #endregion
        #region Member Functions


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artMainWindow(baseState state, string title, Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            m_State = state;
            m_Count += 1;
            m_MyCount = m_Count;
            // Debug.Log("m_Count" + m_Count);
        }

        /// <summary>
        /// Update
        /// </summary>
        new public void Update()
        {
            //if (m_MyCount == 1)
            {
                m_WinRect.width = Screen.width;
                m_WinRect.height = Screen.height;
                GUI.Window(m_Id, WinRect, Draw, "Artimech");
            }
            // Debug.Log("Update m_Count" + m_Count);
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        new public bool IsWithin(Vector2 vect)
        {
            if (vect.x >= WinRect.x && vect.x < WinRect.x + WinRect.width)
            {
                if (vect.y >= WinRect.y && vect.y < WinRect.y + WinRect.height)
                {
                    return true;
                }
            }
            return false;
        }

        new public void Draw(int id)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;

            m_MousePos = Event.current.mousePosition;
            Rect rect = new Rect(0, 0, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, theStateMachineEditor.ConfigData.BackgroundColor);


            stateEditorDrawUtils.DrawGridBackground(theStateMachineEditor.ConfigData);
            DrawBackGroundWindow(id, theStateMachineEditor.ConfigData);

            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                theStateMachineEditor.VisualStateNodes[i].Update(m_State, theStateMachineEditor.TransMtx, theStateMachineEditor.ConfigData);
            }

            if (ArrowTemp != null)
                ArrowTemp.DrawArrow();

            // Debug.Log("m_MyCount = " + m_MyCount);
        }

        void DrawBackGroundWindow(int id, artConfigurationData configData)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                if (theStateMachineEditor.VisualStateNodes[i].IsWithinUsingPanZoomTransform(Event.current.mousePosition, theStateMachineEditor.TransMtx))
                    return;
            }

            artDisplayWindowsBaseState dState = (artDisplayWindowsBaseState)m_State;
            
            if (Event.current.button == 1 && Event.current.isMouse && dState.DrawMenuBool)
            {
                if (Event.current.type == EventType.MouseDown/* && m_State is artDisplayStates*/)
                {
                    GenericMenu menu = new GenericMenu();

                    DrawConditionalMenus(menu, Event.current.mousePosition);
                    
                    for (int i = 0; i < theStateMachineEditor.ConfigData.StateCopyInfo.Length; i++)
                    {
                        menu.AddItem(new GUIContent("Add State/" + theStateMachineEditor.ConfigData.StateCopyInfo[i].m_MenuString), false, AddStateClassCallback, i);
                    }
                    //menu.AddItem(new GUIContent("Add State/Game State"), false, AddStateClassCallback, this);
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Create StateMachine"), false, CreateStateMachineCallback, null);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
            }
        }

        void DrawConditionalMenus(GenericMenu menu,Vector2 mousePos)
        {
            ArtimechEditor theScript = (ArtimechEditor)m_State.m_UnityObject;
            for (int i = 0; i < theScript.VisualStateNodes.Count; i++)
            {
                string condName = theScript.VisualStateNodes[i].GetConditionalByPosition(theScript.TransMtx.UnTransform(mousePos), 10);
                if (condName != null)
                {
                    menu.AddItem(new GUIContent("Edit Conditional"),
                    false,
                    CreateEditConditionalCallback,
                    condName);
                }
                if (condName != null)
                {
                    menu.AddItem(new GUIContent("Delete Conditional"),
                    false,
                    DeleteEditConditionalCallback,
                    condName);
                }
            }
            menu.AddSeparator("");
        }

        public void AddStateClassCallback(object obj)
        {
            //Debug.Log("Add State");
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
            theStateMachineEditor.CreateStateBool = true;
            theStateMachineEditor.CreateStateCopyDir = theStateMachineEditor.ConfigData.StateCopyInfo[(int)obj].m_MenuString;
            theStateMachineEditor.StateNameClassReplace = theStateMachineEditor.ConfigData.StateCopyInfo[(int)obj].m_ReplaceClassString;
        }

        public void CreateStateMachineCallback(object obj)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
            theStateMachineEditor.CreateStateMachineBool = true;
        }

        public void CreateEditConditionalCallback(object obj)
        {
            string className = (string)obj;
            string fileAndPathName = utlDataAndFile.FindPathAndFileByClassName(className);
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileAndPathName, 1, 1);
        }

        public void DeleteEditConditionalCallback(object obj)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)m_State.m_UnityObject;
            string className = (string)obj;
            string fileAndPathName = utlDataAndFile.FindPathAndFileByClassName(className);
            theStateMachineEditor.DeleteConditionalBool = true;
            theStateMachineEditor.DeleteConditionalPath = fileAndPathName;
            theStateMachineEditor.DeleteConditionalClass = className;
        }
    }
    #endregion
}

#endif