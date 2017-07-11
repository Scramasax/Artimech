using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
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
        static IList<string> m_StateName = new List<string>();

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

            for(int i=0;i<m_StateName.Count;i++)
            {
                m_StateList.Add(CreateStateWindowsNode(m_StateName[i]));
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

            TextAsset text = Resources.Load(typeName+".cs") as TextAsset;
            string strBuff = text.ToString();

            string[] words = strBuff.Split(new char[] { ' ', '<', '>' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "posX")
                    x = Int32.Parse(words[i + 1]);
                if (words[i] == "posY")
                    y = Int32.Parse(words[i + 1]);
                if (words[i] == "sizeX")
                    width = Int32.Parse(words[i + 1]);
                if (words[i] == "sizeY")
                    height = Int32.Parse(words[i + 1]);
            }

            winNode.WinRect.Set(x, y, width, height);
            return winNode;
        }

        public static void PopulateStateStrings(string strBuff)
        {
            m_StateName.Clear();

            //string strBuff = utlDataAndFile.LoadTextFromFile(fileName);

            string[] words = strBuff.Split(new char[] { ' ', '(' });

            string tokenA = "CreateStates";
            string tokenB = "new";

            bool scanForStatesBool = false;

            for(int i=0;i<words.Length;i++)
            {
                //only scan in after the CreateStates function.
                if(words[i]==tokenA)
                {
                    scanForStatesBool = true;
                }

                if(words[i]==tokenB && scanForStatesBool)
                {
                    m_StateName.Add(words[i+1]);
                }
            }

        }
    }

}
