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
    public class editorRestoreState : editorBaseState
    {
        /// <summary>
        /// This state restores the previous gamestate and seleted object
        /// </summary>
        /// <param name="gameobject"></param>
        public editorRestoreState(GameObject gameobject) : base(gameobject)
        {
            m_UnityObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorRestoreToLoadConditional("Load"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
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
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            const string fileAndPath = "Assets/StateMachine.txt";
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);
            if (strBuff == null)
            {
                Debug.LogError("<color=maroon>" + "<b>" + "Restore file not found = " + "</b></color>" + "<color=red>" + "Assets/StateMachine.txt" + "</color>" + " .");
                return;
            }

            FileUtil.DeleteFileOrDirectory(fileAndPath);

            string[] words = strBuff.Split(new char[] { ',' });

            GameObject gameObject = utlDataAndFile.FindGameObjectByName(words[1]);
            if (gameObject == null)
            {
                Debug.LogError("<color=maroon>" + "<b>" + "Restore gameObject not found = " + "</b></color>" + "<color=red>" + words[1] + "</color>" + " .");
                return;
            }

            stateEditorUtils.SelectedObject = gameObject;
            stateEditorUtils.EditorCurrentGameObject = gameObject;
            stateEditorUtils.StateMachineName = words[0];

            GameObject gameObj = null;
            if (stateEditorUtils.SelectedObject is GameObject)
            {
                gameObj = (GameObject)stateEditorUtils.SelectedObject;
            }

            //this will allow only one statemachine per game object which isn't optimal.
            if (gameObj != null && gameObj.GetComponent<iMachineBase>() == null)
                gameObj.AddComponent(System.Type.GetType(stateEditorUtils.kArtimechNamespace + words[0]));

        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}
#endif