/// Artimech
/// 
/// Copyright � <2017> <George A Lancaster>
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

public static class utlMath
{

    /// <summary>
    /// Lerp for a float.
    /// </summary>
    /// <param name="f1"></param>
    /// <param name="f2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float Lerp(float f1, float f2, float t)
    {
        float num;
        num = (f1 + (f2 - f1) * t);
        return (num);
    }

    /// <summary>
    /// Lerp function that isn't capped.  This is "old"  Todo: Remove
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        Vector3 outVector = new Vector3();
        outVector.x = Lerp(a.x, b.x, t);
        outVector.y = Lerp(a.y, b.y, t);
        outVector.z = Lerp(a.z, b.z, t);
        return outVector;
    }

    /// <summary>
    /// Gets the nearest point on a line segment.
    /// </summary>
    /// <param name="vPoint"></param>
    /// <param name="vLineStart"></param>
    /// <param name="vLineEnd"></param>
    /// <returns></returns>
    public static Vector3 NearestPointOnLine(Vector3 vPoint, Vector3 vLineStart, Vector3 vLineEnd)
    {
        Vector3 vectOut = new Vector3();
        Vector3 vectPointMinusLineStart, vectEndMinusStart;
        float dotProduct;

        vectPointMinusLineStart = vPoint - vLineStart;
        vectEndMinusStart = vLineEnd - vLineStart;
        if (Vector3.Dot(vectPointMinusLineStart, vectEndMinusStart) <= 0.0f)
        {
            return vLineStart;
        }

        vectPointMinusLineStart = vPoint - vLineEnd;
        vectEndMinusStart *= -1;

        dotProduct = Vector3.Dot(vectPointMinusLineStart, vectEndMinusStart);
        if (dotProduct <= 0.0f)
        {
            return vLineEnd;
        }

        float distance = dotProduct / vectEndMinusStart.sqrMagnitude;

        vectOut = Vector3.Lerp(vLineEnd, vLineStart, distance);

        return vectOut;
    }

    /// <summary>
    /// Extends a line x amount of distance.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Vector3 ExtendLine(Vector3 start, Vector3 end, float size)
    {
        float dist = Vector3.Distance(start, end);
        if (dist == 0.0f)
            return start;

        return Lerp(start, end, (size / dist) + 1.0f);

    }

    public static Vector3 LerpBySize(Vector3 start, Vector3 end, float size)
    {
        Vector3 outVect = new Vector3();
        float dist = Vector3.Distance(start, end);
        if (dist == 0.0f)
            return start;
        outVect = Lerp(start, end, (size / dist) + 1.0f);

        return outVect;
    }

    /// <summary>
    /// Find the distance between two floats.
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <returns></returns>
    public static float FloatDistance(float x1, float x2)
    {
        float dx = Mathf.Abs(x1 - x2);
        return dx;
    }

    /// <summary>
    /// Distance check that doesn't use the Y component.
    /// </summary>
    /// <param name="vectA"></param>
    /// <param name="vectB"></param>
    /// <returns></returns>
    public static float FlatDistance(Vector3 vectA, Vector3 vectB)
    {
        Vector3 tempA = new Vector3(vectA.x, 0.0f, vectA.z);
        Vector3 tempB = new Vector3(vectB.x, 0.0f, vectB.z);
        return Vector3.Distance(tempA, tempB);
    }

    /// <summary>
    /// Camera visibility check.
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    /// <summary>
    /// Random vect in a cubic volume.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 RandomVector(Vector3 min, Vector3 max)
    {
        Vector3 outVect = new Vector3();
        outVect.x = Random.Range(min.x, max.x);
        outVect.y = Random.Range(min.y, max.y);
        outVect.z = Random.Range(min.z, max.z);
        return outVect;
    }

    /// <summary>
    /// Clamp a Vector3.  This is old as well.  Todo: Remove.
    /// </summary>
    /// <param name="vectIn"></param>
    /// <param name="vectMin"></param>
    /// <param name="vectMax"></param>
    /// <returns></returns>
    public static Vector3 ClampVector(Vector3 vectIn, Vector3 vectMin, Vector3 vectMax)
    {
        Vector3 outVect = new Vector3();
        outVect.x = Mathf.Clamp(vectIn.x, vectMin.x, vectMax.x);
        outVect.y = Mathf.Clamp(vectIn.y, vectMin.y, vectMax.y);
        outVect.z = Mathf.Clamp(vectIn.z, vectMin.z, vectMax.z);
        return outVect;
    }

    /// <summary>
    /// Angle between two vectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Angle(Vector3 a, Vector3 b)
    {

        float mag = a.magnitude * b.magnitude;
        if (mag == 0.0f)
            return 0.0f;
        if (mag != 1.0f)
            mag = Mathf.Sqrt(mag);
        return Mathf.Acos((a.x * b.x + a.y * b.y + a.z * b.z) / mag);
    }

    /// <summary>
    /// Gets the mean "average" of a Vector3.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns>Vector3 average.</returns>
    public static Vector3 GetMeanVector(Vector3[] positions)
    {
        if (positions.Length == 0)
            return Vector3.zero;

        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3 pos in positions)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }

        return new Vector3(x / positions.Length, y / positions.Length, z / positions.Length);
    }

    public static Vector3 AddScaled(Vector3 vectA, Vector3 vectB, float scale)
    {
        Vector3 outVect = new Vector3();
        outVect.x = vectA.x + scale * vectB.x;
        outVect.y = vectA.y + scale * vectB.y;
        outVect.z = vectA.z + scale * vectB.z;
        
        return outVect;
    }

    public static Vector3 SubScaled(Vector3 vectA, Vector3 vectB, float scale)
    {
        Vector3 outVect = new Vector3();
        outVect.x = vectA.x - scale * vectB.x;
        outVect.y = vectA.y - scale * vectB.y;
        outVect.z = vectA.z - scale * vectB.z;

        return outVect;
    }

    //{x=a.x-f*b.x; 	y=a.y-f*b.y; 	z=a.z-f*b.z;}

}
