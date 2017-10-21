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
using System.Collections;
using System.Collections.Generic;

public class utlDebugDraw : MonoBehaviour 
{

	private static utlDebugDraw inst		= null;

	//a common singleton class for unity
	public static utlDebugDraw Inst
	{
		get
		{
			if (inst == null)
			{
				Debug.Log("Instantiating utlDebugDraw");
				GameObject DEBUGDRAWOBJ		= new GameObject();
				inst	 					= DEBUGDRAWOBJ.AddComponent<utlDebugDraw>();
				DEBUGDRAWOBJ.name			= "UTLDEBUGDRAW";
				//DontDestroyOnLoad(PRINTOBJ);
			}

			return inst;
		}		
	}
/*
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}*/

	public void DrawCircleY(Vector3 pos, float size,Color color,int sides = 12)
	{
		transform.position					= new Vector3(pos.x,pos.y,pos.z);
		transform.rotation					= Quaternion.identity;

		Vector3 start;
		Vector3 end;
		Vector3 vsize						= new Vector3(0,0,size);

		float angle							=360.0f/sides;

		for(int i=0;i<sides;i++)
		{
			start							= transform.TransformPoint(vsize);
			transform.Rotate(new Vector3(0,angle,0));
			end								= transform.TransformPoint(vsize);
			Debug.DrawLine(start, end, color);
		}
	}

	public void DrawFlatZBox(Vector3 min,Vector3 max,Color color)
	{
		Vector3 start						= new Vector3();
		Vector3 end							= new Vector3();

		//top left to right
		start.Set(min.x,min.y,min.z);
		end.Set(max.x,min.y,min.z);
		Debug.DrawLine(min,max,color);

		/*

		//right side down line
		start.Set(max.x,min.y,min.z);
		end.Set(max.x,max.y,min.z);
		Debug.DrawLine(min,max,color);

		//bottom right to left
		start.Set(max.x,max.y,min.z);
		end.Set(min.x,max.y,min.z);
		Debug.DrawLine(min,max,color);

		//left side down line
		start.Set(min.x,min.y,min.z);
		end.Set(min.x,max.y,min.z);
		Debug.DrawLine(min,max,color);
		
		*/
	}
}
