using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// A static class to help read and write files as well as do some search
/// search an replace.  TODO xml or json.
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
        string strBuff = "";
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            strBuff = streamReader.ReadToEnd();
        }
        fileStream.Close();
        return strBuff;
    }

    /// <summary>
    /// Used to replace sub a string in a file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="findText"></param>
    /// <param name="changeText"></param>
    public static void ReplaceTextInFile(string fileName,string findText,string changeText)
    {
  
        string strBuff = "";
        strBuff = LoadTextFromFile(fileName);
        string modText = strBuff.Replace(findText, changeText);

        StreamWriter writeStream = new StreamWriter(fileName);
        writeStream.Write(modText);
        writeStream.Close();
    }

    /// <summary>
    /// Finds the file of a same named class.  Complex imbedded classes won't be
    /// found since the name of the class and file have to match.
    /// </summary>
    /// <param name="className"></param>
    /// <param name="showInfo"></param>
    /// <returns>The filename with the associated directory.</returns>
    public static string FindPathAndFileByClassName(string className,bool showInfo=false)
    {
        string strBuff = "";

        string searchName = className + ".cs";
        string pathName = Application.dataPath;

        strBuff = FindAFileInADirectoryRecursively(pathName, searchName);

        if(showInfo)
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
            string[] words = file.Split(new char[] { '/','\\' });
            for(int i=0; i<words.Length;i++)
            {
                if (words[i] == findName)
                    return file;
            }
        }

        foreach (string dir in Directory.GetDirectories(startDir))
        {
            string strBuff = FindAFileInADirectoryRecursively(dir,findName);
            if (strBuff != null)
                return strBuff;
        }

        return null;
    }

}
