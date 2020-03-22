#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artimech
{
    [CreateAssetMenu(fileName = "ArtimechConfig", menuName = "Artimech/Configure", order = 1)]
    public class artConfigurationData : ScriptableObject
    {

        [Space(10)]
        [Header("Directories")]

        [SerializeField]
        [Tooltip("This is where Artimech copies the state machine script from using as a template to create with.")]
        filePathAndName m_MasterScripStateMachineFile = null;

        [SerializeField]
        [Tooltip("This is where Artimech copies the master state code from when building new states via a renaming system.")]
        filePathAndName m_MasterScriptStateFile = null;

        [SerializeField]
        [Tooltip("Directory where Artimech puts newly generated scripts.")]
        folderPathName m_CopyToDirectory = null;

        [Space(10)]
        [Header("Naming")]
        [SerializeField]
        [Tooltip("Prefix naming allows for the various state machines to be named with a pre sub string.")]
        string m_PrefixName = "";
        [SerializeField]
        [Tooltip("Postfix naming allows for the various state machines to be named with a post sub string.")]
        string m_PostfixName = "";

        [Space(10)]
        [Header("Colors")]
        [SerializeField]
        [Tooltip("Color for the main window's back ground color.")]
        Color m_BackgroundColor = Color.white;

        [SerializeField]
        [Tooltip("Line color for the main editor window.")]
        Color m_LineColor = Color.blue;

        [SerializeField]
        [Tooltip("Line color for the state windows.")]
        Color m_LineShadowColor = Color.grey;

        [SerializeField]
        [Tooltip("Line color for the state windows.")]
        Vector2 m_MainWindowGridSize = new Vector2(-5000, 5000);

        [SerializeField]
        [Tooltip("Line color for the state windows.")]
        Vector2 m_SmallSquareGridSize = new Vector2(25, 25);

        [SerializeField]
        [Tooltip("Small grid line width.")]
        [Range(0.5f, 10)]
        float m_SmallSquareGridLineWidth = 1.0f;

        [SerializeField]
        [Tooltip("Line color for the state windows.")]
        Vector2 m_BigSqureGridSize = new Vector2(250, 250);

        [SerializeField]
        [Tooltip("Small grid line width.")]
        [Range(0.5f, 10)]
        float m_BigSquareGridLineWidth = 3.0f;

        [Space(10)]
        [SerializeField]
        [Tooltip("Line color for the state windows.")]
        Color m_WindowLineColor = Color.black;

        [SerializeField]
        [Tooltip("State window background window color default.")]
        Color m_StateWindowColor = Color.gray;

        [SerializeField]
        [Tooltip("State window header's color.")]
        Color m_StateHeaderColor = Color.grey;

        [Space(10)]
        [SerializeField]
        [Tooltip("Arrow color line for the arrows that connect the states.")]
        Color m_ArrowLineColor = Color.black;

        [SerializeField]
        [Tooltip("Arrow fill color for the arrows that connect the states.")]
        Color m_ArrowFillColor = Color.white;

        [SerializeField]
        [Tooltip("Arrow fill color for the arrows that connect the states.")]
        [Range(0.5f, 10)]
        float m_ArrowLineWidth = 2.0f;

        public filePathAndName MasterScripStateMachineFile { get => m_MasterScripStateMachineFile; }
        public filePathAndName MasterScriptStateFile { get => m_MasterScriptStateFile; }
        public folderPathName CopyToDirectory { get => m_CopyToDirectory; }
        public string PrefixName { get => m_PrefixName; }
        public string PostfixName { get => m_PostfixName; }
        public Color BackgroundColor { get => m_BackgroundColor; }
        public Color LineColor { get => m_LineColor; }
        public Color StateWindowColor { get => m_StateWindowColor; }
        public Color StateHeaderColor { get => m_StateHeaderColor; }
        public Color ArrowLineColor { get => m_ArrowLineColor; }
        public Color ArrowFillColor { get => m_ArrowFillColor; }
        public Color WindowLineColor { get => m_WindowLineColor; }
        public float ArrowLineWidth { get => m_ArrowLineWidth; }
        public Color LineShadowColor { get => m_LineShadowColor; }
        public Vector2 MainWindowGridSize { get => m_MainWindowGridSize; }
        public Vector2 SmallSquareGridSize { get => m_SmallSquareGridSize; }
        public float SmallSquareGridLineWidth { get => m_SmallSquareGridLineWidth; }
        public Vector2 BigSqureGridSize { get => m_BigSqureGridSize; }
        public float BigSquareGridLineWidth { get => m_BigSquareGridLineWidth; }
    }
}

#endif