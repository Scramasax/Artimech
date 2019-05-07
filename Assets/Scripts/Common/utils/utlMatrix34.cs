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
    /// <summary> Get and set the x of the matrix. </summary>
    public Vector3 A { get { return m_A; } set { m_A = value; } }

    /// <summary> Get and set the y of the matrix. </summary>
    public Vector3 B { get { return m_B; } set { m_B = value; } }

    /// <summary> Get and set the z of the matrix. </summary>
    public Vector3 C { get { return m_C; } set { m_C = value; } }

    /// <summary> Get and set the position of the matrix. </summary>
    public Vector3 D { get { return m_D; } set { m_D = value; } }

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

    public void Set(utlMatrix34 mtx)
    {
        m_A = mtx.A;
        m_B = mtx.B;
        m_C = mtx.C;
        m_D = mtx.D;
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

    public void LookAtFlat(Vector3 to)
    {
        LookAt(new Vector3(to.x, m_D.y, to.z));
    }

    public void Translate(Vector3 pos)
    {
        m_D += pos;
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

    /// <summary>
    /// Local to world for a rect.  Size and position.
    /// </summary>
    /// <param name="localRect"></param>
    /// <returns></returns>
    public Rect Transform(Rect localRect)
    {
        Rect worldRect = new Rect(localRect);
        worldRect.position = Transform(worldRect.position);
        worldRect.width *= m_A.sqrMagnitude;
        worldRect.height *= m_B.sqrMagnitude;
        return worldRect;
    }

    /// <summary>
    /// World to local pos.
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector3 UnTransform(Vector3 worldPos)
    {
        Vector3 localPos = new Vector3();

        localPos = worldPos - m_D;
        localPos.x = Vector3.Dot(m_A, localPos);
        localPos.y = Vector3.Dot(m_B, localPos);
        localPos.z = Vector3.Dot(m_C, localPos);

        return localPos;
    }

    public void RotateLocalX(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);
        Vector3 rB = new Vector3(m_B.x, m_B.y, m_B.z);
        Vector3.Scale(rB, new Vector3(cr, cr, cr));
        utlMath.AddScaled(rB, m_C, sr);
        m_C.Scale(new Vector3(cr, cr, cr));
        utlMath.SubScaled(m_C, m_B, sr);
        m_B.Set(rB.x, rB.y, rB.z);
    }

    public void RotateLocalY(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);
        Vector3 rA = new Vector3(m_B.x, m_B.y, m_B.z);
        Vector3.Scale(rA, new Vector3(cr, cr, cr));
        utlMath.SubScaled(rA, m_C, sr);
        m_C.Scale(new Vector3(cr, cr, cr));
        utlMath.AddScaled(m_C, m_A, sr);
        m_B.Set(rA.x, rA.y, rA.z);
    }

    public void RotateLocalZ(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);
        Vector3 rA = new Vector3(m_B.x, m_B.y, m_B.z);
        Vector3.Scale(rA, new Vector3(cr, cr, cr));
        utlMath.AddScaled(rA, m_B, sr);
        m_B.Scale(new Vector3(cr, cr, cr));
        utlMath.SubScaled(m_B, m_A, sr);
        m_B.Set(rA.x, rA.y, rA.z);

        /*        float cr = cosf(r);
                float sr = sinf(r);
                Vector3 rA(a);
                rA.Scale(cr);
                rA.AddScaled(b, sr);
                b.Scale(cr);
                b.SubScaled(a, sr);
                a.Set(rA);*/
    }

    public void RotateX(float r)
    {
        utlMatrix34 m = new utlMatrix34();
        m.RotateXXform(r);
        this.Dot(m);
    }

    public void RotateY(float r)
    {
        utlMatrix34 m = new utlMatrix34();
        m.RotateYXform(r);
        this.Dot(m);
    }

    public void RotateZ(float r)
    {
        utlMatrix34 m = new utlMatrix34();
        m.RotateZXform(r);
        this.Dot(m);
    }

    void RotateXXform(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);

        A.Set(1.0f, 0.0f, 0.0f);
        B.Set(0.0f, cr, sr);
        C.Set(0.0f, -sr, cr);
        D.Set(0, 0, 0);
    }

    void RotateYXform(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);

        A.Set(cr, 0.0f, -sr);
        B.Set(0.0f, 1.0f, 0.0f);
        C.Set(sr, 0.0f, cr);
        D.Set(0, 0, 0);
    }

    void RotateZXform(float r)
    {
        float cr = Mathf.Cos(r);
        float sr = Mathf.Sin(r);

        A.Set(cr, sr, 0.0f);
        B.Set(-sr, cr, 0.0f);
        C.Set(0.0f, 0.0f, 1.0f);
        D.Set(0, 0, 0);
    }

    void Dot(utlMatrix34 m)
    {
        float x, y, z;

        x = A.x * m.A.x + A.y * m.B.x + A.z * m.C.x;
        y = A.x * m.A.y + A.y * m.B.y + A.z * m.C.y;
        z = A.x * m.A.z + A.y * m.B.z + A.z * m.C.z;
        A.Set(x, y, z);

        x = B.x * m.A.x + B.y * m.B.x + B.z * m.C.x;
        y = B.x * m.A.y + B.y * m.B.y + B.z * m.C.y;
        z = B.x * m.A.z + B.y * m.B.z + B.z * m.C.z;
        B.Set(x, y, z);

        x = C.x * m.A.x + C.y * m.B.x + C.z * m.C.x;
        y = C.x * m.A.y + C.y * m.B.y + C.z * m.C.y;
        z = C.x * m.A.z + C.y * m.B.z + C.z * m.C.z;
        C.Set(x, y, z);

        x = D.x * m.A.x + D.y * m.B.x + D.z * m.C.x + m.D.x;
        y = D.x * m.A.y + D.y * m.B.y + D.z * m.C.y + m.D.y;
        z = D.x * m.A.z + D.y * m.B.z + D.z * m.C.z + m.D.z;
        D.Set(x, y, z);
    }

    public void Print()
    {
        string buffer = "Click to see utlMatrix34 info:\nA = " + A + "\nB = " + B + "\nC = " + C + "\nD = " + D + "\n";
        utlDebugPrint.Inst.print(buffer);
        //Debug.Log(buffer);
    }
}

#endregion

