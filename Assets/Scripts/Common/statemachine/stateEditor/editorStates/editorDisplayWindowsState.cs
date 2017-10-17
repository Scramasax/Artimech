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
namespace artiMech
{
    public class editorDisplayWindowsState : baseState
    {

        public class menuData
        {
            public menuData(string fileAndPath,string replaceName)
            {
                m_FileAndPath = fileAndPath;
                m_ReplaceName = replaceName;
            }

            public string m_FileAndPath = "";
            public string m_ReplaceName = "";
        }

        #region Variables
        IList<stateConditionalBase> m_ConditionalList;

        bool m_bAddCondtion = false;
        bool m_bDeleteWindowNode = false;
        bool m_bMoveWindowNode = false;
        bool m_bRenameWindowNode = false;
        bool m_bResizeWindowNode = false;

        #endregion

        #region Accessors

        /// <summary>  State wants to add a condition and has set this bool. </summary>
        public bool AddConditional { get { return m_bAddCondtion; } }

        /// <summary>  State wants to delete a state/window and has set this bool. </summary>
        public bool DeleteWindowNode { get { return m_bDeleteWindowNode; } }

        /// <summary>  State wants to move a state window around and has set this bool. </summary>
        public bool MoveWindowNode { get { return m_bMoveWindowNode; } }

        /// <summary>  State wants to rename a statewindow and has set this bool. </summary>
        public bool RenameWindowNode { get { return m_bRenameWindowNode; } }

        /// <summary>  State wants to resize a statewindow and has set this bool. </summary>
        public bool ResizeWindowNode { get { return m_bResizeWindowNode; } }

        #endregion

        #region Member Functions

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        public editorDisplayWindowsState(GameObject gameobject)
        {
            m_bAddCondtion = false;
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorDisplayToWaitConditional("Wait"));
            m_ConditionalList.Add(new editorDisplayToLoadConditional("Load"));
            m_ConditionalList.Add(new editorDisplayToAddConditional("Add Conditional"));
            m_ConditionalList.Add(new editor_Display_To_Delete("Delete"));
            m_ConditionalList.Add(new editor_Display_To_Move("Move"));
            m_ConditionalList.Add(new editor_Display_To_Rename("Rename"));
            m_ConditionalList.Add(new editor_Display_To_Resize("Resize"));
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
                        //For shortening up the test position conditionals when it comes to code layout.
                        float x = stateEditorUtils.StateList[i].WinRect.x;
                        float y = stateEditorUtils.StateList[i].WinRect.y;
                        float width = stateEditorUtils.StateList[i].WinRect.width;
                        float height = stateEditorUtils.StateList[i].WinRect.height;

                        //Test to see if the mouse position is within the global limit of the window in the x.
                        if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                        {
                            //Test to see if the mouse position is within the global limit of the window in the y.
                            if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
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
            if (ev.control &&  ev.keyCode == KeyCode.S)
            {
                Debug.Log("<color=blue>" + "<b>" + "Saving...." + "</b></color>");
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                {
                    stateEditorUtils.StateList[i].SaveMetaData();
                }
            }

            //Right click and not on a state.
            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();

                    //TODO: add this from the xml.
                    menu.AddItem(new GUIContent("Add State/Empty State"), 
                                                false, 
                                                CreateAddStateCallback, 
                                                new menuData("Assets/Scripts/Common/statemachine/state_examples/stateEmptyExample.cs", "stateEmptyExample"));

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

            // render populated state windows
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                stateEditorUtils.StateList[i].Update(this);

                // highlight the current state
                if (Application.isPlaying)
                {
                    stateMachineBase stateMachine = stateEditorUtils.GameObject.GetComponent<stateMachineBase>();

                    string currentClassName = stateMachine.CurrentState.GetType().ToString();
                    currentClassName = currentClassName.Replace("artiMech.", "");

                    if(stateEditorUtils.StateList[i].ClassName==currentClassName)
                    {
                        float margin = 8.0f;
                        Rect lastRect = new Rect();
                        Vector4 startColor = new Vector4(0, 1, 1, 1);
                        Vector4 endColor = new Vector4(0.5f, 1, 1, 0.1f);
                        for (float edgleCoef = 0; edgleCoef<=1.0f; edgleCoef += 0.15f)
                        {
                            Vector4 colorVector = Vector4.Lerp(startColor, endColor, edgleCoef);
                            Color backroundColor = new Color(colorVector.x,colorVector.y,colorVector.z,colorVector.w);

                            Rect rect = new Rect(stateEditorUtils.StateList[i].WinRect.x - margin * edgleCoef,
                                                    stateEditorUtils.StateList[i].WinRect.y - margin * edgleCoef,
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

                        Handles.color = new Color(0.5f,1,1,0.8f);
                       
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

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

            ResetBools();
           
            stateEditorUtils.SaveStateInfo(stateEditorUtils.StateMachineName, stateEditorUtils.GameObject.name);
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
            m_bRenameWindowNode = false;
            m_bResizeWindowNode = false;
        }
        #endregion
    }
}