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

#if UNITY_EDITOR
#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Create State Data Entry</alias>
    <comment></comment>
    <posX>859</posX>
    <posY>359</posY>
    <sizeX>170</sizeX>
    <sizeY>46</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artCreateStateDataEnter : artBaseDisplayOkCanel
    {
        artRenameWindow m_NameStateindow;
        bool m_Once = false;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artCreateStateDataEnter(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artCreateStateDataEnter_To_artDisplayStates("artDisplayStates"));
            m_ConditionalList.Add(new artCreateStateDataEnter_To_artCreateState("artCreateState"));
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
            m_NameStateindow.Update(theStateMachineEditor);

            if (!m_Once)
            {
                theStateMachineEditor.DrawToolBarBool = false;
                theStateMachineEditor.MouseClickDownPosStart = Event.current.mousePosition;
                base.UpdateEditorGUI();
                m_Once = true;
            }
           // base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            m_Once = false;

            string machineName = utlDataAndFile.GetAfter(theStateMachineEditor.MachineScript.GetType().ToString(), ".");
            string className = theStateMachineEditor.MachineScript.GetType().ToString();
            //string fileAndPathForClass = utlDataAndFile.FindPathAndFileByClassName(machineName);
            string fileAndPathForClass = utlDataAndFile.FindPathAndFileByClassNameByDirectoryArray(machineName,theStateMachineEditor.ConfigData.GetRefactorAndConstructionPaths());
            string fileText = utlDataAndFile.LoadTextFromFile(fileAndPathForClass);
            int codeIndex = utlDataAndFile.CountSubstring(fileText, "AddState");

            EntryString = machineName + theStateMachineEditor.ConfigData.GenericStateName + utlDataAndFile.GetCode(codeIndex);
            m_NameStateindow = new artRenameWindow(this, "New State Name", "Enter a name for the new state:", 12, Color.black, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 4);

            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            if(OkBool)
                theStateMachineEditor.NewStateName = EntryString;

            base.Exit();
        }
    }
}
#endif