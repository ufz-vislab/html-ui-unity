using UnityEngine;
using System.Collections;
using WebSocketSharp;

public class Server : MonoBehaviour
{
	private WebSocketSharp.Server.WebSocketServer wssv;
	private WebSocket ws;

	// Use this for initialization
	void Start ()
	{
		var ugly = UnityThreadHelper.Dispatcher;

		wssv = new WebSocketSharp.Server.WebSocketServer ("ws://localhost:7000/");
		wssv.AddWebSocketService<Vis> ("/", () => new Vis () { IgnoreExtensions = true });
		wssv.AddWebSocketService<Laputa> (
			"/Laputa",
			() => new Laputa () {
				IgnoreExtensions = true
			});
		;
		wssv.Start ();
		Debug.Log ("Is listening: " + wssv.IsListening + " on port " + wssv.Port);

		ws = new WebSocket ("ws://localhost:7000/Laputa");

		ws.OnMessage += (sender, e) =>
			Debug.Log ("Laputa says: " + e.Data);

		ws.Connect ();
	}

	void OnApplicationQuit ()
	{
		if (ws != null && ws.ReadyState == WebSocketState.Open)
			ws.Close ();
		if (wssv != null && wssv.IsListening)
			wssv.Stop ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//ws.Send ("BALUS");
	}

	//static public IJson GetVis ()
	//{
	//return GameObject.FindObjectOfType<Visibility> ();
	//}

}

public class Vis : WebSocketSharp.Server.WebSocketBehavior
{
	protected override void OnMessage (MessageEventArgs e)
	{
		Debug.Log (e.Data);
		if (e.Data == "GET") {
			UnityThreadHelper.Dispatcher.Dispatch (() => {
				var vis = GameObject.FindObjectOfType<Visibility> ();
				Debug.Log ("Main Thread!");
				Send (vis.GetJson ());
			});
			
			//var vis = Server.GetVis ();
			//vis.GetJson()

			//Send("{\"Name\":\"Foo\",\"Enabled\":true,\"Opacity\":0.5}");
			//Send ("{\n          \"type\": \"visibility\",\n        \"id\": 5,\n           \"attributes\": {\n             \"name\": \"Grand Old Mansion\",\n            \"enabled\": true,\n            \"opacity\": 0.5\n          }\n}");
		}
		//var msg = e.Data == "BALUS"
		//	? "I've been balused already..."
		//	: "I'm not available now.";

		//Send (msg);
	}
}

public class Laputa : WebSocketSharp.Server.WebSocketBehavior
{
	protected override void OnMessage (MessageEventArgs e)
	{
		//Debug.Log (e.Data);
		var msg = e.Data == "BALUS"
			? "I've been balused already..."
			: "I'm not available now.";

		Send (msg);
	}
}