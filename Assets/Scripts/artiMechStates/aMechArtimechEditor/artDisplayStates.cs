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
    <alias>Display States</alias>
    <comment></comment>
    <posX>521</posX>
    <posY>199</posY>
    <sizeX>220</sizeX>
    <sizeY>142</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artDisplayStates : artDisplayWindowsBaseState
    {
        bool m_SaveDataBool = false;
        bool m_RenameBool = false;


        public bool SaveDataBool { get { return m_SaveDataBool; }  set { m_SaveDataBool = value; } }
        public bool RenameBool { get { return m_RenameBool; }  set { m_RenameBool = value; } }

        // artMainWindow m_MainWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artDisplayStates(Object unityObj) : base(unityObj)
        {
            //m_MainWindow = new artMainWindow(this, "Main Display Window", new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 1);
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artDisplayStates_To_artDeleteConditionalStartState("artDeleteConditionalStartState"));
            m_ConditionalList.Add(new artDisplayStates_To_artAddConditionalState("artAddConditionalState"));
            m_ConditionalList.Add(new artDisplayStates_To_artChooseStateMachineName("artChooseStateMachineName"));
            m_ConditionalList.Add(new artDisplayStates_To_artRefactorEnterData("artRefactorEnterData"));
            m_ConditionalList.Add(new artDisplayStates_To_artRefactorDataEntry("artRefactorDataEntry"));
            m_ConditionalList.Add(new artDisplayStates_To_artRename("artRename"));
            m_ConditionalList.Add(new artDisplayStates_To_artRenameAlias("artRenameAlias"));
            m_ConditionalList.Add(new artDisplayStates_To_artSaveScreen("artSaveScreen"));
            m_ConditionalList.Add(new artDisplayStates_To_artCreateStateDataEnter("artCreateStateDataEnter"));
            m_ConditionalList.Add(new artDisplayStates_To_artDeleteAsk("artDeleteAsk"));
            m_ConditionalList.Add(new artDisplayStates_To_artResizeMouseDown("artResizeMouseDown"));
            m_ConditionalList.Add(new artDisplayStates_To_artMoveMouseDown("artMoveMouseDown"));
            m_ConditionalList.Add(new artDisplayStates_To_artCheckIfIMachine("artCheckIfIMachine"));
            m_ConditionalList.Add(new artDisplayStates_To_artNoObject("artNoObject"));
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
            //Saves meta data for the visual window system via the keyboard
            if (ev.control && ev.keyCode == KeyCode.S)
            {
                SaveDataBool = true;
            }
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            if (Event.current.type == EventType.MouseDown)
            {
                theStateMachineEditor.MouseClickDownPosStart = Event.current.mousePosition;
            }
            base.UpdateEditorGUI();
            //theStateMachineEditor.Repaint();
        }


        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            SaveDataBool = false;
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            theStateMachineEditor.DrawToolBarBool = true;
            theStateMachineEditor.CreateStateBool = false;
            theStateMachineEditor.CreateConditionalBool = false;
            theStateMachineEditor.DeleteConditionalBool = false;
            theStateMachineEditor.DeleteStateBool = false;
            theStateMachineEditor.FromConditionalStateNode = null;
            theStateMachineEditor.ToConditionalStateNode = null;

            theStateMachineEditor.Repaint();
            for(int i=0;i<theStateMachineEditor.VisualStateNodes.Count;i++)
            {
                theStateMachineEditor.VisualStateNodes[i].Reset();
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