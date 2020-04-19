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

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Artimech
{
    public class ArtimechEditor : artimechEditorBase
    {
        #region Vars
        //static ArtimechEditor m_Instance = null;
        UnityEngine.Object m_SelectedObj;
        UnityEngine.Object m_WasSelectedObj;
        utlMatrix34 m_TransMtx;
        iMachineBase m_MachineScript;
        bool m_DrawToolBarBool = true;
        IList<artVisualStateNode> m_VisualStateNodes;
        Vector2 m_MouseClickDownPosStart;
        string m_RefactorName = "";
        artConfigurationData m_ConfigData;
        Texture m_MenuArrowTexture;
        editorOnLoadInfo m_LoadingInfo = new editorOnLoadInfo();
        bool m_RepaintOnUpdate = false;

        #endregion
        #region Accessors
        /// <summary>Used class matching via parsing for namespace.</summary>
        public static string kArtimechNamespace = "Artimech.";

        /// <summary> SelectedObj returns and sets the object selected in the main menu bar. </summary>
        public Object SelectedObj { get { return m_SelectedObj; } set { m_SelectedObj = value; } }

        /// <summary> WasSelectedObj returns and sets the object that was selected in the menu bar last frame.</summary>
        public Object WasSelectedObj { get { return m_WasSelectedObj; } set { m_WasSelectedObj = value; } }

        /// <summary>Returns an instance of the ArtimechEditor </summary>
        //public static ArtimechEditor Inst { get { return m_Instance; } }

        /// <summary> Translation matrix</summary>
        public utlMatrix34 TransMtx { get { return m_TransMtx; } set { m_TransMtx = value; } }

        /// <summary>Active Machine Script</summary>
        public iMachineBase MachineScript { get { return m_MachineScript; } set { m_MachineScript = value; } }

        /// <summary>Turns on and off the toolbar.</summary>
        public bool DrawToolBarBool { get { return m_DrawToolBarBool; } set { m_DrawToolBarBool = value; } }

        /// <summary>A list of visual state nodes.</summary>
        public IList<artVisualStateNode> VisualStateNodes { get { return m_VisualStateNodes; } set { m_VisualStateNodes = value; } }


        public string RefactorName { get => m_RefactorName; set => m_RefactorName = value; }
        public Vector2 MouseClickDownPosStart { get => m_MouseClickDownPosStart; set => m_MouseClickDownPosStart = value; }
        public artConfigurationData ConfigData { get => m_ConfigData; set => m_ConfigData = value; }
        public editorOnLoadInfo LoadingInfo { get => m_LoadingInfo; set => m_LoadingInfo = value; }
        public bool RepaintOnUpdate { get => m_RepaintOnUpdate; set => m_RepaintOnUpdate = value; }

        #endregion
        #region Member Functions
        public ArtimechEditor() : base()
        {

        }

        [MenuItem("Window/Artimech/ArtimechEditor")]
        static void ShowEditor()
        {
            EditorWindow.GetWindow<ArtimechEditor>();
        }

        new void OnEnable()
        {
            this.Show();
            base.OnEnable();

            VisualStateNodes = new List<artVisualStateNode>();
            TransMtx = new utlMatrix34();

            m_MenuArrowTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Materials/menuArrow.png", typeof(Texture));
            //        m_MenuArrowTexture.width = 10;
            //        m_MenuArrowTexture.height = 10;

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
            WasSelectedObj = SelectedObj;
            if(RepaintOnUpdate)
            {
                RepaintOnUpdate = false;
                Repaint();
            }
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
                AddConfigureMenuEntries(toolsMenu);
                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, OnPrintAboutToConsole);
                toolsMenu.AddItem(new GUIContent("Wiki"), false, OnWiki);

                toolsMenu.DropDown(new Rect(position.width - 150 + 19, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

            EditorGUI.EndDisabledGroup();
        }

        void AddConfigureMenuEntries(GenericMenu toolsMenu)
        {
            //EditorGUI.BeginDisabledGroup(SelectedObj == null ? true : false);
          //  EditorGUI.BeginDisabledGroup(true);
            artConfigurationData[] data = utlDataAndFile.GetAllInstances<artConfigurationData>();
            for (int i = 0; i < data.Length; i++)
            {
                //new GenericMenu.MenuFunction2(this.ToggleLogStackTraces), current
                if (SelectedObj == null)
                    toolsMenu.AddDisabledItem(new GUIContent("Configure/" + data[i].name));
                else
                    toolsMenu.AddItem(new GUIContent("Configure/" + data[i].name, "Select a configuartion file to use."), data[i] == m_ConfigData ? true : false, new GenericMenu.MenuFunction2(this.OnConfigure), data[i]);
            }
           // EditorGUI.EndDisabledGroup();
        }

        public void SetConfigurationDataViaLoadInfo()
        {
            artConfigurationData[] data = utlDataAndFile.GetAllInstances<artConfigurationData>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].name == LoadingInfo.m_NameOfConfigData)
                {
                    m_ConfigData = data[i];
                    return;
                }
            }
        }

        new void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            //if (DrawToolBarBool)
            DrawToolBar();
            GUILayout.EndHorizontal();

            // render populated state windows
            BeginWindows();
            if (m_CurrentState != null)
                m_CurrentState.UpdateEditorGUI();
            EndWindows();

            base.OnGUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public artVisualStateNode GetResizeNode()
        {
            for (int i = 0; i < VisualStateNodes.Count; i++)
            {
                if (VisualStateNodes[i].Resize)
                {
                    return VisualStateNodes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public artVisualStateNode GetLeftButtonNode()
        {
            for (int i = 0; i < VisualStateNodes.Count; i++)
            {
                if (VisualStateNodes[i].LeftMouseButton)
                {
                    return VisualStateNodes[i];
                }
            }
            return null;
        }

        public void SaveMetaDataInStates()
        {
            for (int i = 0; i < VisualStateNodes.Count; i++)
            {
                VisualStateNodes[i].SaveMetaData();
            }
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new artStart(this), "artStart");

            //<ArtiMechStates>
            AddState(new artSetSingleMachine(this), "artSetSingleMachine");
            AddState(new artRefactorStateClass(this), "artRefactorStateClass");
            AddState(new artRefactorEnterData(this), "artRefactorEnterData");
            AddState(new artRefactScreen(this), "artRefactScreen");
            AddState(new artSaveScreen(this), "artSaveScreen");
            AddState(new artSaveMetaData(this), "artSaveMetaData");
            AddState(new artRename(this), "artRename");
            AddState(new artCreateState(this), "artCreateState");
            AddState(new artCreateStateDataEnter(this), "artCreateStateDataEnter");
            AddState(new artDeleteState(this), "artDeleteState");
            AddState(new artDeleteAsk(this), "artDeleteAsk");
            AddState(new artResizeMouseDown(this), "artResizeMouseDown");
            AddState(new artMoveMouseDown(this), "artMoveMouseDown");
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

        public void OnConfigure(object obj)
        {
            m_ConfigData = (artConfigurationData)obj;
            string dataPath = Application.dataPath + "/Resources/Config/";
            string fileName = dataPath + SelectedObj.name + ".json";
            LoadingInfo.m_NameOfConfigData = ConfigData.name;
            LoadingInfo.SaveToFile(fileName);
        }

        void OnPrintAboutToConsole()
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
#endif