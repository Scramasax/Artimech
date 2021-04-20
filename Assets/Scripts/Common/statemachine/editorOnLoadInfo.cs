using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Artimech
{
    [Serializable]
    public class editorOnLoadInfo 
    {
        public string m_NameOfConfigData;
        public Vector3 m_CurrentWindowPosition;

        public void SaveToFile(string fileName)
        {
            string data = JsonUtility.ToJson(this);
            utlDataAndFile.SaveTextToFile(fileName, data);
        }

        public bool LoadFromFile(string fileName)
        {
            string data = utlDataAndFile.LoadTextFromFile(fileName);

            if (data == null)
                return false;

            editorOnLoadInfo loadInfo = CreateFromJSON(data);
            this.m_NameOfConfigData = loadInfo.m_NameOfConfigData;
            this.m_CurrentWindowPosition = loadInfo.m_CurrentWindowPosition;

            return true;
        }

        public static editorOnLoadInfo CreateFromJSON(string jsonString)
        {
            //object boxedStruct = new editorOnLoadInfo();

            //return EditorJsonUtility.FromJsonOverwrite(jsonString, boxedStruct);
            return JsonUtility.FromJson<editorOnLoadInfo>(jsonString);
        }
    }
}
#endif