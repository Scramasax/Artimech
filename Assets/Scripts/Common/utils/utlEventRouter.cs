// Copyright (c) 2009 David Koontz and Dan Peschman
// Please direct any bugs/comments/suggestions to david@koontzfamily.org
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 	Class for broadcasting messages to subscribers.  Clients of the EventRouter register 
/// 	themselves by calling Subscribe and passing in the even they are interested in along with
/// 	a delegate to be called back when the event is received.  Events are published via the 
/// 	Publish method which allows arbitrary data to be passed along with the event to be interpreted
/// 	by the event receiver.  Events do not have to be pre-defined.
/// </summary>
/// <example>
/// 	Sender.cs
///		<code>
///		using UnityEngine;
///		using System.Collections;
///		
///		public enum SenderEvent 
/// 	{
///			Test
///		}
///		
///		public class Sender : MonoBehaviour 
/// 	{
///			void Start () 
/// 		{
///				EventRouter.Publish(SenderEvent.Test, this, "Hello World");
///			}
///		}
/// 	</code>
/// </example>
/// <example>
/// 	Receiver.cs
/// 	<code>
/// 	using UnityEngine;
///		using System.Collections;
///		
///		public class Receiver : MonoBehaviour
///		{
///			void Awake()
///			{
///				EventRouter.Subscribe(SenderEvent.Test, OnSenderEvent);
///			}
///			
///			void OnSenderEvent(EventRouter.Event evt)
///			{
///				if(evt.HasData) 
/// 			{
///					Debug.Log("Received event: " + evt.Type + " from: " + evt.Sender.name + " with data: " + evt.Data[0]);
///				}
///				else 
/// 			{
///					Debug.Log("Received event: " + evt.Type + " from: " + evt.Sender.name + " with no data");
///				}
///			}
///		}
/// 	</code>
/// </example>
public class utlEventRouter {

    public enum EventCode
    {
        Standard,
        Simulation,
        AI,
        GUI,
        System
    };

    /// <summary>
    /// 	Event data class, passed in to the subscriber whenever an event occours.
    /// </summary>
    public class utlEvent {
		public string Type;
		public Component Sender;
		public object[] Data;
		
		public bool HasData {
			get { return Data != null && Data.Length > 0;}
		}
	}
	
	public delegate void Handler(utlEvent e);
	
	private static Dictionary<string, Delegate> handlers = new Dictionary<string, Delegate>();
	
	
	/// <summary>
	/// Subscribe to the event specified by evt.  Pass in a delegate to be called back when the even occurs.
	/// </summary>
	/// <param name='evt'>
	/// The event enumeration value.
	/// </param>
	/// <param name='h'>
	/// The delegate to be called when the even occurs.
	/// </param>
	public static void Subscribe(Enum evt, Handler h) {
		Subscribe(evt.ToString(), h);
	}
	
	/// <summary>
	/// Subscribe to the event specified by evt.  Pass in a delegate to be called back when the even occurs.
	/// </summary>
	/// <param name='evt'>
	/// The string representing the event.
	/// </param>
	/// <param name='h'>
	/// The delegate to be called when the even occurs.
	/// </param>
	public static void Subscribe(string evt, Handler h) {
		if (!handlers.ContainsKey(evt)) {
			handlers.Add(evt, null);
		}
		
		handlers[evt] = (Handler)handlers[evt] + h;
	}
	
	/// <summary>
	/// Unsubscribe the specified delegate from the event.
	/// </summary>
	/// <param name='evt'>
	/// The event enumeration value.
	/// </param>
	/// <param name='h'>
	/// The delegate to be removed from the event handlers.
	/// </param>
	public static void Unsubscribe(Enum evt, Handler h) {
		Unsubscribe(evt.ToString(), h);
	}
	
	/// <summary>
	/// Unsubscribe the specified delegate from the event.
	/// </summary>
	/// <param name='evt'>
	/// The string representing the event.
	/// </param>
	/// <param name='h'>
	/// The delegate to be removed from the event handlers.
	/// </param>
	public static void Unsubscribe(string evt, Handler h) {
		if (handlers.ContainsKey(evt)) {
			handlers[evt] = (Handler)handlers[evt] - h;
			
			if (handlers[evt] == null) {
				handlers.Remove(evt);
			}
		}
	}
	
	/// <summary>
	/// Publish the specified event with no extra data.
	/// </summary>
	/// <param name='evt'>
	/// The event enumeration value.
	/// </param>
	/// <param name='sender'>
	/// The event's sender, usually 'this'.
	/// </param>
	public static void Publish(Enum evt, Component sender) {
		Publish(evt.ToString(), sender, null);
	}
	
	/// <summary>
	/// Publish the specified event with extra data.
	/// </summary>
	/// <param name='evt'>
	/// The event enumeration value.
	/// </param>
	/// <param name='sender'>
	/// The event's sender, usually 'this'.
	/// </param>
	/// <param name='data'>
	/// An arbitrary params array of objects to be interpreted by the receiver of the event.
	/// </param>
	public static void Publish(Enum evt, Component sender, params object[] data) {
		Publish(evt.ToString(), sender, data);
	}
	
	/// <summary>
	/// Publish the specified event with no extra data.
	/// </summary>
	/// <param name='evt'>
	/// The string representing the event.
	/// </param>
	/// <param name='sender'>
	/// The event's sender, usually 'this'.
	/// </param>
	public static void Publish(string evt, Component sender) {
		Publish(evt, sender, null);
	}
	
	/// <summary>
	/// Publish the specified event with extra data.
	/// </summary>
	/// <param name='evt'>
	/// The string representing the event.
	/// </param>
	/// <param name='sender'>
	/// The event's sender, usually 'this'.
	/// </param>
	/// <param name='data'>
	/// An arbitrary params array of objects to be interpreted by the receiver of the event.
	/// </param>
	public static void Publish(string evt, Component sender, params object[] data) {
		Delegate d;
		if (handlers.TryGetValue(evt, out d)){
			utlEvent e = new utlEvent();
			e.Type = evt;
			e.Sender = sender;
			if (data != null && data.Length > 0)
				e.Data = data;
				
			Handler h = (Handler)d;
			h(e);
		}
	}
	
	/// <summary>
	/// Clear all event subscribers, used primarily when switching or resetting a level.
	/// </summary>
	public static void Clear() {
		handlers.Clear();
	}
}