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
using System;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Load States</alias>
    <comment></comment>
    <posX>470</posX>
    <posY>55</posY>
    <sizeX>104</sizeX>
    <sizeY>48</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artLoadStates : artBaseCreateState
    {
        artMessageWindow m_MessageWindow;
        //IList<string> m_ListOfStateStringsInMachine;
        bool m_Error = false;
        bool m_GoodLoad = false;

        public bool Error
        {
            get
            {
                return m_Error;
            }

            set
            {
                m_Error = value;
            }
        }

        public bool GoodLoad
        {
            get
            {
                return m_GoodLoad;
            }

            set
            {
                m_GoodLoad = value;
            }
        }

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artLoadStates(UnityEngine.Object unityObj) : base(unityObj)
        {
           // m_ListOfStateStringsInMachine = new List<string>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artLoadStates_To_artNotEditorOrGameObject("artNotEditorOrGameObject"));
            m_ConditionalList.Add(new artLoadStates_To_artDisplayStates("artDisplayStates"));
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
            m_MessageWindow.Update();
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

            Error = false;
            GoodLoad = false;
            m_ListOfStateStringsInMachine.Clear();
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            m_MessageWindow = new artMessageWindow("Artimech System Status", "Loading State Machine Scripts....", 14, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 4);
            theStateMachineEditor.Repaint();

            theStateMachineEditor.VisualStateNodes.Clear();

            GameObject gmObj = (GameObject) theStateMachineEditor.SelectedObj;
            stateMachineGame stateMachineScript = null;
            if(gmObj)
                stateMachineScript = gmObj.GetComponent<stateMachineGame>();
            /// <summary>Loads visualstates via meta data and code.</summary>
            if (stateMachineScript != null)
            {
                string machineSourceCodeText = utlDataAndFile.FindPathAndFileByClassName(theStateMachineEditor.MachineScript.GetType().Name, false);
                //Debug.Log("<color=navy>" + machineSourceCodeText + "</color>");
                CreateVisualStateNodes(machineSourceCodeText);
                if (!Error)
                    GoodLoad = true;
            }
            else
            {
                Error = true;
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