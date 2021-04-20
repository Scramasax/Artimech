using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

#if UNITY_EDITOR
namespace Artimech
{
    public class artBaseCreateState : artBaseOkCancel
    {
        protected IList<string> m_ListOfStateStringsInMachine;
        public artBaseCreateState(UnityEngine.Object unityObj) : base(unityObj)
        {
            m_ListOfStateStringsInMachine = new List<string>();
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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

            base.Enter();
        }


        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        protected void CreateVisualStateNodes(string fileName)
        {
            string strBuff = utlDataAndFile.LoadTextFromFile(fileName);
            this.PopulateStateStrings(strBuff);
            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;

            for (int i = 0; i < m_ListOfStateStringsInMachine.Count; i++)
            {
                artVisualStateNode node = CreateVisualStateNode(m_ListOfStateStringsInMachine[i]);
                theStateMachineEditor.VisualStateNodes.Add(node);
            }

            for (int i = 0; i < theStateMachineEditor.VisualStateNodes.Count; i++)
            {
                string stateFileName = utlDataAndFile.FindPathAndFileByClassName(theStateMachineEditor.VisualStateNodes[i].ClassName, false);
                string buffer = utlDataAndFile.LoadTextFromFile(stateFileName);
                PopulateLinkedConditionStates(theStateMachineEditor.VisualStateNodes[i], buffer);
            }
        }

        protected artVisualStateNode CreateVisualStateNode(string typeName)
        {

            ArtimechEditor theStateMachineEditor = (ArtimechEditor)GetScriptableObject;
            artVisualStateNode visualNode = new artVisualStateNode(theStateMachineEditor.VisualStateNodes.Count + 10000);
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
        protected void PopulateLinkedConditionStates(artVisualStateNode node, string strBuff)
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
        protected artVisualStateNode FindStateWindowsNodeByName(string name)
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

        protected string ReadReplaceAndWrite(
                    string fileName,
                    string objectName,
                    string pathName,
                    string pathAndFileName,
                    string findName,
                    string replaceName)
        {

            string text = utlDataAndFile.LoadTextFromFile(fileName);

            string changedName = replaceName + objectName;
            string modText = text.Replace(findName, changedName);

            StreamWriter writeStream = new StreamWriter(pathAndFileName);
            writeStream.Write(modText);
            writeStream.Close();

            return changedName;
        }

    }
}
#endif