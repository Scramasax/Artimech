using System.Collections.Generic;
using System.IO;
using System.Text;

namespace artiMech
{
    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor.
    /// </summary>
    public static class stateEditorUtils
    {

        static IList<stateWindowsNode> m_StateList = new List<stateWindowsNode>();

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
    }
}
