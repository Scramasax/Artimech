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
namespace artiMech
{
    public class editorMoveState : editorBaseState
    {
        stateWindowsNode m_WindowsSelectedNode = null;
        bool m_ActionConfirmed = false;
        Vector2 m_MoveOffsetPercent;

        #region Accessors

        /// <summary>  Returns true if the action is confirmed. </summary>
        public bool ActionConfirmed { get { return m_ActionConfirmed; } }

        #endregion

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public editorMoveState(GameObject gameobject) : base(gameobject)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Move_To_Display("Display Windows"));
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
                if (ev.type != EventType.mouseUp)
                {
                    float x = m_WindowsSelectedNode.WinRect.x;
                    float y = m_WindowsSelectedNode.WinRect.y;
                    float width = m_WindowsSelectedNode.WinRect.width;
                    float height = m_WindowsSelectedNode.WinRect.height;

                    Vector2 mousePos = new Vector2();
                    mousePos = stateEditorUtils.TranslationMtx.UnTransform(ev.mousePosition);

                    if (mousePos.x >= x && mousePos.x <= x + width)
                    {
                        if (mousePos.y >= y && mousePos.y <= y + height)
                        {
                            m_WindowsSelectedNode.SetPos(ev.mousePosition.x - (width * m_MoveOffsetPercent.x), ev.mousePosition.y - (height * m_MoveOffsetPercent.y));
                            stateEditorUtils.Repaint();
                        }
                    }
                }
                else
                    m_ActionConfirmed = true;

            }

            /*
            stateEditorDrawUtils.DrawGridBackground();

            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                stateEditorUtils.StateList[i].Update(this);
            }*/

            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            m_ActionConfirmed = false;

            float diff = stateEditorUtils.MousePos.x - stateEditorUtils.SelectedNode.WinRect.x;
            m_MoveOffsetPercent.x = diff / stateEditorUtils.SelectedNode.WinRect.width;

            diff = stateEditorUtils.MousePos.y - stateEditorUtils.SelectedNode.WinRect.y;
            m_MoveOffsetPercent.y = diff / stateEditorUtils.SelectedNode.WinRect.height;
            //Debug.Log("m_MoveOffsetPercent.x = " + m_MoveOffsetPercent.x);


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