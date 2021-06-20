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
    <alias>Add Conditonal</alias>
    <comment></comment>
    <posX>862</posX>
    <posY>196</posY>
    <sizeX>175</sizeX>
    <sizeY>48</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artAddConditionalState : artDisplayWindowsBaseState
    {
        artVisualStateNode m_WindowsSelectedNode = null;
        bool m_RightClickBool = false;
        bool m_LeftClickBool = false;
        bool m_Once = false;
        int m_LeftClickBlock = 0;

        public bool RightClickBool { get { return m_RightClickBool; } }
        public bool LeftClickBool { get { return m_LeftClickBool; } }

        Vector2 m_MousePos;

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artAddConditionalState(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artAddConditionalState_To_artDisplayStates("artDisplayStates"));
            m_ConditionalList.Add(new artAddConditionalState_To_artAddConditionalPostState("artAddConditionalPostState"));
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

            //base.UpdateEditorGUI();
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            Event ev = Event.current;

            //if(m_WindowsSelectedNode==null)
            //    m_WindowsSelectedNode = GetWindowsNodeAtThisLocation(ev.mousePosition);

            m_MousePos = ev.mousePosition;
            if (m_WindowsSelectedNode == null)
            {
                Debug.Log("Can't find state window!");
                return;
            }


            if (ev.button == 0)
            {
                Debug.Log("-------------> " + ev.type);
                //if (ev.type == EventType.Used && m_WindowsSelectedNode != null && m_LeftClickBool == false)
                if (ev.type == EventType.MouseDown &&  m_WindowsSelectedNode != null && !m_LeftClickBool)
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

            //DrawConditionalTemp(ev.mousePosition);
            DrawConditionalTemp(m_MousePos);

            base.UpdateEditorGUI();
            // 

            theStateMachineEditor.EditorRepaint();

            m_Once = true;
            m_LeftClickBlock += 1;
        }

        void DrawConditionalTemp(Vector2 mousePos)
        {
           // Debug.Log("mousePos = " + mousePos);

            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            Vector3 modMousePos = mousePos;
            //Debug.Log("modMousePos = " + modMousePos);
            Vector3 startPos = theStateMachineEditor.TransMtx.Transform(m_WindowsSelectedNode.GetPos());
            startPos.x += m_WindowsSelectedNode.WinRect.width * 0.5f;
            startPos.y += m_WindowsSelectedNode.WinRect.height * 0.5f;
            startPos.z = 1;
            modMousePos.z = 1;
            Color shadowCol = new Color(1, 1, 1, 0.2f);
            Color arrowCol = new Color(204 / 255, 255 / 255, 102 / 255, 1.0f);

            m_MainWindow.ArrowTemp = new artMainWindow.ConditionArrowTemp(startPos,
                                    modMousePos,
                                    theStateMachineEditor.TransMtx.Transform(m_WindowsSelectedNode.WinRect),
                                    new Rect(0, 0, 0, 0),
                                    2,
                                    Color.black,
                                    2,
                                    shadowCol, arrowCol);

            /*artGUIUtils.DrawArrow(startPos,
                                    modMousePos,
                                    theStateMachineEditor.TransMtx.Transform(m_WindowsSelectedNode.WinRect),
                                    new Rect(0, 0, 0, 0),
                                    2,
                                    Color.black,
                                    2,
                                    shadowCol, arrowCol);*/
            //artGUIUtils.DrawArrowTranformed(theStateMachineEditor.TransMtx, new Vector2(0, 0), new Vector2(400, 400), m_MainWindow.WinRect, m_MainWindow.WinRect, 2, Color.black, Color.white);
        }

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
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            theStateMachineEditor.DrawToolBarBool = false;
            m_WindowsSelectedNode = theStateMachineEditor.SelectedVisualStateNode;
            m_MainWindow.ArrowTemp = null;
            DrawMenuBool = false;
            m_Once = false;
            m_RightClickBool = false;
            m_LeftClickBool = false;
            m_LeftClickBlock = 0;
            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            DrawMenuBool = true;
            m_MainWindow.ArrowTemp = null;
            base.Exit();
        }
    }
}