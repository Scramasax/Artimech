/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.
/// 

using UnityEngine;
using System.Collections;

public class utlBoundingBox : MonoBehaviour
{

    public bool m_ShowGizmos = true;
    public Vector3 m_BBoxSize;
    public Color m_InsideColor;
    public Color m_OutsideColor;
    public Transform m_Focus;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool IsFocusInsideBox()
    {
        if (m_Focus == null)
            return false;

        Vector3 localPos = transform.InverseTransformPoint(m_Focus.position);
        if (localPos.x < -m_BBoxSize.x * 0.5f || localPos.y < -m_BBoxSize.y * 0.5f || localPos.z < -m_BBoxSize.z * 0.5f)
            return false;
        if (localPos.x > m_BBoxSize.x * 0.5f || localPos.y > m_BBoxSize.y * 0.5f || localPos.z > m_BBoxSize.z * 0.5f)
            return false;
        return true;
    }

    void OnDrawGizmos()
    {
        if (!m_ShowGizmos)
            return;

        if(IsFocusInsideBox())
            Gizmos.color = new Color(m_InsideColor.r, m_InsideColor.g, m_InsideColor.b, m_InsideColor.a);
        else
            Gizmos.color = new Color(m_OutsideColor.r, m_OutsideColor.g, m_OutsideColor.b, m_OutsideColor.a);

        Vector3 start;
        Vector3 end;

        //plus to neg
        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        //neg to plus
        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        //neg x,neg y, pos z
        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, -m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

        start = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, -m_BBoxSize.z * 0.5f);
        end = transform.TransformPoint(-m_BBoxSize.x * 0.5f, m_BBoxSize.y * 0.5f, m_BBoxSize.z * 0.5f);
        Gizmos.DrawLine(start, end);

    }
}