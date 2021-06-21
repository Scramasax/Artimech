using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Artimech
{
    /// <summary>
    /// Base window for the Artimech state editor system
    /// </summary>
    public class artProcessingWindow : artMessageWindow
    {

        Texture2D m_LoadingImage = null;
        Thread m_Thread;
        float m_RotAngle;
        EditorWindow m_EditorWindow;
        int m_GuiDepth = 0;

        public int GuiDepth { get => m_GuiDepth; set => m_GuiDepth = value; }

        public artProcessingWindow(string title, string message, int fontSize, Color fontColor, Rect rect, Color color, int id) : base(title, message, fontSize, fontColor, rect, color, id)
        {
            InitImage();
            m_Thread = new Thread(new ThreadStart(ThreadProc));
            m_Thread.Start();
        }
        //            GUI.DrawTexture(new Rect(0, 0, 10, 10), m_LoadingImage);
        new public void Update(EditorWindow editorWindow)
        {
            m_EditorWindow = editorWindow;


            m_WinRect.x = editorWindow.position.width * 0.5f;
            m_WinRect.width = editorWindow.position.width * 0.5f;
            //m_WinRect.height = Screen.height * 0.25f;
            m_WinRect.height = 85;
            m_WinRect.x = (editorWindow.position.width * 0.5f) - (m_WinRect.width * 0.5f);
            m_WinRect.y = (editorWindow.position.height * 0.5f) - (m_WinRect.height * 0.5f);

            var TextStyle = new GUIStyle();

            GUI.depth = m_GuiDepth;
            GUI.Window(m_Id + 1, WinRect, DrawProcessing, "", TextStyle);
            GUI.Window(m_Id, WinRect, DrawMessage, m_Title);
            //m_EditorWindow.Repaint();
            // GUI.DrawTexture(new Rect(WinRect.x, WinRect.y, 64, 64), m_LoadingImage);
            //base.Update(editorWindow);

        }

        public void DrawProcessing(int id)
        {
            GUI.backgroundColor = Color.clear;
            float xOffset = WinRect.width - 72;
            float yOffset = 21;
            Vector2 pivot = new Vector2(xOffset + 32, yOffset + 32);
            GUIUtility.RotateAroundPivot(m_RotAngle % 360, pivot);
            GUI.DrawTexture(new Rect(xOffset, yOffset, 64, 64), m_LoadingImage);
        }

        void InitImage()
        {
            string fileAndPath = utlDataAndFile.FindAFileInADirectoryRecursively(Application.dataPath, "circleArrows.png");
            byte[] fileData;
            fileData = File.ReadAllBytes(fileAndPath);

            m_LoadingImage = null;
            m_LoadingImage = new Texture2D(512, 512);
            m_LoadingImage.LoadImage(fileData);
        }

        public void AbortThread()
        {
            m_Thread.Abort();
        }

        void ThreadProc()
        {
            while (true)
            {
                //                if (m_EditorWindow != null)
                //                    m_EditorWindow.Repaint();
                m_RotAngle += 3.0f;
                if (m_EditorWindow != null)
                {
                    ArtimechEditor theScript = (ArtimechEditor)m_EditorWindow;
                    theScript.RepaintOnUpdate = true;
                }
                Thread.Sleep(20);
            }
        }
    }
}

#endif