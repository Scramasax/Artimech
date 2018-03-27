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
/// Original layer mask expample: http://wiki.unity3d.com/index.php/LayerMaskExtensions
/// under this lic:
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER 
/// THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.  BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, 
/// YOU ACCEPT AND AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE.TO THE EXTENT THIS LICENSE MAY BE CONSIDERED TO BE A CONTRACT, 
/// THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 

using UnityEngine;
using System.Collections.Generic;

public static class utlLayerMaskExtensions
{
    public static LayerMask Create(params string[] layerNames)
    {
        return NamesToMask(layerNames);
    }

    public static LayerMask Create(params int[] layerNumbers)
    {
        return LayerNumbersToMask(layerNumbers);
    }

    public static LayerMask NamesToMask(params string[] layerNames)
    {
        LayerMask ret = (LayerMask)0;
        foreach (var name in layerNames)
        {
            ret |= (1 << LayerMask.NameToLayer(name));
        }
        return ret;
    }

    public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
    {
        LayerMask ret = (LayerMask)0;
        foreach (var layer in layerNumbers)
        {
            ret |= (1 << layer);
        }
        return ret;
    }

    public static LayerMask Inverse(this LayerMask original)
    {
        return ~original;
    }

    public static LayerMask AddToMask(this LayerMask original, params string[] layerNames)
    {
        return original | NamesToMask(layerNames);
    }

    public static LayerMask RemoveFromMask(this LayerMask original, params string[] layerNames)
    {
        LayerMask invertedOriginal = ~original;
        return ~(invertedOriginal | NamesToMask(layerNames));
    }

    public static string[] MaskToNames(this LayerMask original)
    {
        var output = new List<string>();

        for (int i = 0; i < 32; ++i)
        {
            int shifted = 1 << i;
            if ((original & shifted) == shifted)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    output.Add(layerName);
                }
            }
        }
        return output.ToArray();
    }

    public static string MaskToString(this LayerMask original)
    {
        return MaskToString(original, ", ");
    }

    public static string MaskToString(this LayerMask original, string delimiter)
    {
        return string.Join(delimiter, MaskToNames(original));
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
         return mask == (mask | (1 << layer));
    }
}