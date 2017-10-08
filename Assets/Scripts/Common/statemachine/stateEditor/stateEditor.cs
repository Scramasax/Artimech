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
            stateEditorUtils.StateEditor = this;
        }


        /// <summary>
        /// Editor Update.
        /// </summary>
        new void Update()
        {
            stateEditorUtils.EditorCurrentGameObject = stateEditorUtils.GameObject;

            base.Update();

            //sets the 'was' gameobject so as to dectect a gameobject swap.
            stateEditorUtils.WasGameObject = stateEditorUtils.GameObject;

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

                if (m_CurrentState.m_StateName == "Display Windows" && stateEditorUtils.GameObject != null)
                    toolsMenu.AddItem(new GUIContent("Save"), false, OnSaveMetaData);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Save"));

                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, PrintAboutToConsole);
                toolsMenu.AddItem(new GUIContent("Wiki"), false, OnWiki);
                
                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

        }

        void OnSaveMetaData()
        {
            for(int i=0;i<stateEditorUtils.StateList.Count;i++)
            {
                stateEditorUtils.StateList[i].SaveMetaData();
            }
        }

        void OnCreateStateMachine()
        {
            if(m_CurrentState is editorWaitState)
            {
                editorWaitState waitState = m_CurrentState as editorWaitState;
                waitState.CreateBool = true;
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

        void OnWiki()
        {
            Help.BrowseURL("http://example.com/product/help");
        }

        public void EditorRepaint()
        {
            Repaint();
        }
    }
}
#endif