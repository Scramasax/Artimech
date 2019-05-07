#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Artimech
{
    public class artRGBAData
    {
        [XmlAttribute("Name")]
        public string m_Name;
        [XmlElement("Red")]
        public float m_R;
        [XmlElement("Green")]
        public float m_G;
        [XmlElement("Blue")]
        public float m_B;
        [XmlElement("Alpha")]
        public float m_A;
    }

}
#endif