#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;



namespace Artimech
{
    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor.
    /// </summary>
    public static class stateEditorUtils
    {
        /// <summary>Used class matching via parsing for namespace.</summary>
        public static string kArtimechNamespace = "Artimech.";

        // A list of windows to be displayed
        static IList<stateWindowsNode> m_StateList = new List<stateWindowsNode>();

        // This is a list filled by scanning the source code of the 
        // generated statemachine and populating them with their class names.
        static IList<string> m_StateNameList = new List<string>();

        static UnityEngine.Object m_EditorCurrentGameObject = null;

        static Vector3 m_MousePos;

        static UnityEngine.Object m_SelectedObject = null;
        static UnityEngine.Object m_WasSelectedObject = null;
        static string m_StateMachineName = "";

        static stateWindowsNode m_SelectedWindowsNode = null;

        static stateConditionalBase m_SelectedConditional = null;

        static stateEditor m_StateEditor = null;

        static string m_AddConditionPath = "";
        static string m_AddConditionReplace = "";

        static string m_DeleteConditionalPath = "";
        static string m_DeleteClassName = "";

        static utlMatrix34 m_TranslationMtx = new utlMatrix34();

        static bool m_bVerbose = false;

        #region Accessors 

        /// <summary>  Not sure. </summary>
        public static IList<stateWindowsNode> StateList { get { return m_StateList; } set { m_StateList = value; } }

        /// <summary>  Not sure. </summary>
        public static UnityEngine.Object EditorCurrentGameObject { get { return m_EditorCurrentGameObject; } set { m_EditorCurrentGameObject = value; } }

        /// <summary>  Not sure. </summary>
        public static UnityEngine.Object SelectedObject { get { return m_SelectedObject; } set { m_SelectedObject = value; } }

        /// <summary>  Not sure. </summary>
        public static string StateMachineName { get { return m_StateMachineName; } set { m_StateMachineName = value; } }

        /// <summary>  Not sure. </summary>
        public static UnityEngine.Object WasSelectedObject { get { return m_WasSelectedObject; } set { m_WasSelectedObject = value; } }

        /// <summary>  Not sure. </summary>
        public static Vector3 MousePos { get { return m_MousePos; } set { m_MousePos = value; } }

        /// <summary>  Not sure. </summary>
        public static stateWindowsNode SelectedNode { get { return m_SelectedWindowsNode; } set { m_SelectedWindowsNode = value; } }

        /// <summary>  Not sure. </summary>
        public static stateConditionalBase SelectedConditional { get { return m_SelectedConditional; } set { m_SelectedConditional = value; } }

        /// <summary>  Not sure. </summary>
        public static stateEditor StateEditor { get { return m_StateEditor; } set { m_StateEditor = value; } }

        /// <summary>  Not sure. </summary>
        public static string AddConditionPath { get { return m_AddConditionPath; } set { m_AddConditionPath = value; } }

        /// <summary>  Not sure. </summary>
        public static string AddConditionReplace { get { return m_AddConditionReplace; } set { m_AddConditionReplace = value; } }

        /// <summary>  Not sure. </summary>
        public static string DeleteConditionalPath { get { return m_DeleteConditionalPath; } set { m_DeleteConditionalPath = value; } }

        /// <summary>  Not sure. </summary>
        public static string DeleteConditionalClass { get { return m_DeleteClassName; } set { m_DeleteClassName = value; } }

        /// <summary>  Returns the translation matrix for the visual state window so panning can happen. </summary>
        public static utlMatrix34 TranslationMtx { get { return m_TranslationMtx; } set { m_TranslationMtx = value; } }

        /// <summary> Verbose. </summary>
        public static bool Verbose { get { return m_bVerbose; } set { m_bVerbose = value; } }

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

            for (int i = 0; i < m_StateNameList.Count; i++)
            {
                stateWindowsNode node = CreateStateWindowsNode(m_StateNameList[i]);
                m_StateList.Add(node);
            }

            for (int i = 0; i < m_StateList.Count; i++)
            {
                string stateFileName = utlDataAndFile.FindPathAndFileByClassName(m_StateList[i].ClassName, false);
                string buffer = utlDataAndFile.LoadTextFromFile(stateFileName);
                PopulateLinkedConditionStates(m_StateList[i], buffer);
            }
        }

