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
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 10), GUILayout.Height(Screen.height - 110));

            int startMargin = 5;
            int endMargin = 25;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            ArtimechEditor.Inst.RefactorName = EditorGUILayout.TextField("Path/File Statemachine", ArtimechEditor.Inst.RefactorName);
            GUI.Label(new Rect(1, 1, WinRect.width, 20), new GUIContent("", "This is the path and file name for the statemachine to be copied."));
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            ArtimechEditor.Inst.RefactorName = EditorGUILayout.TextField("Created Script Directory", ArtimechEditor.Inst.RefactorName);
            GUI.Label(new Rect(1, 20, WinRect.width, 20), new GUIContent("", "This is the root directory the statemachines will be created at."));
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            ArtimechEditor.Inst.RefactorName = EditorGUILayout.TextField("Script Name Prefix", ArtimechEditor.Inst.RefactorName);
            GUI.Label(new Rect(1, 40, WinRect.width, 20), new GUIContent("", "This is the prefix to use when creating directories and the statemachine name.  The prefix is added to the name of the Unity3d object."));
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            // GUI.Button(new Rect(10, 10, 100, 20), new GUIContent("Click me", "This is the tooltip"));

            // Display the tooltip from the element that has mouseover or keyboard focus
            //GUI.Label(new Rect(1, 1, 1, 1), GUI.tooltip);


            //GUILayout.Space(-10);

            /*       for (int i = 0; i < 100; i++)
                   {
                       EditorGUILayout.BeginHorizontal();
                       GUILayout.Space(5);
                       ArtimechEditor.Inst.RefactorName = EditorGUILayout.TextField("Refactor Name", ArtimechEditor.Inst.RefactorName);
                       GUILayout.Space(25);
                       EditorGUILayout.EndHorizontal();
                   }*/


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            if (GUILayout.Button("Save"))
            {
                m_State.OkBool = true;
            }
            GUILayout.Space(15);
            if (GUILayout.Button("Cancel"))
            {
                m_State.CancelBool = true;
            }
            GUILayout.Space(15);
            EditorGUILayout.EndHorizontal();
        }
    }
    #endregion
}

#endif