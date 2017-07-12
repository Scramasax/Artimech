using System;
using System.Collections.Generic;
using System.IO;
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
            stateWindowsNode winNode = new stateWindowsNode();
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
                    Type type = Type.GetType(words[i + 1]);

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
    }
}