        public static stateWindowsNode CreateStateWindowsNode(string typeName)
        {
            stateWindowsNode winNode = new stateWindowsNode(StateList.Count);
            winNode.ClassName = typeName;

            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;
            string winName = typeName;

            //            TextAsset text = Resources.Load(typeName+".cs") as TextAsset;
            string strBuff = "";
            string fileName = "";
            fileName = utlDataAndFile.FindPathAndFileByClassName(typeName, false);
            strBuff = utlDataAndFile.LoadTextFromFile(fileName);
            string[] words = strBuff.Split(new char[] { '<', '>' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "alias")
                    winName = words[i + 1];
                if (words[i] == "posX")
                    x = Convert.ToSingle(words[i + 1]);
                if (words[i] == "posY")
                    y = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeX")
                    width = Convert.ToSingle(words[i + 1]);
                if (words[i] == "sizeY")
                    height = Convert.ToSingle(words[i + 1]);
            }

            winNode.Set(fileName, typeName, winName, x, y, width, height);
            return winNode;
        }

        /// <summary>
        /// Parse the conditions from the state c sharp file.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="strBuff"></param>
        static void PopulateLinkedConditionStates(stateWindowsNode node, string strBuff)
        {
            string[] words = strBuff.Split(new char[] { ' ', '/', '\n', '\r', '_', '(' });
            bool lookForConditionals = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "<ArtiMechConditions>")
                {
                    lookForConditionals = true;
                }

