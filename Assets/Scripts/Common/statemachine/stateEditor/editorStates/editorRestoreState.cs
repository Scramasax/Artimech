using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace artiMech
{
    public class editorRestoreState : editorBaseState
    {
        /// <summary>
        /// This state restores the previous gamestate and seleted object
        /// </summary>
        /// <param name="gameobject"></param>
        public editorRestoreState(GameObject gameobject) : base(gameobject)
        {
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorRestoreToLoadConditional("Load"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            for (int i = 0; i < m_ConditionalList.Count; i++)
            {
                string changeNameToThisState = null;
                changeNameToThisState = m_ConditionalList[i].UpdateConditionalTest(this);
                if (changeNameToThisState != null)
                {
                    m_ChangeStateName = changeNameToThisState;
                    m_ChangeBool = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {

        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            const string fileAndPath = "Assets/StateMachine.txt";
            string strBuff = "";
            strBuff = utlDataAndFile.LoadTextFromFile(fileAndPath);
            if(strBuff==null)
            {
                Debug.LogError("<color=maroon>" + "<b>" + "Restore file not found = " + "</b></color>" + "<color=red>" + "Assets/StateMachine.txt" + "</color>" + " .");
                return;
            }

            FileUtil.DeleteFileOrDirectory(fileAndPath);

            string[] words = strBuff.Split(new char[] { ' ' });

            GameObject gameObject = utlDataAndFile.FindGameObjectByName(words[1]);
            if(gameObject==null)
            {
                Debug.LogError("<color=maroon>" + "<b>" + "Restore gameObject not found = " + "</b></color>" + "<color=red>" + words[1] + "</color>" + " .");
                return;
            }

            stateEditorUtils.GameObject = gameObject;
            stateEditorUtils.EditorCurrentGameObject = gameObject;
            stateEditorUtils.StateMachineName = words[0];

            //this will allow only one statemachine per game object which isn't optimal.
            if(stateEditorUtils.GameObject.GetComponent<stateMachineBase>()==null)
                stateEditorUtils.GameObject.AddComponent(System.Type.GetType("artiMech." + words[0]));

        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}