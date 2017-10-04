using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public class editorDisplayWindowsState : baseState
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        /// 
        IList<stateConditionalBase> m_ConditionalList;
        static Texture2D m_BackGroundImage = null;
        bool m_AddConditionalBool = false;

        public bool AddConditionalBool
        {
            get
            {
                return m_AddConditionalBool;
            }
        }

        public editorDisplayWindowsState(GameObject gameobject)
        {
            m_AddConditionalBool = false;
            m_GameObject = gameobject;
            m_ConditionalList = new List<stateConditionalBase>();
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editorDisplayToWaitConditional("Wait"));
            m_ConditionalList.Add(new editorDisplayToLoadConditional("Load"));
            m_ConditionalList.Add(new editorDisplayToAddConditional("Add Conditional"));
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
         //   GUILayout.Label(m_BackGroundImage);

            // input
            Event ev = Event.current;
            //           Debug.Log(ev.mousePosition);
            //           Debug.Log(ev.type);
            //           Debug.Log("---> " + ev.button);

            stateEditorUtils.MousePos = ev.mousePosition;

            if (Event.current.button == 0)
            {
                //Debug.Log("-------------> " + ev.type);
                for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
                {
                    float x = stateEditorUtils.StateList[i].WinRect.x;
                    float y = stateEditorUtils.StateList[i].WinRect.y;
                    float width = stateEditorUtils.StateList[i].WinRect.width;
                    float height = stateEditorUtils.StateList[i].WinRect.height;

                    if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                    {
                        if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
                        {
                            stateEditorUtils.StateList[i].SetPos(ev.mousePosition.x - (width * 0.5f), ev.mousePosition.y - (height * 0.5f));
                        }
                    }

                }
            }

            if (ev.button == 1)
            {
                if (ev.type == EventType.MouseDown)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add State"), false, ContextCallback, "addState");
                    menu.ShowAsContext();
                    ev.Use();
                }
            }

            // render populated state windows
      
            for (int i = 0; i < stateEditorUtils.StateList.Count; i++)
            {
                //GUI.Window(i, stateEditorUtils.StateList[i].WinRect, DrawNodeWindow, stateEditorUtils.StateList[i].WindowTitle);
                stateEditorUtils.StateList[i].Update(this);
            }
        }

        /*
            void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
        */

        void ContextCallback(object obj)
        {
            stateEditorUtils.ContextCallback(obj);
        }

        public void AddConditionalCallback(object obj)
        {
            m_AddConditionalBool = true;
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_AddConditionalBool = false;
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}