                if (lookForConditionals && words[i] == "new")
                {
                    //check to see if stateConditionalBase
                    Type type = Type.GetType(stateEditorUtils.kArtimechNamespace + words[i + 3]);
                    if (type != null)
                    {

                        if (IsBaseClassViaString(type, "baseState"))
                        {
                            stateWindowsNode compNode = FindStateWindowsNodeByName(words[i + 3]);
                            if (compNode != null)
                            {
                                node.ConditionLineList.Add(compNode);
                                //Debug.Log("compNode = " + compNode.ClassName);
                            }
                        }
                        /*

                        Type[] nestType = type.GetNestedTypes();
                        string base1Str = "";
                        string base2Str = "";
                        string base3Str = "";
                        string base4Str = "";
                        string base5Str = "";
                        string base6Str = "";
                        string base7Str = "";
                        string base8Str = "";


                        if (type.BaseType != null)
                        {
                            base1Str = type.BaseType.Name;
                            if (type.BaseType.BaseType != null)
                            {
                                base2Str = type.BaseType.BaseType.Name;
                                if (type.BaseType.BaseType.BaseType != null)
                                {
                                    base3Str = type.BaseType.BaseType.BaseType.Name;
                                    if (type.BaseType.BaseType.BaseType.BaseType != null)
                                    {
                                        base4Str = type.BaseType.BaseType.BaseType.BaseType.Name;
                                        if (type.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                        {
                                            base5Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                            if (type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                            {
                                                base6Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                                if (type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                                {
                                                    base7Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                                    if (type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType != null)
                                                    {
                                                        base8Str = type.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.BaseType.Name;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //if (baseOneStr == "baseState" )//|| buffer == "stateGameBase")
                        if (base1Str == "baseState" || base2Str == "baseState" || base3Str == "baseState" || base4Str == "baseState" || base5Str == "baseState" || base6Str == "baseState" || base7Str == "baseState" || base8Str == "baseState")
                        {
                            stateWindowsNode compNode = FindStateWindowsNodeByName(words[i + 3]);
                            if (compNode != null)
                            {
                                node.ConditionLineList.Add(compNode);
                                //Debug.Log("compNode = " + compNode.ClassName);
                            }
                        }*/
                    }
                }
            }
        }

        static stateWindowsNode FindStateWindowsNodeByName(string name)
        {
            stateWindowsNode node = null;
            for (int i = 0; i < m_StateList.Count; i++)
            {
                if (m_StateList[i].ClassName == name)
                    return m_StateList[i];
            }
            return node;
        }

        public static void PopulateStateStrings(string strBuff)
        {
            m_StateNameList.Clear();

            string[] words = strBuff.Split(new char[] { ' ', '(' });

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == "new")
                {
                    Type type = Type.GetType(stateEditorUtils.kArtimechNamespace + words[i + 1]);

                    if (type != null)
                    {
                        if (IsBaseClassViaString(type, "baseState"))
                        {
                            m_StateNameList.Add(words[i + 1]);
                        }
                        /*
                        string baseOneStr = "";
                        string baseTwoStr = "";
                        string baseThreeStr = "";

                        baseOneStr = type.BaseType.Name;
                        if (type.BaseType.BaseType != null)
                        {
                            baseTwoStr = type.BaseType.BaseType.Name;
                            if (type.BaseType.BaseType.BaseType != null)
                                baseThreeStr = type.BaseType.BaseType.BaseType.Name;
                        }

                        //if (baseOneStr == "baseState" )//|| buffer == "stateGameBase")
                        if (baseOneStr == "baseState" || baseTwoStr == "baseState" || baseThreeStr == "baseState")
                        {
                            m_StateNameList.Add(words[i + 1]);
                            // Debug.Log("<color=cyan>" + "<b>" + "words[i + 1] = " + "</b></color>" + "<color=grey>" + words[i + 1] + "</color>" + " .");
                        }
                        */

                    }
                }
            }
        }

        static bool IsBaseClassViaString(Type type, string classNameStr)
        {
            if (type.Name == classNameStr)
                return true;
            if (type.BaseType != null)
            {
                return IsBaseClassViaString(type.BaseType, classNameStr);
            }
            return false;
        }

        public static bool CreateAndAddStateCodeToProject(UnityEngine.Object unityObj, string stateName, string exampleToCopy, string replaceName, bool showLog = true)
        {

            string pathName = "Assets/Scripts/artiMechStates/";
            //string FileName = "Assets/Scripts/Common/statemachine/state_examples/stateTemplate.cs";

            string pathAndFileNameStartState = null;
            if (unityObj is EditorWindow)
            {
                pathAndFileNameStartState = pathName
                                + "aMech"
                                + unityObj.GetType().Name
                                + "/"
                                + stateName
                                + ".cs";
            }
            else
            {
                pathAndFileNameStartState = pathName
                + "aMech"
                + unityObj.name
                + "/"
                + stateName
                + ".cs";
            }

            if (File.Exists(pathAndFileNameStartState))
            {
                if (showLog)
                    Debug.Log("<color=red>stateEditor.CreateStateMachine = </color> <color=blue> " + pathAndFileNameStartState + "</color> <color=red>Already exists and can't be overridden...</color>");
                return false;
            }

            //creates a start state from a template and populate aMech directory
            ReadReplaceAndWrite(exampleToCopy, stateName, pathName, pathAndFileNameStartState, replaceName, "");

            return true;
        }

        public static bool AddConditionCodeToStateCode(string fileAndPath, string conditionName, string toStateName)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";
            //AddState(new stateStartTemplate(this.gameObject), "stateStartTemplate", "new state change system");
            string insertString = "\n            m_ConditionalList.Add(new "
                                + conditionName
                                + "("
                                + "\""
                                + toStateName
                                + "\""
                                + "));";

            //Debug.Log("changeName = " + changeName);

            modStr = utlDataAndFile.InsertInFrontOf(strBuff,
                                                    "<ArtiMechConditions>",
                                                    insertString);

            utlDataAndFile.SaveTextToFile(fileAndPath, modStr);



            return true;
        }

        /// <summary>
        /// Adds states to the statemachine
        /// </summary>
        /// <param name="fileAndPath"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public static bool AddStateCodeToStateMachineCode(string fileAndPath, string stateName)
        {
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);

            if (strBuff == null || strBuff.Length == 0)
                return false;

            string modStr = "";

            string objTypeStr = "";
            if (stateEditorUtils.SelectedObject is GameObject)
                objTypeStr = "(this.gameObject),";
            else
                objTypeStr = "(this),";

            string insertString = "\n            AddState(new "
                                + stateName
                                + objTypeStr
                                + "\""
                                + stateName
                                + "\""
                                + ");";

            modStr = utlDataAndFile.InsertInFrontOf(strBuff,
                                                    "<ArtiMechStates>",
                                                    insertString);

            utlDataAndFile.SaveTextToFile(fileAndPath, modStr);

            return true;
        }

        public static bool SaveStateWindowsNodeData(string fileName, string titleAlias, int x, int y, int width, int height)
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

        //paths and filenames
        public const string k_StateMachineTemplateFileAndPath = "Assets/Scripts/Common/statemachine/state_examples/stateMachineTemplate.cs";
        public const string k_StateTemplateFileAndPath = "Assets/Scripts/Common/statemachine/state_examples/stateEmptyExample.cs";
        public const string k_PathName = "Assets/Scripts/artiMechStates/";

        //public const string k_StateConditionalFileAndPath = "Assets/Scripts/Common/statemachine/state_examples/stateConditionalTemplate.cs";

        /// <summary>
        /// Create the conditional code and add it to the state that has called us.
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        public static void CreateConditionalAndAddToState(string fromState, string toState)
        {


            string replaceName = fromState + "_To_" + toState;

            string text = utlDataAndFile.LoadTextFromFile(AddConditionPath);

            // int a = 12;

            string modText = text.Replace(AddConditionReplace, replaceName);

            string pathAndFile = utlDataAndFile.FindPathAndFileByClassName(fromState);
            string outDir = Path.GetDirectoryName(pathAndFile);

            string pathAndFileName = outDir
                    + "/"
                    + replaceName
                    + ".cs";

            /*
            string pathAndFileName = k_PathName
                                + "aMech"
                                + stateEditorUtils.GameObject.name
                                + "/"
                                + replaceName
                                + ".cs";
                                */

            StreamWriter writeStream = new StreamWriter(pathAndFileName);
            writeStream.Write(modText);
            writeStream.Close();

            string fileAndPathOfState = utlDataAndFile.FindPathAndFileByClassName(fromState);

            AddConditionCodeToStateCode(fileAndPathOfState, replaceName, toState);

            stateWindowsNode node = FindStateWindowsNodeByName(fromState);
            if (node != null)
            {
                /*
                Type windowsNodeType = Type.GetType(toState);
                stateConditionalBase compNode = (stateConditionalBase)Activator.CreateInstance(conditionType);*/
                for (int i = 0; i < m_StateList.Count; i++)
                {
                    if (m_StateList[i].ClassName == toState)
                    {
                        node.ConditionLineList.Add(m_StateList[i]);
                        return;
                    }
                }

            }

            //AssetDatabase.Refresh();
        }

        /// <summary>
        /// Artimech's statemachine and startState generation system.
        /// </summary>
        public static void CreateStateMachineScriptAndStartState()
        {

            string pathAndFileName = k_PathName
                                                        + "aMech"
                                                        + stateEditorUtils.SelectedObject.name
                                                        + "/"
                                                        + "aMech"
                                                        + stateEditorUtils.SelectedObject.name
                                                        + ".cs";

            string pathAndFileNameStartState = k_PathName
                                            + "aMech"
                                            + stateEditorUtils.SelectedObject.name
                                            + "/"
                                            + "aMech"
                                            + stateEditorUtils.SelectedObject.name
                                            + "StartState"
                                            + ".cs";

            if (File.Exists(pathAndFileName))
            {
                Debug.Log("<color=red>stateEditor.CreateStateMachine = </color> <color=blue> " + pathAndFileName + "</color> <color=red>Already exists and can't be overridden...</color>");
                return;
            }

            //create the aMech directory 
            string replaceName = "aMech";
            string directoryName = k_PathName + replaceName + stateEditorUtils.SelectedObject.name;
            Directory.CreateDirectory(directoryName);

            //creates a start state from a template and populate aMech directory
            string stateStartName = "";
            stateStartName = stateEditorUtils.ReadReplaceAndWrite(
                                                        k_StateTemplateFileAndPath,
                                                        stateEditorUtils.SelectedObject.name + "StartState",
                                                        k_PathName,
                                                        pathAndFileNameStartState,
                                                        "stateEmptyExample",
                                                        "aMech");

            //creates the statemachine from a template
            string stateMachName = "";
            stateMachName = stateEditorUtils.ReadReplaceAndWrite(
                                                        k_StateMachineTemplateFileAndPath,
                                                        stateEditorUtils.SelectedObject.name,
                                                        k_PathName,
                                                        pathAndFileName,
                                                        "stateMachineTemplate",
                                                        replaceName);

            //replace the startStartStateTemplate
            utlDataAndFile.ReplaceTextInFile(pathAndFileName, "stateEmptyExample", stateStartName);
            if (Verbose)
            {
                Debug.Log(
                            "<b><color=navy>Artimech Report Log Section A\n</color></b>"
                            + "<i><color=grey>Click to view details</color></i>"
                            + "\n"
                            + "<color=blue>Finished creating a state machine named </color><b>"
                            + stateMachName
                            + "</b>:\n"
                            + "<color=blue>Created and added a start state named </color>"
                            + stateStartName
                            + "<color=blue> to </color>"
                            + stateMachName
                            + "\n\n");
            }

            SaveStateInfo(stateMachName, stateEditorUtils.SelectedObject.name);

            AssetDatabase.Refresh();

            stateEditorUtils.StateMachineName = stateMachName;
            //            m_AddStateMachine = true;

            utlDataAndFile.FindPathAndFileByClassName(stateEditorUtils.StateMachineName, false);
        }

        /// <summary>
        /// This function is used to save what Unity game object I'm working on.  That way when I'm 
        /// rebuilt I remember where I was at.
        /// </summary>
        /// <param name="stateMachineName"></param>
        /// <param name="gameObjectName"></param>
        public static void SaveStateInfo(string stateMachineName, string gameObjectName)
        {
            string stateInfo = stateMachineName + "," + gameObjectName;
            utlDataAndFile.SaveTextToFile(Application.dataPath + "/StateMachine.txt", stateInfo);
        }

        /*
        public static void AddConditionalCallback(object obj)
        {
            Debug.Log("Add conditional.");

        }*/


        /// <summary>
        /// adds a state.
        /// </summary>
        /// <param name="obj"></param>
        public static void CreateStateContextCallback(object obj)
        {
            //make the passed object to a string
            //string clb = obj.ToString();
            editorDisplayWindowsState.menuData menuData = (editorDisplayWindowsState.menuData)obj;


            if (/*clb.Equals("addState") && */ SelectedObject != null)
            {
                SaveAllVisualStateMetaData();

                if (StateList.Count == 0)
                {
                    Debug.LogError("StateList is Empty so you can't create a state.");
                    return;
                }
                //string stateName = "aMech" + GameObject.name + "State" + utlDataAndFile.GetCode(StateList.Count);

                int codeIndex = StateList.Count;
                string stateName = "aMech" + SelectedObject.name + "State" + utlDataAndFile.GetCode(codeIndex);

                /*                GameObject gameObj = null;
                                if (stateEditorUtils.SelectedObject is GameObject)
                                {
                                    gameObj = (GameObject)stateEditorUtils.SelectedObject;
                                }*/

                UnityEngine.Object unityObj = null;
                if (stateEditorUtils.SelectedObject is UnityEngine.Object)
                {
                    unityObj = (UnityEngine.Object)stateEditorUtils.SelectedObject;
                }

                while (!CreateAndAddStateCodeToProject(unityObj, stateName, menuData.m_FileAndPath, menuData.m_ReplaceName, false))
                {
                    codeIndex += 1;
                    stateName = "aMech" + SelectedObject.name + "State" + utlDataAndFile.GetCode(codeIndex);
                    //sanity check
                    if (codeIndex > 10000)
                        return;
                }


                string fileAndPath = "";
                fileAndPath = utlDataAndFile.FindPathAndFileByClassName(stateName);

                stateWindowsNode windowNode = new stateWindowsNode(stateEditorUtils.StateList.Count);

                Vector3 transMousePos = new Vector3();
                transMousePos = stateEditorUtils.TranslationMtx.UnTransform(MousePos);

                windowNode.Set(fileAndPath, stateName, stateName, transMousePos.x, transMousePos.y, 150, 80);
                StateList.Add(windowNode);

                SaveStateWindowsNodeData(fileAndPath, stateName, (int)MousePos.x, (int)MousePos.y, 150, 80);

                fileAndPath = utlDataAndFile.FindPathAndFileByClassName(StateMachineName);

                SaveStateInfo(StateMachineName, SelectedObject.name);

                AddStateCodeToStateMachineCode(fileAndPath, stateName);

            }
        }

        public static stateWindowsNode GetWindowsNodeAtThisLocation(Vector2 vect)
        {
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                if (stateEditorUtils.StateList[i].IsWithinUsingPanZoomTransform(vect))
                {
                    return stateEditorUtils.StateList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the conditional and refrences to it in other classes.
        /// </summary>
        /// <param name="className"></param>
        public static void DeleteAndRemoveConditonal(string className)
        {
            SaveAllVisualStateMetaData();

            string pathAndFileForClassName = utlDataAndFile.FindPathAndFileByClassName(className);
            File.Delete(pathAndFileForClassName);

            utlDataAndFile.RemoveLinesBySubStringInFiles(className, Application.dataPath);

            AssetDatabase.Refresh();
        }

        public static void DeleteAndRemoveState(stateWindowsNode node, string className)
        {
            SaveAllVisualStateMetaData();

            string pathAndFileForClassName = utlDataAndFile.FindPathAndFileByClassName(className);
            string strBuff = utlDataAndFile.LoadTextFromFile(pathAndFileForClassName);
            string[] wordsA = strBuff.Split(new char[] { ' ', '(' });

            for (int i = 0; i < wordsA.Length; i++)
            {
                if (wordsA[i] == "new")
                {
                    Type type = Type.GetType(stateEditorUtils.kArtimechNamespace + wordsA[i + 1]);

                    if (type != null)
                    {
                        string buffer = "";
                        buffer = type.BaseType.Name;
                        if (buffer == "stateConditionBase")
                        {
                            string pathAndFileForConditionClass = utlDataAndFile.FindPathAndFileByClassName(wordsA[i + 1]);
                            File.Delete(pathAndFileForConditionClass);
                        }
                    }
                }
            }

            File.Delete(pathAndFileForClassName);
            m_StateList.Remove(node);
            for (int i = 0; i < m_StateNameList.Count; i++)
            {
                if (m_StateNameList[i] == className)
                {
                    m_StateNameList.RemoveAt(i);
                    break;
                }
            }



            //Delete the state inside the statemachine.
            string pathAndFileForStateMachine = utlDataAndFile.FindPathAndFileByClassName(StateMachineName, false);

            int lineIndex = 0;
            using (StreamReader reader = new StreamReader(pathAndFileForStateMachine))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    lineIndex += 1;
                    if (line.IndexOf(className) >= 0)
                        break;
                }
            }

            var file = new List<string>(System.IO.File.ReadAllLines(pathAndFileForStateMachine));
            file.RemoveAt(lineIndex - 1);
            File.WriteAllLines(pathAndFileForStateMachine, file.ToArray());

            //Refresh because there is a visual bug if not done after adding a state.
            AssetDatabase.Refresh();

        }

        public static void SaveAllVisualStateMetaData()
        {
            for (int i = 0; i < m_StateList.Count; i++)
            {
                m_StateList[i].SaveMetaData();
            }
        }

        public static void CopyStateMachineAndRefactor(string sourcePath, string destinationPath, string renameStr, bool showDebugInfo = false)
        {
            string createdDirectoryName = destinationPath + "/" + renameStr;
            FileUtil.CopyFileOrDirectory(sourcePath, createdDirectoryName);
            for (int i = 0; i < m_StateNameList.Count; i++)
            {
                string changeToName = m_StateList[i].ClassName + renameStr;
                utlDataAndFile.RefactorAllAssets(m_StateList[i].ClassName, changeToName, createdDirectoryName, true);
            }
        }

        public static void Repaint()
        {
            if (m_StateEditor != null)
                m_StateEditor.EditorRepaint();
        }
    }
}

#endif