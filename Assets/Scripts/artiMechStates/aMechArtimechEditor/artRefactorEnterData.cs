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
    <alias>Refactor Enter Data</alias>
    <comment></comment>
    <posX>365</posX>
    <posY>456</posY>
    <sizeX>156</sizeX>
    <sizeY>39</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artRefactorEnterData : artBaseDisplayOkCanel
    {
        artRenameWindow m_RenameWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        bool m_Once = false;
        public artRefactorEnterData(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artRefactorEnterData_To_artDisplayStates("artDisplayStates"));
            m_ConditionalList.Add(new artRefactorEnterData_To_artRefactScreen("artRefactScreen"));
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
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            m_RenameWindow.Update(theStateMachineEditor);
            if (!m_Once)
            {
                theStateMachineEditor.DrawToolBarBool = false;
                base.UpdateEditorGUI();
                m_Once = true;
            }
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_Once = false;
            artVisualStateNode node = GetSelectedNode();
            EntryString = node.ClassName;
            m_RenameWindow = new artRenameWindow(this, "Refactor Class", "Refactors the state class name:", 12, Color.black, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 4);
            m_RenameWindow.EntryLabel = "Refactor Name";

            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            theStateMachineEditor.DrawToolBarBool = false;

            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            theStateMachineEditor.DrawToolBarBool = true;
            base.Exit();
        }

        artVisualStateNode GetSelectedNode()
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                if (theStateMachineEditor.VisualStateNodes[i].RefactorBool)
                {

                    return theStateMachineEditor.VisualStateNodes[i];
                }
            }
            return null;
        }
    }
}
#endif