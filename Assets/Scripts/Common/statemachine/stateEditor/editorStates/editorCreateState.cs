/// Artimech
/// 
/// Copyright � <2017-2018> <George A Lancaster>
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
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class editorCreateState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        //IList<stateConditionalBase> m_ConditionalList;

        public editorCreateState(GameObject gameobject)
        {
            m_UnityObject = gameobject;
            //m_ConditionalList = new List<stateConditionalBase>();

            //<ArtiMechConditions>
            //m_ConditionalList.Add(new editorCreateToDisplayConditional("Display Windows"));

        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            if (System.Type.GetType("artiMech." + stateEditorUtils.StateMachineName) != null)
            {
                if (stateEditorUtils.SelectedObject is GameObject)
                {
                    GameObject gameObj = (GameObject)stateEditorUtils.SelectedObject;
                    gameObj.AddComponent(System.Type.GetType(stateEditorUtils.kArtimechNamespace + stateEditorUtils.StateMachineName));
                }
               // stateEditorUtils.SelectedObject.AddComponent(System.Type.GetType(stateEditorUtils.kArtimechNamespace + stateEditorUtils.StateMachineName));
                Debug.Log(
                            "<b><color=navy>Artimech Report Log Section B\n</color></b>"
                            + "<i><color=grey>Click to view details</color></i>"
                            + "\n"
                            + "<color=blue>Added a statemachine </color><b>"
                            + stateEditorUtils.StateMachineName
                            + "</b>"
                            + "<color=blue> to a gameobject named </color>"
                            + stateEditorUtils.SelectedObject.name
                            + " .\n\n");
            }

            /*
            for (int i = 0; i < m_ConditionalList.Count; i++)
            {
                string changeNameToThisState = null;
                changeNameToThisState = m_ConditionalList[i].UpdateConditionalTest(this);
                if (changeNameToThisState != null)
                {
                    m_ChangeStateName = changeNameToThisState;
                    m_ChangeBool = true;
                    return;
                }
            }
            */
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {

        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void LateUpdate()
        {

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {

        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            stateEditorUtils.CreateStateMachineScriptAndStartState();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            if (stateEditorUtils.SelectedObject == null)
            {
                if (stateEditorUtils.WasSelectedObject != stateEditorUtils.SelectedObject)
                    stateEditorUtils.StateList.Clear();
                //sets the 'was' gameobject so as to dectect a gameobject swap.
                stateEditorUtils.WasSelectedObject = stateEditorUtils.SelectedObject;
            }
        }
    }
}
#endif