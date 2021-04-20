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
/// 

#if UNITY_EDITOR
using UnityEngine;
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
    <alias>Create</alias>
    <comment></comment>
    <posX>294</posX>
    <posY>332</posY>
    <sizeX>109</sizeX>
    <sizeY>57</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artCreateStateMachine : artBaseCreateState
    {
        artProcessingWindow m_MessageWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artCreateStateMachine(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artCreateStateMachine_To_artDirectoryAlreadyExistsError("artDirectoryAlreadyExistsError"));
            m_ConditionalList.Add(new artCreateStateMachine_To_artDisplayStates("artDisplayStates"));
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
            m_MessageWindow = new artProcessingWindow("Artimech System Status", "Creating State Machine.....", 16, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 10);
            ArtimechEditor theScript = (ArtimechEditor)GetScriptableObject;

            //string pathAndFileNameForStateMachine = theScript.ConfigData.path

            string directoryName = theScript.ConfigData.CopyToDirectory.m_PathName +
                                    "/" +
                                    theScript.ConfigData.PrefixName +
                                    theScript.CurrentStateMachineName;



            if (Directory.Exists(directoryName))
            {
                //  Debug.Log(directoryName);
                CancelBool = true;
                return;
            }
            Directory.CreateDirectory(directoryName);

            string stateStartName = "";
            stateStartName = ReadReplaceAndWrite(
                        theScript.ConfigData.MasterScriptStateFile.m_PathAndName,
                        theScript.CurrentStateMachineName + "StartState",
                        theScript.ConfigData.CopyToDirectory.m_PathName,
                        directoryName + "/" + theScript.CurrentStateMachineName + "StartState.cs",
                        "stateEmptyExample",
                        theScript.ConfigData.PrefixName);

            Debug.Log("stateStartName = " + stateStartName);

            string stateMachName = "";
            stateStartName = ReadReplaceAndWrite(
                        theScript.ConfigData.MasterScriptStateFile.m_PathAndName,
                        theScript.CurrentStateMachineName,
                        theScript.ConfigData.CopyToDirectory.m_PathName,
                        directoryName + "/" + theScript.CurrentStateMachineName + ".cs",
                        "stateMachineTemplate",
                        theScript.ConfigData.PrefixName);

            theScript.Repaint();
            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            m_MessageWindow.AbortThread();
            base.Exit();
        }
    }
}
#endif