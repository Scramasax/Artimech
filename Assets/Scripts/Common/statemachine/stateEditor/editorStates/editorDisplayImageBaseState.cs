using UnityEngine;
using UnityEditor;
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
    public class editorDisplayImageBaseState : stateGameBase
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        Texture2D m_BackgroundImage = null;
        string m_ImageName = "";

        public editorDisplayImageBaseState(GameObject gameobject,string imageName) : base(gameobject)
        {
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            m_ImageName = imageName;
        }

        protected void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, m_ImageName);
            byte[] fileData;
            fileData = File.ReadAllBytes(fileAndPath);

            m_BackgroundImage = null;
            m_BackgroundImage = new Texture2D(2048, 2048);
            m_BackgroundImage.LoadImage(fileData);
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            if (m_BackgroundImage == null)
                InitImage();

            base.Update();
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
            GUILayout.Label(m_BackgroundImage);
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