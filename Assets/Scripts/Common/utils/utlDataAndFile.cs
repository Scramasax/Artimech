using System.IO;
using System.Text;


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

 
}
