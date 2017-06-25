using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System;

namespace artiMech
{
    public class stateEditor : EditorWindow
    {
        IList<stateNode> m_StateList;
        public GameObject m_GameObject = null;
        bool m_AddStateMachine = false;
        string m_StateMachineName = "";
        Vector2 m_MousePos;

        stateEditor()
        {
            m_StateList = new List<stateNode>();
        }

        [MenuItem("Window/ArtiMech/State Editor")]
        static void ShowEditor()
        {
            //stateEditor editor = EditorWindow.GetWindow<stateEditor>();
            //editor.stopWatch.Start();

            EditorWindow.GetWindow<stateEditor>();

        }

        void Update()
        {
            if (m_AddStateMachine && System.Type.GetType("artiMech." + m_StateMachineName) != null)
            {
                m_GameObject.AddComponent(System.Type.GetType("artiMech." + m_StateMachineName));
                m_AddStateMachine = false;
                Debug.Log("added a statemachine to an object....");

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
        }

        void ContextCallback(object obj)
        {
            //make the passed object to a string
            string clb = obj.ToString();
            string stateName = "";

            if (clb.Equals("addState"))
            {

                //InputNode inputNode = new InputNode();
                //inputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
                //new Rect(m_MousePos.x, m_MousePos.y, 200, 150);
                stateName = GUI.TextField(new Rect(250, 93, 250, 25), stateName, 40);
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
                toolsMenu.AddItem(new GUIContent("About"), false, OnCreateState);
                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

        }

        void OnCreateStateMachine()
        {
            CreateStateMachineScriptAndLink();
        }

        void OnCreateState()
        {

        }

        public string FileName = "Assets/Scripts/Common/statemachine/stateMachineTemplate.cs";
        public string pathName = "Assets/Scripts/artiMechStates/";

        void CreateStateMachineScriptAndLink()
        {

            string pathAndFileName = pathName
                                                        + "aMech"
                                                        + m_GameObject.name
                                                        + "/"
                                                        + "aMech"
                                                        + m_GameObject.name
                                                        + ".cs";

            if (File.Exists(pathAndFileName))
            {
                Debug.Log("<color=red>stateEditor.CreateStateMachine = </color> <color=blue> " + pathAndFileName + "</color> <color=red>Already exists and can't be overridden...</color>");
                return;
            }

            string stateMachName = utlDataAndFile.ReadReplaceAndWrite(FileName,m_GameObject.name,pathName,pathAndFileName, "stateMachineTemplate", "aMech");

            Debug.Log("<color=green>Finished Creating StateMachine</color>.");

            AssetDatabase.Refresh();

            m_StateMachineName = stateMachName;
            m_AddStateMachine = true;



            /*
            string text = "";
            FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            fileStream.Close();

            string replaceName = "aMech" + m_GameObject.name;
            string modText = text.Replace("stateMachineTemplate", replaceName);

            string directoryName = pathName + "aMech" + m_GameObject.name;

            Directory.CreateDirectory(directoryName);

            StreamWriter writeStream = new StreamWriter(pathAndFileName);

            writeStream.Write(modText);

            writeStream.Close();*/


        }

        /*
        void DrawToolBar()
        {
            if (GUILayout.Button("Create...", EditorStyles.toolbarButton))
            {
                OnMenu_Create();
                EditorGUIUtility.ExitGUI();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Tools", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                if (Selection.activeGameObject != null)
                    toolsMenu.AddItem(new GUIContent("Optimize Selected"), false, OnTools_OptimizeSelected);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Optimize Selected"));
                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("Help..."), false, OnTools_Help);
                // Offset menu from right of editor window
                toolsMenu.DropDown(new Rect(Screen.width - 216 - 40, 0, 0, 16));
                EditorGUIUtility.ExitGUI();
            }
        }

        void OnMenu_Create()
        {
            // Do something!
        }

        void OnTools_OptimizeSelected()
        {
            // Do something!
        }

        void OnTools_Help()
        {
            Help.BrowseURL("http://example.com/product/help");
        }*/
    }
}
