using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


public class utlXmlLoadSave<T>
{
    string m_FileName;
    string m_FileLocation;

    T m_Value;
    string m_Data;
    public utlXmlLoadSave(T t)
    {
        // The field has the same type as the parameter.
        this.m_Value = t;
    }
    protected void Save()
    {
     //   m_Data = SerializeObject(m_Value);

        StreamWriter streamWriter;
        FileInfo fileInfo = new FileInfo(m_FileLocation + "\\" + m_FileName);
        if(!fileInfo.Exists)
        {
            streamWriter = fileInfo.CreateText();
        }
        else
        {
            fileInfo.Delete();
            streamWriter = fileInfo.CreateText();
        }
        streamWriter.Write(typeof(T));
        streamWriter.Close();

    }

    protected void Load()
    {
        StreamReader streamReader = File.OpenText(m_FileLocation + "\\" + m_FileName);
        string _info = streamReader.ReadToEnd();
        streamReader.Close();
        m_Data = _info;
        //Debug.Log("File Read");
    }

}
