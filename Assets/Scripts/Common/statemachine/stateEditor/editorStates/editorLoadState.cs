using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
    public class editorLoadState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        IList<stateConditionalBase> m_ConditionalList;
        static Texture2D m_LoadingImage = null;

        public editorLoadState(GameObject gameobject)
        {
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();

            //InitImage();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorLoadToDisplayConditional("Display Windows"));
            m_ConditionalList.Add(new editorLoadToWaitConditional("Wait"));
        }

        void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, "LoadingA.png");
            byte[] fileData;
            fileData = File.ReadAllBytes(fileAndPath);

            m_LoadingImage = null;
            m_LoadingImage = new Texture2D(512, 256);
            m_LoadingImage.LoadImage(fileData);
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
            GUILayout.Label(m_LoadingImage);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {

        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}