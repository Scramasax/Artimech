using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    /// <summary>
    /// Base window for the Artimech state editor system
    /// </summary>
    public class artMainWindowConfigure : artWindowBase
    {
        #region Variables
        artBaseOkCancel m_State;
        Vector2 m_MousePos;
        Vector2 m_ScrollPos;

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
        #endregion
        #region Member Functions


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artMainWindowConfigure(artBaseOkCancel state, string title, Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            m_State = state;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            m_WinRect.width = Screen.width;
            m_WinRect.height = Screen.height;
            GUI.Window(m_Id, WinRect, Draw, m_Title);
        }

        /// <summary>
        /// Check to see if a position is inside the main window.
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        public bool IsWithin(Vector2 vect)
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

        public void Draw(int id)
        {
            m_TitleSize = 1;
            m_MainWindowStart = 1;

            var TextStyle = new GUIStyle();
            TextStyle.normal.textColor = Color.blue;
            TextStyle.fontSize = 12;

            var TitleStyle = new GUIStyle();
            TitleStyle.normal.textColor = Color.blue;
            TitleStyle.fontSize = 15;

            m_MousePos = Event.current.mousePosition;
            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 0, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("Configure Window", TitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width-10), GUILayout.Height(Screen.height-100));
            //GUILayout.Space(-10);

            for (int i = 0; i < 100; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(5);
                ArtimechEditor.Inst.RefactorName = EditorGUILayout.TextField("Refactor Name", ArtimechEditor.Inst.RefactorName);
                GUILayout.Space(25);
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                m_State.OkBool = true;
            }
            if (GUILayout.Button("Cancel"))
            {
                m_State.CancelBool = true;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    #endregion
}

#endif