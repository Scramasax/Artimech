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

        #region Variables
        IList<stateConditionalBase> m_ConditionalList;
        bool m_AddConditionalBool = false;
        bool m_MoveWindowNode = false;
        #endregion

        #region Accessors
        public bool AddConditionalBool
        {
            get
            {
                return m_AddConditionalBool;
            }
        }

        /// <summary>  Returns true if the mouse cursor is hovering over the window title area. </summary>
        public bool MoveWindowNode { get { return m_MoveWindowNode; } }

        #endregion

        #region Member Functions

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        public editorDisplayWindowsState(GameObject gameobject)
        {
            m_AddConditionalBool = false;
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorDisplayToWaitConditional("Wait"));
            m_ConditionalList.Add(new editorDisplayToLoadConditional("Load"));
            m_ConditionalList.Add(new editorDisplayToAddConditional("Add Conditional"));
            m_ConditionalList.Add(new editor_Display_To_Move("Move"));
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
         //   GUILayout.Label(m_BackGroundImage);

            // input
            Event ev = Event.current;
            stateEditorUtils.MousePos = ev.mousePosition;

            if (ev.button == 0)
            {
                //Debug.Log("-------------> " + ev.type);
                if (ev.type == EventType.Used)
                    //Debug.Log("-------------> " + ev.type);
                    for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                    {
                        if (!stateEditorUtils.StateList[i].MainBodyHover)
                            continue;
                        
                        float x = stateEditorUtils.StateList[i].WinRect.x;
                        float y = stateEditorUtils.StateList[i].WinRect.y;
                        float width = stateEditorUtils.StateList[i].WinRect.width;
                        float height = stateEditorUtils.StateList[i].WinRect.height;
                    
                        if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                        {
                            if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
                            {
                                m_MoveWindowNode = true;
                                stateEditorUtils.SelectedNode = stateEditorUtils.StateList[i];
                                stateEditorUtils.Repaint();
                            }
                        }       
                }
            }

            //if( ev.control)
            //    Debug.Log("<color=blue>" + "<b>" + "ev = " + "</b></color>" + ev.ToString());

            //saves meta data for the visual window system
            if (ev.control &&  ev.keyCode == KeyCode.S)
            {
                Debug.Log("<color=blue>" + "<b>" + "Saving...." + "</b></color>");
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                {
                    stateEditorUtils.StateList[i].SaveMetaData();
                }
            }

            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add State"), false, ContextCallback, "addState");
                    menu.ShowAsContext();
                    ev.Use();
                }
            }

            // render populated state windows

            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                stateEditorUtils.StateList[i].Update(this);
            }

            stateEditorUtils.Repaint();
        }

        void ContextCallback(object obj)
        {
            stateEditorUtils.ContextCallback(obj);
        }

        public void AddConditionalCallback(object obj)
        {
            m_AddConditionalBool = true;
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_AddConditionalBool = false;
            m_MoveWindowNode = false;
            stateEditorUtils.SaveStateInfo(stateEditorUtils.StateMachineName, stateEditorUtils.GameObject.name);
            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
#endregion
    }
}