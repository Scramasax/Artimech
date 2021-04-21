/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
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

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Move Mouse Down</alias>
    <comment></comment>
    <posX>529</posX>
    <posY>-217</posY>
    <sizeX>138</sizeX>
    <sizeY>37</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artMoveMouseDown : artDisplayWindowsBaseState
    {
        //Vector2 m_MousePositionEnter;
        Vector2 m_MousePosition;
        Vector2 m_StartOffset;
        Vector2 m_MoveOffsetPercent;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artMoveMouseDown(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artMoveMouseDown_To_artDisplayStates("artDisplayStates"));
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

            base.FixedUpdate();
        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            Event ev = Event.current;
            artVisualStateNode m_WindowsSelectedNode = this.GetSelectedNode();

            if (m_WindowsSelectedNode != null && ev.button == 0)
            {
                if (ev.type != EventType.MouseUp)
                {
                    float x = m_WindowsSelectedNode.WinRect.x;
                    float y = m_WindowsSelectedNode.WinRect.y;
                    float width = m_WindowsSelectedNode.WinRect.width;
                    float height = m_WindowsSelectedNode.WinRect.height;

                    m_WindowsSelectedNode.SetPos(ev.mousePosition.x - (width * m_MoveOffsetPercent.x), ev.mousePosition.y - (height * m_MoveOffsetPercent.y));
                    stateEditorUtils.Repaint();
                }

            }

            base.UpdateEditorGUI();

            m_MainWindow.Update();
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            theStateMachineEditor.Repaint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        artVisualStateNode GetSelectedNode()
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                if (theStateMachineEditor.VisualStateNodes[i].Selected)
                {

                    return theStateMachineEditor.VisualStateNodes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            artVisualStateNode visualStateNode = GetSelectedNode();
            if (visualStateNode != null)
            {
                ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
                float diff = theStateMachineEditor.MouseClickDownPosStart.x - visualStateNode.WinRect.x;
                m_MoveOffsetPercent.x = diff / visualStateNode.WinRect.width;

                diff = theStateMachineEditor.MouseClickDownPosStart.y - visualStateNode.WinRect.y;
                m_MoveOffsetPercent.y = diff / visualStateNode.WinRect.height;
            }

            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }
    }
}
#endif
