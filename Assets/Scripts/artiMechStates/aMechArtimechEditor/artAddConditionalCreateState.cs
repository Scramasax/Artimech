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
    <alias>Create Conditional</alias>
    <comment></comment>
    <posX>856</posX>
    <posY>287</posY>
    <sizeX>169</sizeX>
    <sizeY>43</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artAddConditionalCreateState : artDisplayWindowsBaseState
    {
        artProcessingWindow m_MessageWindow;
        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public artAddConditionalCreateState(Object unityObj) : base (unityObj)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new artAddConditionalCreateState_To_artDisplayStates("artDisplayStates"));
            m_ConditionalList.Add(new artAddConditionalCreateState_To_artAddConditionalCreateState("artAddConditionalCreateState"));
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
            m_MessageWindow = new artProcessingWindow("Artimech System Status", "Creating Conditional .....", 16, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 10);
            m_MessageWindow.GuiDepth = -1000;
            base.Enter();

            theStateMachineEditor.DrawToolBarBool = false;

            CreateConditionalAndAddToState(theStateMachineEditor.FromConditionalStateNode.ClassName, theStateMachineEditor.ToConditionalStateNode.ClassName,theStateMachineEditor.ConfigData.GetRefactorAndConstructionPaths());
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }

        bool AddConditionCodeToStateCode(string fileAndPath, string conditionName, string toStateName)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";
            //AddState(new stateStartTemplate(this.gameObject), "stateStartTemplate", "new state change system");
            string insertString = "\n            m_ConditionalList.Add(new "
                                + conditionName
                                + "("
                                + "\""
                                + toStateName
                                + "\""
                                + "));";

            //Debug.Log("changeName = " + changeName);

            modStr = utlDataAndFile.InsertInFrontOf(strBuff,
                                                    "<ArtiMechConditions>",
                                                    insertString);

            utlDataAndFile.SaveTextToFile(fileAndPath, modStr);



            return true;
        }

        artVisualStateNode FindStateWindowsNodeByName(string name)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            artVisualStateNode node = null;
            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                if (theStateMachineEditor.VisualStateNodes[i].ClassName == name)
                    return theStateMachineEditor.VisualStateNodes[i];
            }
            return node;
        }

        void CreateConditionalAndAddToState(string fromState, string toState, string[] paths)
        {
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            artConfigurationData.CopyConditionalInfo copyInfo = theStateMachineEditor.ConfigData.ConditionalCopyInfo[theStateMachineEditor.ConditionalSelectionIndex];

            string replaceName = fromState + "_To_" + toState;
            string templateConditonalPath = copyInfo.m_ConditionalSript.m_PathAndName;

            string text = utlDataAndFile.LoadTextFromFile(templateConditonalPath);
            string modText = text.Replace(copyInfo.m_ReplaceClassString, replaceName);

            //string pathAndFile = utlDataAndFile.FindPathAndFileByClassName(fromState);
            string pathAndFile = utlDataAndFile.FindPathAndFileByClassNameByDirectoryArray(fromState,paths);
            string outDir = Path.GetDirectoryName(pathAndFile);

            string pathAndFileName = outDir
                    + "/"
                    + replaceName
                    + ".cs";

            StreamWriter writeStream = new StreamWriter(pathAndFileName);
            writeStream.Write(modText);
            writeStream.Close();

            //            string fileAndPathOfState = utlDataAndFile.FindPathAndFileByClassName(fromState);
            string fileAndPathOfState = utlDataAndFile.FindPathAndFileByClassNameByDirectoryArray(fromState,paths);

            AddConditionCodeToStateCode(fileAndPathOfState, replaceName, toState);

            artVisualStateNode node = FindStateWindowsNodeByName(fromState);
            if (node != null)
            {
                for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
                {
                    if (theStateMachineEditor.VisualStateNodes[i].ClassName == toState)
                    {
                        node.ConditionLineList.Add(theStateMachineEditor.VisualStateNodes[i]);
                        return;
                    }
                }

            }

        }

        /*
        public static void CreateConditionalAndAddToState(string fromState, string toState)
        {


            string replaceName = fromState + "_To_" + toState;

            string text = utlDataAndFile.LoadTextFromFile(AddConditionPath);

            // int a = 12;

            string modText = text.Replace(AddConditionReplace, replaceName);

            string pathAndFile = utlDataAndFile.FindPathAndFileByClassName(fromState);
            string outDir = Path.GetDirectoryName(pathAndFile);

            string pathAndFileName = outDir
                    + "/"
                    + replaceName
                    + ".cs";

            StreamWriter writeStream = new StreamWriter(pathAndFileName);
            writeStream.Write(modText);
            writeStream.Close();

            string fileAndPathOfState = utlDataAndFile.FindPathAndFileByClassName(fromState);

            AddConditionCodeToStateCode(fileAndPathOfState, replaceName, toState);

            stateWindowsNode node = FindStateWindowsNodeByName(fromState);
            if (node != null)
            {
                for (int i = 0; i < m_StateList.Count; i++)
                {
                    if (m_StateList[i].ClassName == toState)
                    {
                        node.ConditionLineList.Add(m_StateList[i]);
                        return;
                    }
                }

            }

            //AssetDatabase.Refresh();
        }
*/
    }
}
#endif