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
    /// 
    public class artMainWindowConfigure : artWindowBase
    {
        #region Variables
        artBaseOkCancel m_State;
        Vector2 m_MousePos;
        Vector2 m_ScrollPos;
        string m_StateMachinePathAndFile;
        Color m_MatColorA;
        Color m_MatColorB;
        Color m_MatColorC;
        Color m_MatColorD;
        Color m_MatColorE;
        Color m_MatColorF;
        Color m_MatColorG;
        Color m_MatColorH;
        Color m_MatColorI;

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
            TextStyle.normal.textColor = Color.black;
            TextStyle.fontSize = 12;

            var TitleStyle = new GUIStyle();
            TitleStyle.normal.textColor = Color.blue;
            TitleStyle.fontSize = 15;

            var SmallTitleStyle = new GUIStyle();
            SmallTitleStyle.normal.textColor = new Color(0.2f, 0.6f, 0.2f, 1);
            SmallTitleStyle.fontSize = 9;



            m_MousePos = Event.current.mousePosition;
            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 0, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("Configure Artimech", TitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 10), GUILayout.Height(Screen.height - 110));

            int startMargin = 5;
            int endMargin = 25;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("State Creation", SmallTitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();



            GUILayout.Space(startMargin);

            m_StateMachinePathAndFile = EditorGUILayout.TextField("Path/File Statemachine", m_StateMachinePathAndFile);
            GUI.Label(new Rect(1, 1, WinRect.width, 20), new GUIContent("", "This is the path and file name for the statemachine to be copied."));
            GUILayout.Space(10);
            if (GUI.Button(new Rect(WinRect.width - (endMargin * 1.5f), 13, 25, 15), "..."))
            {
                m_StateMachinePathAndFile = EditorUtility.OpenFilePanel("File", "", "cs");
            }
            GUILayout.Space(endMargin - 10);
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

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("Main Display Configuration", SmallTitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorA = EditorGUILayout.ColorField("Main Display Color", m_MatColorA);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorB = EditorGUILayout.ColorField("Graph Paper Line Color", m_MatColorB);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("State Display Configuration", SmallTitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorC = EditorGUILayout.ColorField("State Outline Color", m_MatColorC);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorD = EditorGUILayout.ColorField("State Header Color", m_MatColorD);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorE = EditorGUILayout.ColorField("State Body Color", m_MatColorE);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorF = EditorGUILayout.ColorField("State Close Color", m_MatColorF);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(0);
            GUILayout.Label("Conditionial Display Configuration", SmallTitleStyle);
            GUILayout.Space(100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorG = EditorGUILayout.ColorField("Arrow Line Color", m_MatColorG);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorH = EditorGUILayout.ColorField("Arrow Body Color", m_MatColorH);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(startMargin);
            m_MatColorI = EditorGUILayout.ColorField("Some other Color", m_MatColorI);
            GUILayout.Space(endMargin);
            EditorGUILayout.EndHorizontal();



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