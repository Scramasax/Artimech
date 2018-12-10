/// Artimech
/// 
/// Copyright � <2017-2018> <George A Lancaster>
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class editorDisplayWindowsState : editorBaseState
    {

        public class menuData
        {
            public menuData(string fileAndPath, string replaceName)
            {
                m_FileAndPath = fileAndPath;
                m_ReplaceName = replaceName;
            }

            public string m_FileAndPath = "";
            public string m_ReplaceName = "";
        }

        #region Variables

        bool m_bAddCondtion = false;
        bool m_bDeleteWindowNode = false;
        bool m_bMoveWindowNode = false;
        bool m_bMoveBackground = false;
        bool m_bRenameWindowNode = false;
        bool m_bResizeWindowNode = false;
        bool m_bRefactor = false;
        bool m_bSave = false;
        bool m_bCopyStateMachine = false;
        bool m_bDeleteConditional = false;

        #endregion

        #region Accessors

        /// <summary>  State wants to add a condition and has set this bool. </summary>
        public bool AddConditional { get { return m_bAddCondtion; } }

        /// <summary>  State wants to delete a state/window and has set this bool. </summary>
        public bool DeleteWindowNode { get { return m_bDeleteWindowNode; } }

        /// <summary>  State wants to move a state window around and has set this bool. </summary>
        public bool MoveWindowNode { get { return m_bMoveWindowNode; } }

        /// <summary>  State wants to move a state window around and has set this bool. </summary>
        public bool MoveBackground { get { return m_bMoveBackground; } }

        /// <summary>  State wants to rename a statewindow and has set this bool. </summary>
        public bool RenameWindowNode { get { return m_bRenameWindowNode; } }

        /// <summary>  State wants to resize a statewindow and has set this bool. </summary>
        public bool ResizeWindowNode { get { return m_bResizeWindowNode; } }

        /// <summary>  State wants to refactor a class and has set this bool. </summary>
        public bool RefactorClass { get { return m_bRefactor; } }

        /// <summary>  State wants to Save metadata and has set this bool. </summary>
        public bool Save { get { return m_bSave; } set { m_bSave = value; } }

        /// <summary>  State wants to copy a statemachine and has set this bool. </summary>
        public bool CopyStateMachine { get { return m_bCopyStateMachine; } set { m_bCopyStateMachine = value; } }

        /// <summary>  State wants to delete a conditional and has set this bool. </summary>
        public bool DeleteConditional { get { return m_bDeleteConditional; } }
        #endregion

        #region Member Functions

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        public editorDisplayWindowsState(GameObject gameobject) : base(gameobject)
        {
            m_bAddCondtion = false;
            m_UnityObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Display_To_Wait("Wait"));
            m_ConditionalList.Add(new editor_Display_To_Load("Load"));
            m_ConditionalList.Add(new editor_Display_To_Add("Add Conditional"));
            m_ConditionalList.Add(new editor_Display_To_Delete("Delete"));
            m_ConditionalList.Add(new editor_Display_To_Move("Move"));
            m_ConditionalList.Add(new editor_Display_To_MoveBackground("MoveBackground"));
            m_ConditionalList.Add(new editor_Display_To_Rename("Rename"));
            m_ConditionalList.Add(new editor_Display_To_Resize("Resize"));
            m_ConditionalList.Add(new editor_Display_To_Refactor("Refactor"));
            m_ConditionalList.Add(new editor_Display_To_Save("Save"));
            m_ConditionalList.Add(new editor_Display_To_CopyStateMachine("Copy State Machine"));
            m_ConditionalList.Add(new editor_Display_To_DeleteCondition("Delete Conditional"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            for (int i = 0; i < m_ConditionalList.Count; i++)
            {
                string changeNameToThisState = null;
                changeNameToThisState = m_ConditionalList[i].UpdateConditionalTest(this);
                if (changeNameToThisState != null)
                {
                    m_ChangeStateName = changeNameToThisState;
                    m_ChangeBool = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
            // input
            Event ev = Event.current;
            stateEditorUtils.MousePos = ev.mousePosition;

            //if the mouse button is down.
            if (ev.button == 0)
            {
                //if the mouse has clicked a visual state
                if (ev.type == EventType.Used)
                {
                    //loop through and find what state has been clicked.
                    for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                    {

                        Vector2 winPos = new Vector3(stateEditorUtils.StateList[i].WinRect.x, stateEditorUtils.StateList[i].WinRect.y);
                        Vector3 winSize = new Vector3(stateEditorUtils.StateList[i].WinRect.width, stateEditorUtils.StateList[i].WinRect.height);

                        Vector2 winTransPos = stateEditorUtils.TranslationMtx.Transform(winPos);

                        Vector3 mouseTransPos = new Vector3();
                        //transPos = stateEditorUtils.TranslationMtx.UnTransform(ev.mousePosition);
                        mouseTransPos = ev.mousePosition;

                        //Test to see if the mouse position is within the global limit of the window in the x.
                        if (mouseTransPos.x >= winTransPos.x && mouseTransPos.x <= winTransPos.x + winSize.x)
                        {
                            //Test to see if the mouse position is within the global limit of the window in the y.
                            if (mouseTransPos.y >= winTransPos.y && mouseTransPos.y <= winTransPos.y + winSize.y)
                            {
                                // If the mouse button is clicked then check to see what to do depending and where
                                // in the visual state window your are pointing at.
                                stateEditorUtils.SelectedNode = stateEditorUtils.StateList[i];
                                stateEditorUtils.Repaint();

                                // Set these for state conditions.
                                m_bMoveWindowNode = stateEditorUtils.SelectedNode.MainBodyHover;
                                m_bResizeWindowNode = stateEditorUtils.SelectedNode.ResizeBodyHover;
                                m_bRenameWindowNode = stateEditorUtils.SelectedNode.TitleHover;
                                m_bDeleteWindowNode = stateEditorUtils.SelectedNode.CloseButtonHover;
                            }
                        }
                    }
                }
            }


            //Saves meta data for the visual window system via the keyboard
            if (ev.control && ev.keyCode == KeyCode.S)
            {
                /*
                Debug.Log("<color=blue>" + "<b>" + "Saving...." + "</b></color>");
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                {
                    stateEditorUtils.StateList[i].SaveMetaData();
                }
                Debug.Log("<color=blue>" + "<b>" + "Control s Saving...." + "</b></color>");*/
                m_bSave = true;
                return;
            }



            //Right click and not on a state.
            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();

                    for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                    {
                        string conditionEditName = stateEditorUtils.StateList[i].GetConditionalByPosition(stateEditorUtils.TranslationMtx.UnTransform(ev.mousePosition), 10);
                        if (conditionEditName != null)
                        {
                            menu.AddItem(new GUIContent("Edit Conditional"),
                            false,
                            CreateEditConditionalCallback,
                            conditionEditName);
                        }

                        if (conditionEditName != null)
                        {
                            menu.AddItem(new GUIContent("Delete Conditional"),
                            false,
                            DeleteEditConditionalCallback,
                            conditionEditName);
                        }
                        //if (stateEditorUtils.StateList[i].GetConditionalByPosition(ev.mousePosition, 10);
                    }

                    string fileAndPath = " ";
                    string replaceStr = " ";
                    if (stateEditorUtils.SelectedObject is GameObject)
                    {
                        fileAndPath = "Assets/Scripts/Common/statemachine/state_examples/stateEmptyExample.cs";
                        replaceStr = "stateEmptyExample";
                    }
                    else
                    {
                        fileAndPath = "Assets/Scripts/Common/statemachine/state_examples/stateEditorExample.cs";
                        replaceStr = "stateEditorExample";
                    }

                    //TODO: add this from the xml.
                    menu.AddItem(new GUIContent("Add State/Empty State"),
                                                false,
                                                CreateAddStateCallback,
                                                new menuData(fileAndPath, replaceStr));

                    menu.AddItem(new GUIContent("Add State/Subscribe State"),
                                                false,
                                                CreateAddStateCallback,
                                                new menuData("Assets/Scripts/Common/statemachine/state_examples/stateEventSubscribeExample.cs", "stateEventSubscribeExample"));

                    menu.AddItem(new GUIContent("Add State/Publish State"),
                                                false,
                                                CreateAddStateCallback,
                                                new menuData("Assets/Scripts/Common/statemachine/state_examples/stateEventPublishExample.cs", "stateEventPublishExample"));



                    menu.ShowAsContext();
                    ev.Use();
                }
            }

            // Middle hold click and not on a state.
            if (ev.button == 2)
            {
                //Debug.Log("<color=blue>" + "<b>" + ev.type + "</b></color>");
                if (ev.type == EventType.MouseDown)
                {
                    //Debug.Log("<color=red>" + "<b>" + "middle mouse down" + "</b></color>");
                    m_bMoveBackground = true;
                }
                //ev.Use();
            }

            // render populated state windows
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {

                // highlight the current state
                if ((stateEditorUtils.SelectedObject is GameObject && Application.isPlaying && stateEditorUtils.SelectedObject != null) ||
                    stateEditorUtils.SelectedObject != null && stateEditorUtils.SelectedObject is ScriptableObject)
                {
                    GameObject gameObj = null;
                    iMachineBase stateMachine;
                    if (stateEditorUtils.SelectedObject is GameObject)
                    {
                        gameObj = (GameObject)stateEditorUtils.SelectedObject;
                        stateMachine = gameObj.GetComponent<iMachineBase>();
                    }
                    else
                    {
                        stateMachine = (iMachineBase)stateEditorUtils.SelectedObject;
                    }

                    string currentClassName = stateMachine.GetCurrentState().GetType().ToString();
                    currentClassName = currentClassName.Replace(stateEditorUtils.kArtimechNamespace, "");

                    if (stateEditorUtils.StateList[i].ClassName == currentClassName)
                    {
                        float margin = 8.0f;
                        Rect lastRect = new Rect();
                        Vector4 startColor = new Vector4(0, 1, 1, 1);
                        Vector4 endColor = new Vector4(0.5f, 1, 1, 0.1f);
                        for (float edgleCoef = 0; edgleCoef <= 1.0f; edgleCoef += 0.15f)
                        {
                            Vector4 colorVector = Vector4.Lerp(startColor, endColor, edgleCoef);
                            Color backroundColor = new Color(colorVector.x, colorVector.y, colorVector.z, colorVector.w);

                            Vector2 transVect = new Vector2();
                            transVect = stateEditorUtils.TranslationMtx.Transform(stateEditorUtils.StateList[i].WinRect.position);

                            Rect rect = new Rect(transVect.x - margin * edgleCoef,
                                                    transVect.y - margin * edgleCoef,
                                                    stateEditorUtils.StateList[i].WinRect.width + (margin * 2 * edgleCoef),
                                                    stateEditorUtils.StateList[i].WinRect.height + (margin * 2 * edgleCoef));

                            EditorGUI.DrawRect(rect, backroundColor);
                            lastRect = rect;
                        }
                        Vector3 centerPos = new Vector3();
                        centerPos.x = lastRect.x + (lastRect.width * 0.5f);
                        centerPos.y = lastRect.y + (lastRect.height * 0.5f);

                        Vector3 windowSize = new Vector3();
                        windowSize.x = (lastRect.width);
                        windowSize.y = (lastRect.height);

                        Handles.color = new Color(0.5f, 1, 1, 0.8f);

                        Handles.DrawWireCube(centerPos, windowSize);
                    }
                }
            }

            stateEditorUtils.Repaint();
        }

        void CreateAddStateCallback(object obj)
        {
            stateEditorUtils.CreateStateContextCallback(obj);
        }

        void CreateEditConditionalCallback(object obj)
        {
            string className = (string)obj;
            string fileAndPathName = utlDataAndFile.FindPathAndFileByClassName(className);
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileAndPathName, 1);
        }

        void DeleteEditConditionalCallback(object obj)
        {
            string className = (string)obj;
            string fileAndPathName = utlDataAndFile.FindPathAndFileByClassName(className);
            stateEditorUtils.DeleteConditionalPath = fileAndPathName;
            stateEditorUtils.DeleteConditionalClass = className;


            m_bDeleteConditional = true;

            //UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileAndPathName, 1);
        }

        public void AddConditionalCallback(object obj)
        {
            //some ugly data exchange.  TODO: messaging. 
            menuData data = (menuData)obj;
            stateEditorUtils.AddConditionPath = data.m_FileAndPath;
            stateEditorUtils.AddConditionReplace = data.m_ReplaceName;

            m_bAddCondtion = true;

        }

        public void EditScriptCallback(object obj)
        {
            string fileAndPathName = utlDataAndFile.FindPathAndFileByClassName(stateEditorUtils.SelectedNode.ClassName);
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileAndPathName, 1);
        }

        public void RefactorClassCallback(object obj)
        {
            m_bRefactor = true;
            //utlDataAndFile.RefactorAllAssets(stateEditorUtils.SelectedNode.ClassName,renameStr);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

            ResetBools();

            stateEditorUtils.SaveStateInfo(stateEditorUtils.StateMachineName, stateEditorUtils.SelectedObject.name);
            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
        /// <summary>
        /// Set state bools back to false.
        /// </summary>
        void ResetBools()
        {
            m_bAddCondtion = false;
            m_bDeleteWindowNode = false;
            m_bMoveWindowNode = false;
            m_bMoveBackground = false;
            m_bRenameWindowNode = false;
            m_bResizeWindowNode = false;
            m_bRefactor = false;
            m_bSave = false;
            m_bCopyStateMachine = false;
            m_bDeleteConditional = false;
        }
        #endregion
    }
}
#endif