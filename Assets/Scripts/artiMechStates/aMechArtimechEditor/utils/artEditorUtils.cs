using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Artimech
{
    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor.
    /// </summary>
    public static class artEditorUtils
    {
        /// <summary>Used class matching via parsing for namespace.</summary>
        public static string kArtimechNamespace = "Artimech.";

        /// <summary>
        /// Saves the meta data of a state.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="titleAlias"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>true if the file is found</returns>
        public static bool SaveStateMetaData(string fileName, string titleAlias, int x, int y, int width, int height)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileName);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";
            modStr = utlDataAndFile.ReplaceBetween(strBuff, "<alias>", "</alias>", titleAlias);
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<posX>", "</posX>", x.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<posY>", "</posY>", y.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<sizeX>", "</sizeX>", width.ToString());
            modStr = utlDataAndFile.ReplaceBetween(modStr, "<sizeY>", "</sizeY>", height.ToString());

            utlDataAndFile.SaveTextToFile(fileName, modStr);

            return true;
        }

    }
}
