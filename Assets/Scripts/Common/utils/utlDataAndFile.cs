using System.IO;
using System.Text;


/// <summary>
/// A static class to help read and write files as well as do some search
/// search an replace.  TODO xml or json.
/// </summary>
public static class utlDataAndFile
{
    public static string ReadReplaceAndWrite(
                        string fileName,
                        string objectName,
                        string pathName, 
                        string pathAndFileName,
                        string findName,
                        string replaceName)
    {
        string text = "";
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            text = streamReader.ReadToEnd();
        }
        fileStream.Close();

        string changedName = replaceName + objectName;
        string modText = text.Replace(findName, changedName);

        string directoryName = pathName + replaceName + objectName;

        Directory.CreateDirectory(directoryName);

        StreamWriter writeStream = new StreamWriter(pathAndFileName);

        writeStream.Write(modText);

        writeStream.Close();

        return changedName;
    }
}
