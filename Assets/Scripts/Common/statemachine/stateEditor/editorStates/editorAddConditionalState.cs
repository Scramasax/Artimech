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
    public class editorAddConditionalState : editorBaseState
    {        
     
        stateWindowsNode m_WindowsSelectedNode = null;
        bool m_RightClickBool = false;
        bool m_LeftClickBool = false;

        public bool RightClickBool { get { return m_RightClickBool; } }
        public bool LeftClickBool { get { return m_LeftClickBool; } }

        /// <summary>
        /// This starts the add a condition input sequence.
        /// </summary>
        /// <param name="gameobject"></param>
        public editorAddConditionalState(GameObject gameobject) : base(gameobject)
        {
            m_UnityObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorAddConditionalToDisplay("Display Windows"));
            m_ConditionalList.Add(new editorAddCondToPosAddConditional("Post Add Condition"));
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
            Event ev = Event.current;
            stateEditorUtils.MousePos = ev.mousePosition;

            //check to see if 
            stateWindowsNode stateNode = stateEditorUtils.GetWindowsNodeAtThisLocation(ev.mousePosition);

            if (ev.button == 0)
            {
  //              Debug.Log("-------------> " + ev.type);
                if (ev.type == EventType.Used && stateNode != null && m_LeftClickBool == false)
                {
                    m_LeftClickBool = true;
                    //stateEditorUtils.CreateConditionalAndAddToState(stateEditorUtils.SelectedNode.WindowTitle, stateNode.WindowTitle);
                }
            }

            if (ev.keyCode == KeyCode.Escape)
            {
                m_RightClickBool = true;
            }

            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    m_RightClickBool = true;
                }
            }

            DrawConditionalTemp();

            stateEditorUtils.Repaint();
        }

        void DrawConditionalTemp()
        {
            Vector3 startPos = stateEditorUtils.TranslationMtx.Transform(m_WindowsSelectedNode.GetPos());
            startPos.x += m_WindowsSelectedNode.WinRect.width * 0.5f;
            startPos.y += m_WindowsSelectedNode.WinRect.height * 0.5f;
            Color shadowCol = new Color(1, 1, 1, 0.2f);
            Color arrowCol = new Color(204 / 255, 255 / 255, 102 / 255, 1.0f);
            
            stateEditorDrawUtils.DrawArrow(startPos, 
                                    stateEditorUtils.MousePos, 
                                    stateEditorUtils.TranslationMtx.Transform(m_WindowsSelectedNode.WinRect), 
                                    new Rect(0,0,0,0), 
                                    2, 
                                    Color.black, 
                                    2, 
                                    shadowCol,arrowCol);
        }

        void ContextCallback(object obj)
        {
            stateEditorUtils.CreateStateContextCallback(obj);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            m_RightClickBool = false;
            m_LeftClickBool = false;
            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}

#endif