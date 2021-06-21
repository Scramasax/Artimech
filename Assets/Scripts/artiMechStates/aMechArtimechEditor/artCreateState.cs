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
    <alias>Create State</alias>
    <comment></comment>
    <posX>864</posX>
    <posY>480</posY>
    <sizeX>125</sizeX>
    <sizeY>38</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artCreateState : artBaseCreateState
    {
        artProcessingWindow m_MessageWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artCreateState(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artCreateState_To_artDisplayStates("artDisplayStates"));
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
            m_MessageWindow.Update(theStateMachineEditor);
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_MessageWindow = new artProcessingWindow("Artimech System Status", "Creating State.....", 16, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 10);

            ArtimechEditor theScript = (ArtimechEditor)GetScriptableObject;

            string directoryName = theScript.ConfigData.CopyToDirectory.m_PathName +
                        "/" +
                        //                        theScript.ConfigData.PrefixName +
                        theScript.MachineScript.GetType().Name;

            string stateName = "";
            stateName = ReadReplaceAndWrite(
                                    theScript.ConfigData.MasterScriptStateFile.m_PathAndName,
                                    /*theScript.CurrentStateMachineName +*/ theScript.NewStateName,
                                    theScript.ConfigData.CopyToDirectory.m_PathName,
                                    directoryName + "/" + /*theScript.ConfigData.PrefixName + theScript.MachineScript.GetType().Name +*/ theScript.NewStateName + ".cs",
                                    theScript.StateNameClassReplace,
                                    "");

            string pathAndFile = directoryName + "/" + theScript.MachineScript.GetType().Name + ".cs";

            AddStateCodeToStateMachineCode(pathAndFile, stateName);

            string pathAndFileStateName = directoryName + "/" + theScript.NewStateName + ".cs";

            stateEditorUtils.SaveStateWindowsNodeData(pathAndFileStateName,
                                                        stateName,
                                                        (int)theScript.MouseClickDownPosStart.x,
                                                        (int)theScript.MouseClickDownPosStart.y,
                                                        200, 80);

            artVisualStateNode node = CreateVisualStateNode(theScript.NewStateName);
            theScript.VisualStateNodes.Add(node);

            base.Enter();

            UnityEditor.AssetDatabase.Refresh();
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