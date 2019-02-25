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
    <alias>Choose Name</alias>
    <comment></comment>
    <posX>210</posX>
    <posY>550</posY>
    <sizeX>123</sizeX>
    <sizeY>44</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artChooseStateMachineName : editorStateBase
    {
        artChooseStateMachineNameMachineWindow m_CreateWindow;

 /*       public enum eCreationType
        {
            EmptyState = 0,
            EventState = 1,
            MachineSubState = 2
        } */

        bool m_OkBool = false;
        bool m_CancelBool = false;
        string m_StateMachineName = "";
  //      eCreationType m_CreationType;

        public bool OkBool
        {
            get
            {
                return m_OkBool;
            }

            set
            {
                m_OkBool = value;
            }
        }

        public bool CancelBool
        {
            get
            {
                return m_CancelBool;
            }

            set
            {
                m_CancelBool = value;
            }
        }

        public string StateMachineName
        {
            get
            {
                return m_StateMachineName;
            }

            set
            {
                m_StateMachineName = value;
            }
        }

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artChooseStateMachineName(Object unityObj) : base (unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artChooseStateMachineName_To_artClearObject("artClearObject"));
            m_ConditionalList.Add(new artChooseStateMachineName_To_artCreateStateMachine("artCreateStateMachine"));
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
            m_CreateWindow.Update();
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_CancelBool = false;
            m_OkBool = false;
            StateMachineName = "";

            GameObject gmObject = (GameObject)ArtimechEditor.Inst.SelectedObj;
            iMachineBase[] machines = gmObject.GetComponents<iMachineBase>();
            m_CreateWindow = new artChooseStateMachineNameMachineWindow(this, "Choose A State Machine To Create", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);

            ArtimechEditor.Inst.DrawToolBarBool = false;
            ArtimechEditor.Inst.Repaint();
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
