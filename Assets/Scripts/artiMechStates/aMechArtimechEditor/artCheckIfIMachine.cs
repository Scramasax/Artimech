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
    <alias>Check If State Machine</alias>
    <comment></comment>
    <posX>121</posX>
    <posY>52</posY>
    <sizeX>162</sizeX>
    <sizeY>47</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artCheckIfIMachine : editorStateBase
    {
        static Texture2D m_LoadingImage = null;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artCheckIfIMachine(Object unityObj) : base (unityObj)
        {
            InitImage();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artCheckIfIMachine_To_artChooseMachine("artChooseMachine"));
            m_ConditionalList.Add(new artCheckIfIMachine_To_artLoadStates("artLoadStates"));
            m_ConditionalList.Add(new artCheckIfIMachine_To_artNotEditorOrGameObject("artNotEditorOrGameObject"));
            m_ConditionalList.Add(new artCheckIfIMachine_To_artNoObject("artNoObject"));
            m_ConditionalList.Add(new artCheckIfIMachine_To_artAskToCreate("artAskToCreate"));
        }

        void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, "StartBackground.png");
            byte[] fileData;
            fileData = File.ReadAllBytes(fileAndPath);

            m_LoadingImage = null;
            m_LoadingImage = new Texture2D(512, 512);
            m_LoadingImage.LoadImage(fileData);
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
            GUILayout.Label(m_LoadingImage);
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

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