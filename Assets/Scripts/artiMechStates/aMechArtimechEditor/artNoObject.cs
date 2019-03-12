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
    <alias>No Object</alias>
    <comment></comment>
    <posX>-187</posX>
    <posY>184</posY>
    <sizeX>104</sizeX>
    <sizeY>55</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artNoObject : editorStateBase
    {
        static Texture2D m_LoadingImage = null;
        //artWindowBase m_TestWin;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artNoObject(Object unityObj) : base (unityObj)
        {
            //m_TestWin = new artWindowBase("test",new Rect(100,100,100,100),new Color(1,1,1,0.8f),1);
            InitImage();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artNoObject_To_artConfigure("artConfigure"));
            m_ConditionalList.Add(new artNoObject_To_artCheckIfIMachine("artCheckIfIMachine"));
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
            //m_TestWin.Update();
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
            //         m_TestWin.Draw(1);
            GUILayout.Label(m_LoadingImage);
 //           m_TestWin.Update();
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            ArtimechEditor.Inst.SaveConfigureBool = false;
            ArtimechEditor.Inst.DrawToolBarBool = true;
            ArtimechEditor.Inst.Repaint();
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