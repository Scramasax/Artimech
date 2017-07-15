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
    public class stateEditor : EditorWindow
    {
        //static IList<stateWindowsNode> m_StateList = new List<stateWindowsNode>();
        public GameObject m_GameObject = null;
        GameObject m_WasGameObject = null;
        bool m_AddStateMachine = false;
        string m_StateMachineName = "";
        Vector2 m_MousePos;

        stateWindowsNode m_AddStateWindow = null;

        stateEditor()
        {
            //m_StateList = new List<stateWindowsNode>();
        }

        [MenuItem("Window/ArtiMech/State Editor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<stateEditor>();
        }

        /// <summary>
        /// Editor Update.
        /// </summary>
        void Update()
        {
            if (m_GameObject == null)
            {
                if (m_WasGameObject != m_GameObject)
                    stateEditorUtils.StateList.Clear();
                //sets the 'was' gameobject so as to dectect a gameobject swap.
                m_WasGameObject = m_GameObject;
                return;
            }

            //A gameobject has been selected and the editor will try to find a statemachine.
            //If a statemachine is found then populated the visual representations populated
            //in the aforementioned.
            if (m_GameObject!=m_WasGameObject)
            {
                stateEditorUtils.StateList.Clear();

                stateMachineBase machine = null;
                machine = m_GameObject.GetComponent<stateMachineBase> ();

                //load states and their metadata
                if(machine!=null)
                {
                    Debug.Log("<color=green>" + "<b>" + "machine type is = " + "</b></color>" + "<color=grey>" + machine.GetType().Name + "</color>" + " .");

                    string strBuff = utlDataAndFile.FindPathAndFileByClassName(machine.GetType().Name,true);
                    stateEditorUtils.CreateStateWindows(strBuff);
                }
            }

            //sets the 'was' gameobject so as to dectect a gameobject swap.
            m_WasGameObject = m_GameObject;

            //Once the statemachine is created and unity has refreshed itself the statemachine is 
            //added to the currently selected gameobject.
            if (m_AddStateMachine && System.Type.GetType("artiMech." + m_StateMachineName) != null)
            {
                //makes the editor re pop the state windows.
                m_WasGameObject = null;

                m_GameObject.AddComponent(System.Type.GetType("artiMech." + m_StateMachineName));
                m_AddStateMachine = false;
                Debug.Log(
                            "<b><color=navy>Artimech Report Log Section B\n</color></b>"
                            + "<i><color=grey>Click to view details</color></i>"
                            + "\n"
                            + "<color=blue>Added a statemachine </color><b>"
                            + m_StateMachineName
                            + "</b>"
                            + "<color=blue> to a gameobject named </color>"
                            + m_GameObject.name
                            + " .\n\n");

            }

        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawToolBar();
            GUILayout.EndHorizontal();

            Event e = Event.current;

            //check mouse position
            m_MousePos = e.mousePosition;

            if (e.button == 1)
            {
                if (e.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add State"), false, ContextCallback, "addState");
                    menu.ShowAsContext();
                    e.Use();
                }
            }

            // input
            Event ev = Event.current;
            //Debug.Log(ev.mousePosition);
            if (ev.type == EventType.MouseDown  || ev.type==EventType.MouseDrag)
            {
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                {
                    float x = stateEditorUtils.StateList[i].WinRect.x;
                    float y = stateEditorUtils.StateList[i].WinRect.y;
                    float width = stateEditorUtils.StateList[i].WinRect.width;
                    float height = stateEditorUtils.StateList[i].WinRect.height;
                    if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                    {
                        if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
                        {
                            stateEditorUtils.StateList[i].SetPos(ev.mousePosition.x - (width * 0.5f), ev.mousePosition.y - (height * 0.5f));
                        }
                    }

                }
            }

                // render populated state windows
            BeginWindows();
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                //GUI.Window(i, stateEditorUtils.StateList[i].WinRect, DrawNodeWindow, stateEditorUtils.StateList[i].WindowTitle);
                stateEditorUtils.StateList[i].Update();
                if (m_AddStateWindow != null)
                    m_AddStateWindow.Update();
            }
            EndWindows();
        }

        void OnFocus()
        {
            Debug.Log("<color=blue>" + "<b>" + "focus " + "</b></color>" + "<color=grey>" + "" + "</color>");
            if (m_GameObject!=null && stateEditorUtils.StateList.Count==0)
                m_WasGameObject = null;
        }

        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
        }

        void ContextCallback(object obj)
        {
            //make the passed object to a string
            string clb = obj.ToString();
            //string stateName = "";

            if (clb.Equals("addState") && m_GameObject!=null)
            {
                string stateName = "aMech" + m_GameObject.name + "State" + stateEditorUtils.GetCode(stateEditorUtils.StateList.Count);
                if (stateEditorUtils.CreateAndAddStateCodeToProject(m_GameObject,stateName))
                {
                    stateWindowsNode windowNode = new stateWindowsNode(stateEditorUtils.StateList.Count);
                    windowNode.Set(stateName, m_MousePos.x, m_MousePos.y, 150, 80);
                    stateEditorUtils.StateList.Add(windowNode);

                    string fileAndPath = "";
                    fileAndPath = utlDataAndFile.FindPathAndFileByClassName(stateName);
                    stateEditorUtils.SetPositionAndSizeOfAStateFile(fileAndPath, (int)m_MousePos.x, (int)m_MousePos.y, 150, 80);

                    fileAndPath = utlDataAndFile.FindPathAndFileByClassName(m_StateMachineName);

                    stateEditorUtils.AddStateCodeToStateMachineCode(fileAndPath,stateName);

                    AssetDatabase.Refresh();
                }                
            }
        }

        void DrawToolBar()
        {

            GUILayout.FlexibleSpace();

            m_GameObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 1, position.width - 50, 16), "Game Object", m_GameObject, typeof(GameObject));


            /*if (obj)
                if (GUI.Button(new Rect(3, 25, position.width - 20, 16), "Check Dependencies"))
                    Selection.objects = EditorUtility.CollectDependencies(new GameObject[] { obj });*/

            if (GUILayout.Button("File", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                toolsMenu.AddItem(new GUIContent("Create"), false, OnCreateStateMachine);
               // toolsMenu.AddSeparator("");
               // toolsMenu.AddItem(new GUIContent("Save"), false, OnCreateState);
               // toolsMenu.AddItem(new GUIContent("Load"), false, OnCreateState);
                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, PrintAboutToConsole);
                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

        }

        void OnCreateStateMachine()
        {
            CreateStateMachineScriptAndLink();
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

        //paths and filenames
        public string FileName = "Assets/Scripts/Common/statemachine/stateMachineTemplate.cs";
        const string FileNameStartState = "Assets/Scripts/Common/statemachine/stateTemplate.cs";
        public string pathName = "Assets/Scripts/artiMechStates/";

        /// <summary>
        /// Artimech's statemachine and startState generation system.
        /// </summary>
        void CreateStateMachineScriptAndLink()
        {

            string pathAndFileName = pathName
                                                        + "aMech"
                                                        + m_GameObject.name
                                                        + "/"
                                                        + "aMech"
                                                        + m_GameObject.name
                                                        + ".cs";

            string pathAndFileNameStartState = pathName
                                            + "aMech"
                                            + m_GameObject.name
                                            + "/"
                                            + "aMech"
                                            + m_GameObject.name
                                            + "StartState"
                                            + ".cs";

            if (File.Exists(pathAndFileName))
            {
                Debug.Log("<color=red>stateEditor.CreateStateMachine = </color> <color=blue> " + pathAndFileName + "</color> <color=red>Already exists and can't be overridden...</color>");
                return;
            }

            //clear the visual list if there are any in the editor
            ClearStatesAndRefresh();

            //create the aMech directory 
            string replaceName = "aMech";
            string directoryName = pathName + replaceName + m_GameObject.name;
            Directory.CreateDirectory(directoryName);

            //creates a start state from a template and populate aMech directory
            string stateStartName = "";
            stateStartName = stateEditorUtils.ReadReplaceAndWrite(
                                                        FileNameStartState, 
                                                        m_GameObject.name + "StartState", 
                                                        pathName, 
                                                        pathAndFileNameStartState, 
                                                        "stateTemplate", 
                                                        "aMech");

            //creates the statemachine from a template
            string stateMachName = "";
            stateMachName = stateEditorUtils.ReadReplaceAndWrite(
                                                        FileName, 
                                                        m_GameObject.name, 
                                                        pathName, 
                                                        pathAndFileName, 
                                                        "stateMachineTemplate", 
                                                        replaceName);

            //replace the startStartStateTemplate
            utlDataAndFile.ReplaceTextInFile(pathAndFileName, "stateTemplate", stateStartName);

            Debug.Log(
                        "<b><color=navy>Artimech Report Log Section A\n</color></b>"
                        + "<i><color=grey>Click to view details</color></i>"
                        + "\n"
                        + "<color=blue>Finished creating a state machine named </color><b>"
                        + stateMachName
                        + "</b>:\n"
                        + "<color=blue>Created and added a start state named </color>"
                        + stateStartName
                        + "<color=blue> to </color>"
                        + stateMachName
                        + "\n\n");

            AssetDatabase.Refresh();

            m_StateMachineName = stateMachName;
            m_AddStateMachine = true;

            utlDataAndFile.FindPathAndFileByClassName(m_StateMachineName, true);
        }

/*
        void OnTools_Help()
        {
            Help.BrowseURL("http://example.com/product/help");
        }*/
    }
}
#endif