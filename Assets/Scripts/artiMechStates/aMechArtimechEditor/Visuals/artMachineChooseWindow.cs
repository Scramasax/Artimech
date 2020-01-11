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
    public class artMachineChooseWindow : artWindowBase
    {
        #region Variables
        iMachineBase[] m_Machines;
        bool[] m_MachineSelectionBools;
        artChooseMachine m_State;
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
        public artMachineChooseWindow(artChooseMachine state, iMachineBase[] machines, string title, Rect rect, Color color, int id) : base(title, rect, color, id)
        {
            m_State = state;
            m_Machines = machines;
            m_MachineSelectionBools = new bool[m_Machines.Length];
        }

        /// <summary>
        /// Update
        /// </summary>
        new public void Update()
        {
            m_WinRect.x = Screen.width * 0.5f;
            m_WinRect.width = Screen.width * 0.5f;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = (m_MachineSelectionBools.Length * 25) + 85;
            m_WinRect.x = (Screen.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (Screen.height * 0.5f) - (m_WinRect.height * 0.5f);
            GUI.Window(m_Id, WinRect, Draw, m_Title);
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
            // Color backroundColor = new Color(1, 1, 1, 0.8f);
            Rect rect = new Rect(0, 16, WinRect.width, WinRect.height);
            EditorGUI.DrawRect(rect, m_WindowColor);

            //stateEditorDrawUtils.DrawGridBackground();

            //bool[] arrayOfBools = new bool[m_Machines.Length];

            // GUILayout.BeginHorizontal("");
            for (int i = 0; i < m_Machines.Length; i++)
            {
                //UnityEngine.Object obj = (UnityEngine.Object) m_Machines[i];
                //arrayOfBools[i] = EditorGUILayout.Toggle(obj.name, arrayOfBools[i]);
                Type type = m_Machines[i].GetType().UnderlyingSystemType;
                String className = type.Name;

                m_MachineSelectionBools[i] = EditorGUILayout.Toggle(className, m_MachineSelectionBools[i]);
                if (m_MachineSelectionBools[i])
                    ToggleBools(i);

            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal("");
            if (IsAMachineSelected())
            {
                if (GUILayout.Button("Ok"))
                {
                    m_State.OkBool = true;
                    SetMachineScript();
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
            GUILayout.Space(-10);
            GUILayout.BeginHorizontal("");

            EditorGUI.BeginDisabledGroup(IsAMachineSelected());
            if (GUILayout.Button("Create New State Machine"))
            {
                m_State.CreateBool = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            //GUILayout.EndHorizontal();

            //         GUI.DrawTexture(new Rect(m_TexturePosAndSize.x, m_TexturePosAndSize.y, m_TexturePosAndSize.z, m_TexturePosAndSize.w), m_ExclamtionTexture);

            //GUI.DragWindow();
        }

        void ToggleBools(int index)
        {
            for (int i = 0; i < m_MachineSelectionBools.Length; i++)
            {
                m_MachineSelectionBools[i] = false;
            }
            m_MachineSelectionBools[index] = true;
        }

        bool IsAMachineSelected()
        {

            for (int i = 0; i < m_MachineSelectionBools.Length; i++)
            {
                if (m_MachineSelectionBools[i] == true)
                    return true;
            }
            return false;
        }

        void SetMachineScript()
        {
            ArtimechEditor theMachineScript = (ArtimechEditor)m_State.m_UnityObject;
            for (int i = 0; i < m_MachineSelectionBools.Length; i++)
            {
                if (m_MachineSelectionBools[i] == true)
                {
                    theMachineScript.MachineScript = m_Machines[i];
                    return;
                }
            }
        }
        #endregion
    }

}
#endif