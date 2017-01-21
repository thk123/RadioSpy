using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;

public class TwineManager : MonoBehaviour {
	// Use this for initialization
	void Start () {
		LoadFlat("Revolutionaries", 2, 1, Action.NoAction());
	}

	Dictionary<string, Conversation> LoadFlat(string sFlatName, int iFlatNumber, int iDay, Action xActionTaken)
	{
		XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/" + sFlatName + ".html");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		
		Regex xNameMatcher = new Regex(@"^(?<day>\d+)\.(?<section>\d+):(?<room>\w+)\d*");

		foreach(XmlNode xPassageNode in xNodeList)
		{
			string sPassageName = xPassageNode.Attributes["name"].Value;
			Match xMatch = xNameMatcher.Match(sPassageName);

			if(xMatch.Success)
			{
				int iDayOfPassage = System.Int32.Parse(xMatch.Groups["day"].Value);
				if(iDay == iDayOfPassage)
				{
					sActionString = xActionTaken.ToString();
					IEnumable<string> tags = xPassageNode.Attributes["tags"].Value.Split(' ');
					if(tags == sActionString)
					{

					}
				}
				int iSection = System.Int32.Parse(xMatch.Groups["section"].Value);
				string sRoom = xMatch.Groups["room"].Value;
			}
			else
			{
				Debug.LogWarning("Could not parse tag name" + sPassageName + " in flat " + sFlatName);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
