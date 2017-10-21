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

public class utlDebugPrint : MonoBehaviour 
{	
	private static utlDebugPrint inst		= null;
	
	float xTextPrintOffset					= 3.0f;
	Vector2 scrollPosition 					= Vector2.zero;
	//change windowRect for initial position and size.
	Rect windowRect							= new Rect(20, 20, 300, 400);
	int windowId							= 1;

	//list that contains the info to be printed to the window
	public List<printInfo> printList		= null;

	//printInfo tells the output to be boxed or labeled
	public class printInfo
	{
		public bool boxBool;
		public string buffer;
		public printInfo(string buffer)
		{
			this.buffer						= buffer;
			boxBool							= false;
		}

		public printInfo(string buffer,bool boxBool)
		{
			this.buffer						= buffer;
			this.boxBool					= boxBool;
		}
	}

	//a common singleton class for unity
	public static utlDebugPrint Inst
	{
		get
		{
			if (inst == null)
			{
				Debug.Log("Instantiating utlDebugPrint");
				GameObject PRINTOBJ			= new GameObject();
				inst	 					= PRINTOBJ.AddComponent<utlDebugPrint>();
				PRINTOBJ.name 				= "UTLDEBUGPRINT";
				inst.printList				= new List<printInfo>();
				DontDestroyOnLoad(PRINTOBJ);
			}
			
			return inst;
		}		
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}	
	
	/// <summary>prints a normal text to the debug window 
	/// </summary> 
	public void print(string buffer)
	{
		printInfo data						= new printInfo(buffer);
		printList.Add(data);
		scrollPosition.y					= Mathf.Infinity;
	}

	/// <summary>prints a box text to the debug window 
	/// </summary> 
	public void printbox(string buffer)
	{
		printInfo data						= new printInfo(buffer,true);
		printList.Add(data);
		scrollPosition.y					= Mathf.Infinity;
	}

	//the gui crap
	void OnGUI()
	{
		windowRect							= GUI.Window(windowId,windowRect,DebugWindow,"");
	}

	void DebugWindow(int windowId)
	{	

		GUILayout.BeginArea(new Rect(xTextPrintOffset, 0, windowRect.width, windowRect.height));

		GUILayout.Space(2);
		GUILayout.Box("Debug Window");

		if (GUILayout.Button("Close"))
		{
			printList.Clear();
			GameObject.Destroy(this);
		}
		if (GUILayout.Button("Clear"))
			printList.Clear();

		scrollPosition						= GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowRect.width-10), GUILayout.Height(windowRect.height-80));

		for(int i=0;i<printList.Count;i++)
		{
			var t							= printList[i];
			if(t.boxBool)
				GUILayout.Box(t.buffer);
			else
				GUILayout.Label(t.buffer);
		}
		
		GUILayout.EndScrollView();

		GUILayout.EndArea();

		GUI.DragWindow();

	}
}