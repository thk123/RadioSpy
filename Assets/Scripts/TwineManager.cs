using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TwineManager : MonoBehaviour {
	// Use this for initialization
	void Start () {
		XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/RadioSpy.html");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		print(xNodeList.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
