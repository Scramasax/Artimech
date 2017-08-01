using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System;

#if UNITY_EDITOR

namespace artiMech
{
    public class stateEditor : stateEditorBase
    {
        //static IList<stateWindowsNode> m_StateList = new List<stateWindowsNode>();
        //public GameObject m_GameObject = null;
        //GameObject m_WasGameObject = null;
        bool m_AddStateMachine = false;
        //string m_StateMachineName = "";
        //Vector2 m_MousePos;

        //stateWindowsNode m_AddStateWindow = null;

        public stateEditor() : base()
        {
            //m_StateList = new List<stateWindowsNode>();
        }
/*
        [MenuItem("Window/ArtiMech/State Editor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<stateEditor>();
        }*/

        /// <summary>
        /// Editor Update.
        /// </summary>
        new void Update()
        {
            stateEditorUtils.EditorCurrentGameObject = stateEditorUtils.GameObject;

            base.Update();



            //A gameobject has been selected and the editor will try to find a statemachine.
            //If a statemachine is found then populated the visual representations populated
            //in the aforementioned.

            /*
            if (stateEditorUtils.GameObject != stateEditorUtils.WasGameObject)
            {
                stateEditorUtils.StateList.Clear();

                stateMachineBase machine = null;
                machine = stateEditorUtils.GameObject.GetComponent<stateMachineBase>();

                //load states and their metadata
                if (machine != null)
                {
                    //Debug.Log("<color=green>" + "<b>" + "machine type is = " + "</b></color>" + "<color=grey>" + machine.GetType().Name + "</color>" + " .");

                    string strBuff = utlDataAndFile.FindPathAndFileByClassName(machine.GetType().Name, false);
                    stateEditorUtils.CreateStateWindows(strBuff);
                }
            }
            */

            //sets the 'was' gameobject so as to dectect a gameobject swap.
            stateEditorUtils.WasGameObject = stateEditorUtils.GameObject;


            /*
            //Once the statemachine is created and unity has refreshed itself the statemachine is 
            //added to the currently selected gameobject.
            if (m_AddStateMachine && System.Type.GetType("artiMech." + stateEditorUtils.StateMachineName) != null)
            {
                //makes the editor re pop the state windows.
                stateEditorUtils.WasGameObject = null;

                stateEditorUtils.GameObject.AddComponent(System.Type.GetType("artiMech." + stateEditorUtils.StateMachineName));
                m_AddStateMachine = false;
                Debug.Log(
                            "<b><color=navy>Artimech Report Log Section B\n</color></b>"
                            + "<i><color=grey>Click to view details</color></i>"
                            + "\n"
                            + "<color=blue>Added a statemachine </color><b>"
                            + stateEditorUtils.StateMachineName
                            + "</b>"
                            + "<color=blue> to a gameobject named </color>"
                            + stateEditorUtils.GameObject.name
                            + " .\n\n");

            }
            */

        }

        new void OnGUI()
        {

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawToolBar();
            GUILayout.EndHorizontal();

            // render populated state windows
            BeginWindows();
            m_CurrentState.UpdateEditorGUI();
            EndWindows();
        }

        void OnFocus()
        {
            //Debug.Log("<color=blue>" + "<b>" + "focus " + "</b></color>" + "<color=grey>" + "" + "</color>");
            if (stateEditorUtils.GameObject != null && stateEditorUtils.StateList.Count==0)
                stateEditorUtils.WasGameObject = null;
        }

        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
        }

        void DrawToolBar()
        {

            GUILayout.FlexibleSpace();

            stateEditorUtils.GameObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 1, position.width - 50, 16), "Game Object", stateEditorUtils.GameObject, typeof(GameObject));

            if (GUILayout.Button("File", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                //In the wait state and an object is selected.
                if (m_CurrentState.m_StateName == "Wait" && stateEditorUtils.GameObject != null)
                    toolsMenu.AddItem(new GUIContent("Create"), false, OnCreateStateMachine);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Create"));
                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, PrintAboutToConsole);
                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

        }

        void OnCreateStateMachine()
        {
            if(m_CurrentState is editorWaitState)
            {
                editorWaitState waitState = m_CurrentState as editorWaitState;
                //stateEditorUtils.CreateStateMachineScriptAndLink();
            }
        }

        void PrintAboutToConsole()
        {
            Debug.Log(
            "<b><color=navy>Artimech (c) 2017 by George A Lancaster \n</color></b>"
            + "<i><color=grey>Click to view details</color></i>"
            + "\n"
            + "<color=blue>An Opensource Visual State Editor\n</color><b>"
            + "</b>"
            + "<color=teal>developed in Unity 5.x</color>"
            + " .\n\n");
        }


        /// <summary>
        /// Removes the visual components of the state machine.
        /// </summary>
        void ClearStatesAndRefresh()
        {
            //m_StateList.Clear();
        }

        /*
                void OnTools_Help()
                {
                    Help.BrowseURL("http://example.com/product/help");
                }*/
    }
}
#endif