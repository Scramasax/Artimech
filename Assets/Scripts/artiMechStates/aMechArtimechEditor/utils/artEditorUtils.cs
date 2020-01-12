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

        /*
        public static void CreateStateContextCallback(ArtimechEditor theArtimechEditor, object obj)
        {
            //make the passed object to a string
            //string clb = obj.ToString();
            editorDisplayWindowsState.menuData menuData = (editorDisplayWindowsState.menuData)obj;


            if (theArtimechEditor.SelectedObj != null)
            {
                theArtimechEditor.SaveMetaDataInStates();

                if (theArtimechEditor.StateList.Count == 0)
                {
                    Debug.LogError("StateList is Empty so you can't create a state.");
                    return;
                }
                //string stateName = "aMech" + GameObject.name + "State" + utlDataAndFile.GetCode(StateList.Count);

                int codeIndex = theArtimechEditor.StateList.Count;
                string stateName = theArtimechEditor.ConfigData.PrefixName + theArtimechEditor.name + theArtimechEditor.ConfigData.PostfixName + utlDataAndFile.GetCode(codeIndex);

                UnityEngine.Object unityObj = null;
                if (stateEditorUtils.SelectedObject is UnityEngine.Object)
                {
                    unityObj = (UnityEngine.Object)stateEditorUtils.SelectedObject;
                }

                while (!CreateAndAddStateCodeToProject(unityObj, stateName, menuData.m_FileAndPath, menuData.m_ReplaceName, false))
                {
                    codeIndex += 1;
                    stateName = theArtimechEditor.ConfigData.PrefixName + theArtimechEditor.name + theArtimechEditor.ConfigData.PostfixName + utlDataAndFile.GetCode(codeIndex);

                    //geohack: sanity check
                    if (codeIndex > 10000)
                        return;
                }


                string fileAndPath = "";
                fileAndPath = utlDataAndFile.FindPathAndFileByClassName(stateName);

                //stateWindowsNode windowNode = new stateWindowsNode(stateEditorUtils.StateList.Count);

                Event ev = Event.current;
                Vector3 transMousePos = new Vector3();
                transMousePos = stateEditorUtils.TranslationMtx.UnTransform(ev.mousePosition);

                windowNode.Set(fileAndPath, stateName, stateName, transMousePos.x, transMousePos.y, 150, 80);
                theArtimechEditor.StateList.Add(windowNode);

                SaveStateWindowsNodeData(fileAndPath, stateName, (int)MousePos.x, (int)MousePos.y, 150, 80);

                fileAndPath = utlDataAndFile.FindPathAndFileByClassName(StateMachineName);

                SaveStateInfo(StateMachineName, SelectedObject.name);

                AddStateCodeToStateMachineCode(fileAndPath, stateName);

            }
        }*/

    }
}
