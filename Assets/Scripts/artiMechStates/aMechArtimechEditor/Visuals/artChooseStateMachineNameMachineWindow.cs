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
    public class artChooseStateMachineNameMachineWindow : artWindowBase
    {
        #region Variables
        artChooseStateMachineName m_State;
        #endregion
        #region Gets Sets
        #endregion
        #region Member Functions


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="id"></param>
        public artChooseStateMachineNameMachineWindow(artChooseStateMachineName state, string title, Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            m_State = state;
            // m_Machines = machines;
            // m_MachineSelectionBools = new bool[m_Machines.Length];
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            m_WinRect.x = Screen.width * 0.5f;
            m_WinRect.width = Screen.width * 0.65f;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = 125;
            m_WinRect.x = (Screen.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (Screen.height * 0.5f) - (m_WinRect.height * 0.5f);
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
            var TextStyle = new GUIStyle();
            TextStyle.normal.textColor = Color.blue;
            TextStyle.fontSize = 12;


            //Color backroundColor = new Color(1, 1, 1, 0.8f);

            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 16, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            //stateEditorDrawUtils.DrawGridBackground();

            //bool[] arrayOfBools = new bool[m_Machines.Length];

            GUILayout.Space(10);

            // m_State.CreationType = (artChooseStateMachineName.eCreationType)EditorGUILayout.EnumPopup("State To Create:", m_State.CreationType);

            GUILayout.BeginHorizontal("");
            GUILayout.Space(10);
            GUILayout.Label("Choose a name for the state machine script.", TextStyle);
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            m_State.StateMachineName = EditorGUILayout.TextField("Machine Name", m_State.StateMachineName);
            GUILayout.Space(10);


            GUILayout.BeginHorizontal("");

            if (m_State.StateMachineName.Length > 3)
            {
                if (GUILayout.Button("Ok"))
                {
                    m_State.OkBool = true;
                    //                SetMachineScript();
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Button("Ok");
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.Space(35);
            if (GUILayout.Button("Cancel"))
            {
                m_State.CancelBool = true;
            }
            GUILayout.EndHorizontal();

            //GUILayout.EndHorizontal();

            //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }

        #endregion
    }

}
#endif