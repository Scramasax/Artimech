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
    <alias>Choose A Machine</alias>
    <comment></comment>
    <posX>230</posX>
    <posY>311</posY>
    <sizeX>147</sizeX>
    <sizeY>38</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artChooseMachine : editorStateBase
    {
        bool m_OkBool = false;
        bool m_CancelBool = false;
        bool m_CreateBool = false;
        artMachineChooseWindow m_ChooseWindow;

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

        public bool CreateBool
        {
            get
            {
                return m_CreateBool;
            }

            set
            {
                m_CreateBool = value;
            }
        }

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artChooseMachine(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artChooseMachine_To_artChooseStateMachineName("artChooseStateMachineName"));
            m_ConditionalList.Add(new artChooseMachine_To_artClearObject("artClearObject"));
            m_ConditionalList.Add(new artChooseMachine_To_artLoadStates("artLoadStates"));
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
            m_ChooseWindow.Update();
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            GameObject gmObject = (GameObject)ArtimechEditor.Inst.SelectedObj;
            iMachineBase[] machines = gmObject.GetComponents<iMachineBase>();
            m_ChooseWindow = new artMachineChooseWindow(this, machines, "Choose Or Create A State Machine", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);

            m_CancelBool = false;
            m_OkBool = false;
            m_CreateBool = false;
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
