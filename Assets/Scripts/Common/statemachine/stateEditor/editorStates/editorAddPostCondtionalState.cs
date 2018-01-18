#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

/// <summary>
/// This looks for the up click and then a condition is added and
/// the state is returned to 'Display Windows'
/// </summary>
namespace Artimech
{
    public class editorAddPostCondtionalState : editorBaseState
    {
        stateWindowsNode m_WindowsSelectedNode = null;
        bool m_ExitAddPostState = false;
        public bool ExitAddPostState { get { return m_ExitAddPostState; } }
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public editorAddPostCondtionalState(GameObject gameobject) : base(gameobject)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorAddPostConditionalToDisplay("Display Windows"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            base.Update();
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

            if (ev.button == 0)
            {

                stateWindowsNode stateNode = stateEditorUtils.GetWindowsNodeAtThisLocation(ev.mousePosition);
                if (ev.type == EventType.MouseUp && m_ExitAddPostState == false)
                {
                    if(stateNode != null)
                        stateEditorUtils.CreateConditionalAndAddToState(stateEditorUtils.SelectedNode.ClassName, stateNode.ClassName);
                    m_ExitAddPostState = true;
                }
            }

            if (ev.keyCode == KeyCode.Escape)
            {
                m_ExitAddPostState = true;
            }

            stateEditorUtils.Repaint();
        }

        void DrawConditionalTemp()
        {
            Vector3 startPos = m_WindowsSelectedNode.GetPos();
            startPos.x += m_WindowsSelectedNode.WinRect.width * 0.5f;
            startPos.y += m_WindowsSelectedNode.WinRect.height * 0.5f;
            Color shadowCol = new Color(1, 1, 1, 0.2f);
            Color arrowCol = new Color(204 / 255, 255 / 255, 102 / 255, 1.0f);

            stateEditorDrawUtils.DrawArrow(startPos, stateEditorUtils.MousePos, m_WindowsSelectedNode.WinRect, new Rect(0, 0, 0, 0), 2, Color.black, 2, shadowCol, arrowCol);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            m_ExitAddPostState = false;
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