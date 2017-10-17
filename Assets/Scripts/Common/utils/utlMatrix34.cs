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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple matrix 34 class to do non gameobject related math.
/// </summary>
public class utlMatrix34
{
    #region Variables
    Vector3 m_A = new Vector3();
    Vector3 m_B = new Vector3();
    Vector3 m_C = new Vector3();
    Vector3 m_D = new Vector3();
    #endregion

    #region Accessors

    #endregion

    #region Member Functions
    public utlMatrix34()
    {
        Identity();
    }

    public utlMatrix34(Vector3 position)
    {
        Identity3x3();
        m_D = position;
    }

    public void Identity()
    {
        m_A.Set(1, 0, 0);
        m_B.Set(0, 1, 0);
        m_C.Set(0, 0, 1);
        m_D.Set(0, 0, 0);
    }

    public void Identity3x3()
    {
        m_A.Set(1, 0, 0);
        m_B.Set(0, 1, 0);
        m_C.Set(0, 0, 1);
    }

    /// <summary>
    /// Look at a position in space.
    /// </summary>
    /// <param name="to"></param>
    public void LookAt(Vector3 to)
    {
        if (to == m_D)
            return;

        m_C = to - m_D;
        m_C.Normalize();
        if (m_C.x != 0.0f || m_C.z != 0.0f)
        {
            m_A.Set(m_C.z, 0.0f, -m_C.x);
            m_A.Normalize();
            m_B = Vector3.Cross(m_C, m_A);
        }
        else
        {
            m_A.Set(-m_C.y, 0.0f, 0.0f);
            m_B.Set(0.0f, 0.0f, m_C.y);
        }
    }

    /// <summary>
    /// Local to world.
    /// </summary>
    /// <param name="localPos"></param>
    /// <returns></returns>
    public Vector3 Transform(Vector3 localPos)
    {
        Vector3 worldPos = new Vector3();
        worldPos.x = localPos.x * m_A.x + localPos.y * m_B.x + localPos.z * m_C.x + m_D.x;
        worldPos.y = localPos.x * m_A.y + localPos.y * m_B.y + localPos.z * m_C.y + m_D.y;
        worldPos.z = localPos.x * m_A.z + localPos.y * m_B.z + localPos.z * m_C.z + m_D.z;
        return worldPos;
    }
#endregion
}
