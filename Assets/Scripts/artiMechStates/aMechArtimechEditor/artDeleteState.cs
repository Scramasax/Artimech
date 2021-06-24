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
    <alias>Delete State</alias>
    <comment></comment>
    <posX>609</posX>
    <posY>572</posY>
    <sizeX>106</sizeX>
    <sizeY>40</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artDeleteState : editorStateBase
    {
        artProcessingWindow m_MessageWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artDeleteState(Object unityObj) : base (unityObj)
        {
            //<ArtiMechConditions>
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
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            m_MessageWindow = new artProcessingWindow("Artimech System Status", "Deleting State .....", 16, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 10);

            base.Enter();

            theStateMachineEditor.DrawToolBarBool = false;

            DeleteAndRemoveStateAndConditionals(theStateMachineEditor.DeleteStateClass);
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }

        void DeleteAndRemoveStateAndConditionals(string className)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            theStateMachineEditor.SaveMetaDataInStates();
            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                if(theStateMachineEditor.VisualStateNodes[i].ClassName==className)
                {
                    theStateMachineEditor.VisualStateNodes.Remove(theStateMachineEditor.VisualStateNodes[i]);
                    break;
                }
            }

            //string pathAndFileForClassName = utlDataAndFile.FindPathAndFileByClassName(className);
            string pathAndFileForClassName = utlDataAndFile.FindPathAndFileByClassNameByDirectoryArray(className, theStateMachineEditor.ConfigData.GetRefactorAndConstructionPaths());
            File.Delete(pathAndFileForClassName);

            utlDataAndFile.RemoveLinesBySubStringInFiles(className, Application.dataPath);

            var listOfFiles = utlDataAndFile.GetFilesBySearchPattern(Application.dataPath, "*" + className + "*", SearchOption.AllDirectories);
            for (int i = 0; i < listOfFiles.Length; i++)
            {
                //Debug.Log(listOfFiles[i]);
                File.Delete(listOfFiles[i]);
            }

            AssetDatabase.Refresh();

        }
    }
}
