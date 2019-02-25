/// Artimech
/// 
/// Copyright � <2017-2019> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.
/// 

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Artimech
{
    public class ArtimechEditor : artimechEditorBase
    {
        #region Vars
        static ArtimechEditor m_Instance = null;
        UnityEngine.Object m_SelectedObj;
        UnityEngine.Object m_WasSelectedObj;
        utlMatrix34 m_TransMtx;
        iMachineBase m_MachineScript;
        bool m_DrawToolBarBool = true;
        IList<artVisualStateNode> m_VisualStateNodes;


        #endregion
        #region Accessors
        /// <summary>Used class matching via parsing for namespace.</summary>
        public static string kArtimechNamespace = "Artimech.";

        /// <summary> SelectedObj returns and sets the object selected in the main menu bar. </summary>
        public Object SelectedObj { get { return m_SelectedObj; } set { m_SelectedObj = value; } }

        /// <summary> WasSelectedObj returns and sets the object that was selected in the menu bar last frame.</summary>
        public Object WasSelectedObj { get { return m_WasSelectedObj; } set { m_WasSelectedObj = value; } }

        /// <summary>Returns an instance of the ArtimechEditor </summary>
        public static ArtimechEditor Inst { get { return m_Instance; } }

        /// <summary> Translation matrix</summary>
        public utlMatrix34 TransMtx { get { return m_TransMtx; } set { m_TransMtx = value; } }

        /// <summary>Active Machine Script</summary>
        public iMachineBase MachineScript { get { return m_MachineScript; } set { m_MachineScript = value; } }

        /// <summary>Turns on and off the toolbar.</summary>
        public bool DrawToolBarBool { get { return m_DrawToolBarBool; } set { m_DrawToolBarBool = value; } }

        /// <summary>A list of visual state nodes.</summary>
        public IList<artVisualStateNode> VisualStateNodes { get { return m_VisualStateNodes; } set { m_VisualStateNodes = value; } }


        #endregion
        #region Member Functions
        public ArtimechEditor() : base()
        {
            if (m_Instance == null)
                m_Instance = this;


        }

        [MenuItem("Window/Artimech/ArtimechEditor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<ArtimechEditor>();
        }

        new void OnEnable()
        {
            if (!m_Instance)
            {
                m_Instance = (ArtimechEditor)EditorWindow.GetWindow(typeof(ArtimechEditor), true, "Artimech");
                m_Instance.Show();
            }
            base.OnEnable();
            //m_MainWindow = new artMainWindow("test", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);
            VisualStateNodes = new List<artVisualStateNode>();
            TransMtx = new utlMatrix34();
            CreateStates();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            ArtimechEditor.Inst.WasSelectedObj = ArtimechEditor.Inst.SelectedObj;
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        void DrawToolBar()
        {
            EditorGUI.BeginDisabledGroup(!DrawToolBarBool);
            GUILayout.FlexibleSpace();



#pragma warning disable CS0618 // Type or member is obsolete
            SelectedObj = (Object)EditorGUI.ObjectField(new Rect(3, 1, position.width - 150, 16), "Selected Object", SelectedObj, typeof(Object));
#pragma warning restore CS0618 // Type or member is obsolete

            if (GUILayout.Button("Menu", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, PrintAboutToConsole);
                toolsMenu.AddItem(new GUIContent("Wiki"), false, OnWiki);

                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

            EditorGUI.EndDisabledGroup();
        }

        new void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            //if (DrawToolBarBool)
            DrawToolBar();
            GUILayout.EndHorizontal();

            // render populated state windows
            BeginWindows();
            m_CurrentState.UpdateEditorGUI();
            EndWindows();

            base.OnGUI();
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new artStart(this), "artStart");

            //<ArtiMechStates>
            AddState(new artMoveMouseUp(this),"artMoveMouseUp");
            AddState(new artMoveMouseDown(this),"artMoveMouseDown");
            AddState(new artCreateStateMachine(this), "artCreateStateMachine");
            AddState(new artClearObject(this), "artClearObject");
            AddState(new artChooseMachine(this), "artChooseMachine");
            AddState(new artLoadStates(this), "artLoadStates");
            AddState(new artNotEditorOrGameObject(this), "artNotEditorOrGameObject");
            AddState(new artDisplayStates(this), "artDisplayStates");
            AddState(new artChooseStateMachineName(this), "artChooseStateMachineName");
            AddState(new artAskToCreate(this), "artAskToCreate");
            AddState(new artCheckIfIMachine(this), "artCheckIfIMachine");
            AddState(new artNoObject(this), "artNoObject");

        }

        void PrintAboutToConsole()
        {
            Debug.Log(
            "<b><color=navy>Artimech (c) 2017-2019 by George A Lancaster \n</color></b>"
            + "<i><color=grey>Click to view details</color></i>"
            + "\n"
            + "<color=blue>An Opensource Visual State Editor\n</color><b>"
            + "</b>"
            + "<color=teal>developed in Unity 5.x-2019</color>"
            + "<color=black>\nEmail: </color>"
            + "<color=green>geolan1024@gmail.com</color>"
            + " .\n\n");
        }

        void OnWiki()
        {
            Help.BrowseURL("https://github.com/Scramasax/Artimech/wiki");
        }

        /*        new public void Repaint()
                {
                    Repaint();
                }*/
    }
    #endregion
}
