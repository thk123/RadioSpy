using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TwineManager : MonoBehaviour {

	public AudioClip[] aConversation;
	// Use this for initialization
	void Start () {
		XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/RadioSpy.html");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		print(xNodeList.Count);

		LoadConversation(1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadConversation(int iHouseNumber, int iDay)
	{
		StartCoroutine(PlayConversation());
	}

	 IEnumerator PlayConversation () 
	 {
	 	foreach(AudioClip statement in aConversation)
	 	{
	 		print("Starting clip" + statement.ToString());
	 		AudioSource.PlayClipAtPoint(statement, Vector3.zero);
	 		yield return new WaitForSeconds(statement.length);
	 	}
	 }
}
