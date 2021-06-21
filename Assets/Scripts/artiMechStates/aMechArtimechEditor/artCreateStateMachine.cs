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
using UnityEditor;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Create Machine</alias>
    <comment></comment>
    <posX>307</posX>
    <posY>299</posY>
    <sizeX>131</sizeX>
    <sizeY>58</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artCreateStateMachine : artBaseCreateState
    {
        string m_StateMachineName = "";
        artProcessingWindow m_MessageWindow;

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artCreateStateMachine(Object unityObj) : base(unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artCreateStateMachine_To_artDirectoryAlreadyExistsError("artDirectoryAlreadyExistsError"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
      /*      ArtimechEditor theScript = (ArtimechEditor)GetScriptableObject;
            // if (System.Type.GetType(theScript.ConfigData.PrefixName + "." + m_StateMachineName) != null)
            if (System.Type.GetType(theScript.ConfigData.NamespaceName + "." + m_StateMachineName) != null)
            {
                Debug.Log("foundtype");
                if (stateEditorUtils.SelectedObject is GameObject)
                {
                    GameObject gameObj = (GameObject)stateEditorUtils.SelectedObject;
                    gameObj.AddComponent(System.Type.GetType(theScript.ConfigData.NamespaceName + "." + m_StateMachineName));
                }
            } */
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
            utlDataAndFile.SaveTextToFile(Application.dataPath + "/Resources/CreatedState.txt", "");
            m_StateMachineName = "";

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
                        directoryName + "/" + theScript.ConfigData.PrefixName + theScript.CurrentStateMachineName + "StartState.cs",
                        "stateEmptyExample",
                        theScript.ConfigData.PrefixName);

            //Debug.Log("stateStartName = " + stateStartName);

            string stateMachineFileName = directoryName + "/" + theScript.ConfigData.PrefixName + theScript.CurrentStateMachineName + ".cs";

            m_StateMachineName = ReadReplaceAndWrite(
                        theScript.ConfigData.MasterScripStateMachineFile.m_PathAndName,
                        theScript.CurrentStateMachineName,
                        theScript.ConfigData.CopyToDirectory.m_PathName,
                        stateMachineFileName,
                        "stateMachineTemplate",
                        theScript.ConfigData.PrefixName);

            Debug.Log("<b><color=white> m_StateMachineName = </color></b>" + m_StateMachineName);

            utlDataAndFile.ReplaceTextInFile(stateMachineFileName, "stateEmptyExample", stateStartName);

            //theScript.Repaint();
            base.Enter();

            OkBool = true;

            utlDataAndFile.SaveTextToFile(Application.dataPath + "/Resources/CreatedState.txt", theScript.ConfigData.NamespaceName + "." + m_StateMachineName);

            AssetDatabase.Refresh();
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
