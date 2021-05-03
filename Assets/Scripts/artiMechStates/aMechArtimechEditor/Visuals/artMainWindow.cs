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
        #region Variables
        baseState m_State;
        Vector2 m_MousePos;

        static int m_Count = 0;
        bool m_AlreadyDrawn = false;
        int m_MyCount = 0;

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

            if (Event.current.button == 1 && Event.current.isMouse)
            {
                if (Event.current.type == EventType.MouseDown/* && m_State is artDisplayStates*/)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add State/Game State"), false, AddStateClassCallback, this);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
            }
        }

        public void AddStateClassCallback(object obj)
        {
            Debug.Log("Add State");
        }
    }
    #endregion
}

#endif