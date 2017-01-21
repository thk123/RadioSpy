using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class GenerateConversations
{
	[MenuItem("Assets/Generate/GenerateConversatiosn")]
    public static void CreateConversation()
    {
    	LoadConversationsForFlat("Revolutionaries");
        AssetDatabase.SaveAssets();
    }

    static void LoadConversationsForFlat(string sFlatName)
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
				Conversation asset = ScriptableObject.CreateInstance<Conversation>();

				string sFileName = sPassageName.Replace('.', '_');

        		AssetDatabase.CreateAsset(asset, 
        			"Assets/Conversations/Generated/" + sFlatName + "/" + sFileName + ".asset");

			}
			else
			{
				Debug.LogWarning("Could not parse tag name" + sPassageName + " in flat " + sFlatName);
			}
		}
    }
}
