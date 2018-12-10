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
    public class editorLoadState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        IList<stateConditionalBase> m_ConditionalList;
        static Texture2D m_LoadingImage = null;

        public editorLoadState(GameObject gameobject)
        {
            m_UnityObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();

            //InitImage();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Load_To_Display("Display Windows"));
            m_ConditionalList.Add(new editor_Load_To_Wait("Wait"));
        }

        void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, "LoadingA.png");
            byte[] fileData;
            fileData = File.ReadAllBytes(fileAndPath);

            m_LoadingImage = null;
            m_LoadingImage = new Texture2D(512, 256);
            m_LoadingImage.LoadImage(fileData);
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
            GUILayout.Label(m_LoadingImage);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            iMachineBase machine = null;
            GameObject selectedGameObject = null;
            ScriptableObject editorObject = null;
            if (stateEditorUtils.SelectedObject is GameObject)
            {
                selectedGameObject = (GameObject)stateEditorUtils.SelectedObject;
            }
            if (stateEditorUtils.SelectedObject is ScriptableObject)
            {
                editorObject = (ScriptableObject)stateEditorUtils.SelectedObject;
            }

            //selectedObject = stateEditorUtils.SelectedObject;

            if (selectedGameObject != null)
                machine = selectedGameObject.GetComponent<iMachineBase>();

            if (editorObject != null)
                machine = (iMachineBase)editorObject;
            //machine = (iMachineBase)GetType();

            stateEditorUtils.StateList.Clear();

            //load states and their metadata
            if (machine != null)
            {
                //Debug.Log("<color=green>" + "<b>" + "machine type is = " + "</b></color>" + "<color=grey>" + machine.GetType().Name + "</color>" + " .");

                //remember what the state machine class name is.
                stateEditorUtils.StateMachineName = machine.GetType().Name;
                string strBuff = utlDataAndFile.FindPathAndFileByClassName(stateEditorUtils.StateMachineName, false);
                stateEditorUtils.CreateStateWindows(strBuff);
            }
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