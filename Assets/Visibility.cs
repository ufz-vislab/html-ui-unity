using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour, IJson
{

	public string name;
	public bool enabled;
	public float opacity;

	void Start ()
	{
		Debug.Log (GetJson ());
	}

	public string GetJson ()
	{
		var className = GetType ().Name.ToLower ();
		var id = GetInstanceID ();
		var json = "{\"type\": \"" + className + "\", \"id\": " + id + ", \"attributes\": " + JsonUtility.ToJson (this) + "}";
		Debug.Log (json);
		return json;
	}

	public void SetJson (string json)
	{
		JsonUtility.FromJsonOverwrite (json, this);
	}
}
