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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// A static class to help read and write files as well as do some search
/// search an replace.  Some misc string functions. TODO xml or json.
/// </summary>
public static class utlDataAndFile
{
    /// <summary>
    /// Load text from a file and returns that text.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string LoadTextFromFile(string fileName)
    {
        string strBuff = null;
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            strBuff = streamReader.ReadToEnd();
        }
        fileStream.Close();
        return strBuff;
    }

    /// <summary>
    /// Saves a string to a file.  File argument needs to be provided via the argument 'string fileName'
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    public static void SaveTextToFile(string fileName, string fileContent)
    {
        StreamWriter writeStream = new StreamWriter(fileName);
        writeStream.Write(fileContent);
        writeStream.Close();
    }

    /// <summary>
    /// Used to replace sub a string in a file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="findText"></param>
    /// <param name="changeText"></param>
    public static void ReplaceTextInFile(string fileName, string findText, string changeText)
    {

        string strBuff = "";
        strBuff = LoadTextFromFile(fileName);
        string modText = strBuff.Replace(findText, changeText);

        StreamWriter writeStream = new StreamWriter(fileName);
        writeStream.Write(modText);
        writeStream.Close();
    }

    /// <summary>
    /// Removes the line in a file using a sub string.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="findText"></param>
    public static void RemoveLineInFile(string fileName, string findText)
    {
        int lineIndex = 0;
        bool foundText = false;
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line = "";

            while ((line = reader.ReadLine()) != null)
            {
                lineIndex += 1;
                if (line.IndexOf(findText) >= 0)
                {
                    foundText = true;
                    break;
                }
            }
        }

        if (!foundText)
            return;

        var file = new List<string>(System.IO.File.ReadAllLines(fileName));
        file.RemoveAt(lineIndex - 1);
        File.WriteAllLines(fileName, file.ToArray());
    }

    /// <summary>
    /// Finds the file of a same named class.  Complex imbedded classes won't be
    /// found since the name of the class and file have to match.
    /// </summary>
    /// <param name="className"></param>
    /// <param name="showInfo"></param>
    /// <returns>The filename with the associated directory.</returns>
    public static string FindPathAndFileByClassName(string className, bool showInfo = false)
    {
        string strBuff = "";

        string searchName = className + ".cs";
        string pathName = Application.dataPath;

        strBuff = FindAFileInADirectoryRecursively(pathName, searchName);

        if (showInfo)
            Debug.Log("<color=blue>" + "<b>" + "Class was found at = " + "</b></color>" + "<color=grey>" + strBuff + "</color>" + " .");

        return strBuff;
    }

    /// <summary>
    /// Recursive file search using a file name.
    /// </summary>
    /// <param name="startDir"></param>
    /// <param name="findName"></param>
    /// <returns>The file name and directory.</returns>
    public static string FindAFileInADirectoryRecursively(string startDir, string findName)
    {
        foreach (string file in Directory.GetFiles(startDir))
        {
            string[] words = file.Split(new char[] { '/', '\\' });
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == findName)
                    return file;
            }
        }

        foreach (string dir in Directory.GetDirectories(startDir))
        {
            string strBuff = FindAFileInADirectoryRecursively(dir, findName);
            if (strBuff != null)
                return strBuff;
        }

        return null;
    }

    /// <summary>
    /// Returns a string between two str tokens.
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strStart"></param>
    /// <param name="strEnd"></param>
    /// <returns></returns>
    public static string GetBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return null;
    }

    /// <summary>
    /// Returns a string that has had a string inserted from a str token.
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strStart"></param>
    /// <param name="strInsert"></param>
    /// <returns></returns>
    public static string InsertInFrontOf(string strSource, string strStart, string strInsert)
    {
        string strOut = "";
        int startIndex = 0;

        if (strSource.Contains(strStart))
        {
            startIndex = strSource.IndexOf(strStart, 0) + strStart.Length;

            StringBuilder stringBuilder = new StringBuilder(strSource);
            stringBuilder.Insert(startIndex, strInsert);
            strOut = stringBuilder.ToString();
            return strOut;
        }

        return null;
    }

    /// <summary>
    /// Returns a string with find and replace between two str tokens.
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strStart"></param>
    /// <param name="strEnd"></param>
    /// <param name="strReplace"></param>
    /// <returns>string</returns>
    public static string ReplaceBetween(string strSource, string strStart, string strEnd, string strReplace)
    {
        int Start, End;
        string strOut = "";
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);

            StringBuilder stringBuilder = new StringBuilder(strSource);
            stringBuilder.Remove(Start, End - Start);
            stringBuilder.Insert(Start, strReplace);
            strOut = stringBuilder.ToString();
            return strOut;
        }

        return null;
    }

    /// <summary>
    /// Searches the UnityEngine for an object that matches the 'strIn' parameter
    /// </summary>
    /// <param name="strIn"></param>
    /// <returns>GameObject</returns>
    public static GameObject FindGameObjectByName(string strIn)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].name == strIn)
                return allObjects[i];
        }
        return null;
    }

    /// <summary>
    /// Returns an alpha from a number.
    /// </summary>
    /// <param name="numIn"></param>
    /// <returns>string</returns>
    public static string GetCode(int numIn)
    {
        int start = (int)'A' - 1;
        if (numIn <= 26) return ((char)(numIn + start)).ToString();

        StringBuilder strBuilder = new StringBuilder();
        int nextNum = numIn;

        List<char> listOfChars = new List<char>();

        while (nextNum != 0)
        {
            int remainder = nextNum % 26;
            if (remainder == 0) remainder = 26;

            listOfChars.Add((char)(remainder + start));
            nextNum = nextNum / 26;

            if (remainder == 26) nextNum = nextNum - 1;
        }


        for (int i = listOfChars.Count - 1; i >= 0; i--)
        {
            strBuilder.Append((char)(listOfChars[i]));
        }

        return strBuilder.ToString();
    }

    /// <summary>
    /// Load a png.
    /// </summary>
    /// <param name="fileAndPath"></param>
    /// <returns></returns>
    public static Texture2D LoadPNG(string fileAndPath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(fileAndPath))
        {
            fileData = File.ReadAllBytes(fileAndPath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;

    }

    /// <summary>
    /// Finds filenames that match and replace them.
    /// </summary>
    /// <param name="startDir"></param>
    /// <param name="findName"></param>
    /// <returns></returns>
    public static string ReplaceAFileNamesBySubStringRecursively(string startDir, string findName, string replaceName, bool showDebugInfo = false)
    {
        foreach (string file in Directory.GetFiles(startDir))
        {
            if (file.IndexOf(findName) != -1)
            {
                string oldFileName = file;
                string moveFileName = file.Replace(findName, replaceName);
                File.Move(oldFileName, moveFileName);
            }

            /*
            string[] words = file.Split(new char[] { '/', '\\' });
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == findName)
                    return file;
            }*/
        }

        foreach (string dir in Directory.GetDirectories(startDir))
        {
            string strBuff = ReplaceAFileNamesBySubStringRecursively(dir, findName, replaceName);
            if (strBuff != null)
                return strBuff;
        }

        return null;
    }

    /// <summary>
    /// Finds files that match the fileFindName recursively.  From there is starts to replace file contents
    /// that use the fineName and the replace name as their arguments.
    /// </summary>
    /// <param name="startDir"></param>
    /// <param name="fileFindName"></param>
    /// <param name="findName"></param>
    /// <param name="replaceName"></param>
    /// <param name="showDebugInfo"></param>
    /// <returns></returns>
    public static string ReplaceNamesInFilesRecursively(string startDir, string fileFindName, string findName, string replaceName, bool showDebugInfo = false)
    {
        foreach (string file in Directory.GetFiles(startDir))
        {
            if (file.IndexOf(fileFindName) != -1)
            {
                ReplaceTextInFile(file, findName, replaceName);
            }
        }

        foreach (string dir in Directory.GetDirectories(startDir))
        {
            string strBuff = ReplaceNamesInFilesRecursively(dir, fileFindName, findName, replaceName, showDebugInfo);
            if (strBuff != null)
                return strBuff;
        }

        return null;
    }

    public static void RefactorAllAssets(string searchStr, string renameStr, string pathStr = null, bool showDebugInfo = false)
    {
        string pathLocation = Application.dataPath;
        if (pathStr != null)
            pathLocation = pathStr;

        ReplaceAFileNamesBySubStringRecursively(pathLocation, searchStr, renameStr, showDebugInfo);
        ReplaceNamesInFilesRecursively(pathLocation, ".cs", searchStr, renameStr, showDebugInfo);
    }

    public static string RemoveLinesInFileRecursively(string startDir, string fileFindName, string findName, bool showDebugInfo = false)
    {
        foreach (string file in Directory.GetFiles(startDir))
        {
            if (file.IndexOf(fileFindName) != -1)
            {
                //ReplaceTextInFile(file, findName, replaceName);
                RemoveLineInFile(file, findName);
            }
        }

        foreach (string dir in Directory.GetDirectories(startDir))
        {
            string strBuff = RemoveLinesInFileRecursively(dir, fileFindName, findName, showDebugInfo);
            if (strBuff != null)
                return strBuff;
        }

        return null;
    }

    public static void RemoveLinesBySubStringInFiles(string subStr, string pathStr = null, bool showDebugInfo = false)
    {
        string pathLocation = Application.dataPath;
        if (pathStr != null)
            pathLocation = pathStr;

        RemoveLinesInFileRecursively(pathLocation, ".cs", subStr, showDebugInfo);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public static IList<string> GetListOfFilesInDirctory(string directory, string findName, bool showDebugInfo = false)
    {
        IList<string> listOfStrings = new List<string>();

        foreach (string file in Directory.GetFiles(directory))
        {
            if (file.IndexOf(findName) != -1)
            {
                listOfStrings.Add(null);

                listOfStrings[listOfStrings.Count - 1] = file;
                if (showDebugInfo)
                    Debug.Log("dir files = " + file);
            }
        }
        return listOfStrings.Distinct().ToList();
    }

}