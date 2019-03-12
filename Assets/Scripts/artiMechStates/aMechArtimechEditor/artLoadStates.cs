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
    <posX>356</posX>
    <posY>68</posY>
    <sizeX>104</sizeX>
    <sizeY>48</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class artLoadStates : editorStateBase
    {
        artMessageWindow m_MessageWindow;
        IList<string> m_ListOfStateStringsInMachine;
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
            m_ListOfStateStringsInMachine = new List<string>();
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
            m_MessageWindow = new artMessageWindow("Artimech System Status", "Loading State Machine Scripts....", 14, Color.blue, new Rect(0, 18, Screen.width, Screen.height), new Color(1, 1, 1, 1), 4);
            ArtimechEditor.Inst.Repaint();

            ArtimechEditor.Inst.VisualStateNodes.Clear();

            /// <summary>Loads visualstates via meta data and code.</summary>
            if (ArtimechEditor.Inst.MachineScript != null)
            {
                string machineSourceCodeText = utlDataAndFile.FindPathAndFileByClassName(ArtimechEditor.Inst.MachineScript.GetType().Name, false);
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
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        void CreateVisualStateNodes(string fileName)
        {
            string strBuff = utlDataAndFile.LoadTextFromFile(fileName);
            this.PopulateStateStrings(strBuff);

            for (int i = 0; i < m_ListOfStateStringsInMachine.Count; i++)
            {
                artVisualStateNode node = CreateVisualStateNode(m_ListOfStateStringsInMachine[i]);
                ArtimechEditor.Inst.VisualStateNodes.Add(node);
            }

            for (int i = 0; i < ArtimechEditor.Inst.VisualStateNodes.Count; i++)
            {
                string stateFileName = utlDataAndFile.FindPathAndFileByClassName(ArtimechEditor.Inst.VisualStateNodes[i].ClassName, false);
                string buffer = utlDataAndFile.LoadTextFromFile(stateFileName);
                PopulateLinkedConditionStates(ArtimechEditor.Inst.VisualStateNodes[i], buffer);
            }
        }

        artVisualStateNode CreateVisualStateNode(string typeName)
        {
            artVisualStateNode visualNode = new artVisualStateNode(ArtimechEditor.Inst.VisualStateNodes.Count+10000);
            visualNode.ClassName = typeName;

            float posX = 0;
            float posY = 0;
            float width = 0;
            float height = 0;
            string winName = typeName;
            string strBuff = "";
            string fileName = "";

            fileName = utlDataAndFile.FindPathAndFileByClassName(typeName, false);
            strBuff = utlDataAndFile.LoadTextFromFile(fileName);

            string[] words = strBuff.Split(new char[] { '<', '>' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "alias")
                    winName = words[i + 1];
                if (words[i] == "posX")
                    posX = Convert.ToSingle(words[i + 1]);
                if (words[i] == "posY")
                    posY = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeX")
                    width = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeY")
                    height = Convert.ToSingle(words[i + 1]);
            }

            visualNode.Set(fileName, typeName, winName, posX, posY, width, height);

            return visualNode;
        }

        /// <summary>
        /// Parse the conditions from the state c sharp file.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="strBuff"></param>
        void PopulateLinkedConditionStates(artVisualStateNode node, string strBuff)
        {
            string[] words = strBuff.Split(new char[] { ' ', '/', '\n', '\r', '_', '(' });
            bool lookForConditionals = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "<ArtiMechConditions>")
                {
                    lookForConditionals = true;
                }

                if (lookForConditionals && words[i] == "new")
                {
                    //check to see if stateConditionalBase
                    Type type = Type.GetType(stateEditorUtils.kArtimechNamespace + words[i + 3]);
                    if (type != null)
                    {
                        string base1Str = "";
                        string base2Str = "";
                        string base3Str = "";
                        string base4Str = "";
                        string base5Str = "";
                        string base6Str = "";
                        string base7Str = "";


                        if (type.BaseType != null)
                        {
                            base1Str = type.BaseType.Name;
                            if (type.BaseType.BaseType != null)
                            {
                                base2Str = type.BaseType.BaseType.Name;
                                if (type.BaseType.BaseType.BaseType != null)
                                {
                                    base3Str = type.BaseType.BaseType.BaseType.Name;
                                    if (type.BaseType.BaseType.BaseType.BaseType != null)
                                    {
                                        base4Str = type.BaseType.BaseType.BaseType.BaseType.Name;
                                        if (type.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                        {
                                            base5Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                            if (type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                            {
                                                base6Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                                if (type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                                    base7Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //if (baseOneStr == "baseState" )//|| buffer == "stateGameBase")
                        if (base1Str == "baseState" || base2Str == "baseState" || base3Str == "baseState" || base4Str == "baseState" || base5Str == "baseState" || base6Str == "baseState" || base7Str == "baseState")
                        {
                            artVisualStateNode compNode = FindStateWindowsNodeByName(words[i + 3]);
                            if (compNode != null)
                            {
                                node.ConditionLineList.Add(compNode);
                                //Debug.Log("compNode = " + compNode.ClassName);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        artVisualStateNode FindStateWindowsNodeByName(string name)
        {
            artVisualStateNode node = null;
            for (int i = 0; i < ArtimechEditor.Inst.VisualStateNodes.Count; i++)
            {
                if (ArtimechEditor.Inst.VisualStateNodes[i].ClassName == name)
                    return ArtimechEditor.Inst.VisualStateNodes[i];
            }
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBuff"></param>
        public void PopulateStateStrings(string strBuff)
        {
            //m_StateNameList.Clear();

            string[] words = strBuff.Split(new char[] { ' ', '(' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "new")
                {
                    Type type = Type.GetType(stateEditorUtils.kArtimechNamespace + words[i + 1]);

                    if (type != null)
                    {
                        string baseOneStr = "";
                        string baseTwoStr = "";
                        string baseThreeStr = "";

                        baseOneStr = type.BaseType.Name;
                        if (type.BaseType.BaseType != null)
                        {
                            baseTwoStr = type.BaseType.BaseType.Name;
                            if (type.BaseType.BaseType.BaseType != null)
                                baseThreeStr = type.BaseType.BaseType.BaseType.Name;
                        }

                        //if (baseOneStr == "baseState" )//|| buffer == "stateGameBase")
                        if (baseOneStr == "baseState" || baseTwoStr == "baseState" || baseThreeStr == "baseState")
                        {
                            m_ListOfStateStringsInMachine.Add(words[i + 1]);
                            // Debug.Log("<color=cyan>" + "<b>" + "words[i + 1] = " + "</b></color>" + "<color=grey>" + words[i + 1] + "</color>" + " .");
                        }

                    }
                }
            }
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
