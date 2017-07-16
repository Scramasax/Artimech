using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace artiMech
{
    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor.
    /// </summary>
    public static class stateEditorUtils
    {

        // A list of windows to be displayed
        static IList<stateWindowsNode> m_StateList = new List<stateWindowsNode>();

        // This is a list filled by scanning the source code of the 
        // generated statemachine and populating them with their class names.
        static IList<string> m_StateNameList = new List<string>();

        #region Accessors 
        public static IList<stateWindowsNode> StateList
        {
            get
            {
                return m_StateList;
            }

            set
            {
                m_StateList = value;
            }
        }

        #endregion

        /// <summary>
        /// This function is really more specific to the Artimech project and its 
        /// code generation system.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objectName"></param>
        /// <param name="pathName"></param>
        /// <param name="pathAndFileName"></param>
        /// <param name="findName"></param>
        /// <param name="replaceName"></param>
        /// <returns></returns>
        public static string ReadReplaceAndWrite(
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

        public static void CreateStateWindows(string fileName)
        {
            string strBuff = utlDataAndFile.LoadTextFromFile(fileName);
            PopulateStateStrings(strBuff);

            for(int i=0;i<m_StateNameList.Count;i++)
            {
                stateWindowsNode node = CreateStateWindowsNode(m_StateNameList[i]);
                m_StateList.Add(node);
            }
        }

        public static stateWindowsNode CreateStateWindowsNode(string typeName)
        {
            stateWindowsNode winNode = new stateWindowsNode(StateList.Count);
            winNode.WindowTitle = typeName;

            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;
            string winName = typeName;

//            TextAsset text = Resources.Load(typeName+".cs") as TextAsset;
            string strBuff = "";
            string fileName = "";
            fileName = utlDataAndFile.FindPathAndFileByClassName(typeName,true);
            strBuff = utlDataAndFile.LoadTextFromFile(fileName);
            string[] words = strBuff.Split(new char[] { '<', '>' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "name" && words[i + 1]!="nada")
                    winName = words[i + 1];
                if (words[i] == "posX")
                    x = Convert.ToSingle(words[i + 1]);
                if (words[i] == "posY")
                    y = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeX")
                    width = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeY")
                    height = Convert.ToSingle(words[i + 1]);
            }

            winNode.Set(winName, x, y, width, height);
            return winNode;
        }

        public static void PopulateStateStrings(string strBuff)
        {
            m_StateNameList.Clear();

            string[] words = strBuff.Split(new char[] { ' ', '(' });

            for(int i=0;i<words.Length;i++)
            {
                if (words[i] == "new")
                {
                    Type type = Type.GetType("artiMech." + words[i + 1]);

                    if (type != null)
                    {
                        string buffer = "";
                        buffer = type.BaseType.Name;
                        if (buffer == "baseState")
                        {
                            m_StateNameList.Add(words[i + 1]);
                            Debug.Log("<color=cyan>" + "<b>" + "words[i + 1] = " + "</b></color>" + "<color=grey>" + words[i + 1] + "</color>" + " .");
                        }

                    }
                }
            }
        }

        public static bool CreateAndAddStateCodeToProject(GameObject gameobject,string stateName)
        {

            string pathName = "Assets/Scripts/artiMechStates/";
            string FileName = "Assets/Scripts/Common/statemachine/stateTemplate.cs";

            string pathAndFileNameStartState = pathName
                                + "aMech"
                                + gameobject.name
                                + "/"
                                + stateName
                                + ".cs";

            if (File.Exists(pathAndFileNameStartState))
            {
                Debug.Log("<color=red>stateEditor.CreateStateMachine = </color> <color=blue> " + pathAndFileNameStartState + "</color> <color=red>Already exists and can't be overridden...</color>");
                return false;
            }

            //creates a start state from a template and populate aMech directory
            string stateStartName = stateEditorUtils.ReadReplaceAndWrite(FileName, stateName, pathName, pathAndFileNameStartState, "stateTemplate", "");

            return true;
        }

        public static bool AddStateCodeToStateMachineCode(string fileAndPath,string stateName)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";
            //AddState(new stateStartTemplate(this.gameObject), "stateStartTemplate", "new state change system");
            string insertString = "\n            AddState(new "
                                + stateName
                                + "(this.gameObject),"
                                + "\""
                                + stateName
                                + "\""
                                + ");";

            modStr = utlDataAndFile.InsertInFrontOf(strBuff, 
                                                    "<ArtiMechStates>",
                                                    insertString);

            utlDataAndFile.SaveTextToFile(fileAndPath, modStr);

            return true;
        }

        public static bool SetPositionAndSizeOfAStateFile(string fileName,int x, int y, int width, int height)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileName);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";
            modStr = utlDataAndFile.ReplaceBetween(strBuff, "<posX>", "</posX>",x.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<posY>", "</posY>", y.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<sizeX>", "</sizeX>", width.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<sizeY>", "</sizeY>", height.ToString());

            utlDataAndFile.SaveTextToFile(fileName, modStr);

            return true;
        }

        public static string GetCode(int number)
        {
            int start = (int)'A' - 1;
            if (number <= 26) return ((char)(number + start)).ToString();

            StringBuilder str = new StringBuilder();
            int nxt = number;

            List<char> chars = new List<char>();

            while (nxt != 0)
            {
                int rem = nxt % 26;
                if (rem == 0) rem = 26;

                chars.Add((char)(rem + start));
                nxt = nxt / 26;

                if (rem == 26) nxt = nxt - 1;
            }


            for (int i = chars.Count - 1; i >= 0; i--)
            {
                str.Append((char)(chars[i]));
            }

            return str.ToString();
        }
    }
}
