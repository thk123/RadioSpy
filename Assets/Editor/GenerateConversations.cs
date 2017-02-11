using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class GenerateConversations
{
    const string sGeneratedConversationsFolder = "Assets/Conversations/Resources/Generated";

    [MenuItem("Assets/Generate/GenerateConversations")]
    public static void CreateConversation()
    {
    	LoadConversationsForFlat("Revolutionaries");
    	LoadConversationsForFlat("FloristsStory");
    	LoadConversationsForFlat("TheAffair");
        AssetDatabase.SaveAssets();
    }

    static void LoadConversationsForFlat(string sFlatName)
    {
        if(!AssetDatabase.IsValidFolder(sGeneratedConversationsFolder + sFlatName))
        {
            AssetDatabase.CreateFolder(sGeneratedConversationsFolder, sFlatName);
        }

    	XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/Resources/" + sFlatName + ".html.xml");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		
		Regex xNameMatcher = new Regex(@"^(?<day>\d+)\.(?<section>\d+):(?<room>\w+)\d*");

		foreach(XmlNode xPassageNode in xNodeList)
		{        	
			string sPassageName = xPassageNode.Attributes["name"].Value;
			Match xMatch = xNameMatcher.Match(sPassageName);

			if(xMatch.Success)
			{
                string[] aLines = xPassageNode.InnerText.Split('\n');
                Regex xLineMatcher = new Regex(@"(\w):.*");
                List<Action.Names> aLineNames = new List<Action.Names>();
                foreach(string sLine in aLines)
                {
                    Match xLineMatch = xLineMatcher.Match(sLine);
                    if(xLineMatch.Success)
                    {
                        string sName = xLineMatch.Groups[1].Value;
                        if (Enum.IsDefined(typeof(Action.Names), sName))
                        {
                            Action.Names eName = (Action.Names)Enum.Parse(typeof(Action.Names), sName, true);
                            aLineNames.Add(eName);
                        }
                    }
                }
				Conversation asset = ScriptableObject.CreateInstance<Conversation>();

				string sFileName = sPassageName.Replace('.', '_').Replace(':','_');
                asset.sConversationName = sFileName;

                string[] aAssetIds= AssetDatabase.FindAssets("", new string[]{"Assets/Audio/Dialogue/" + sFlatName + "/" + sFileName} );
                Debug.Log("Found " + aAssetIds.Length + " clips for " + sFileName);

            	List<AudioClip> aConvos = new List<AudioClip>(aAssetIds.Length);
                
                if(aAssetIds.Length != aLineNames.Count)
                {
                    Debug.LogWarning("Non-equal number of names to lines " + aAssetIds.Length + " != " + aLineNames.Count + " for script " + sPassageName);
                }

            	foreach(string sAsset in aAssetIds)
            	{
            		string sFullPath = AssetDatabase.GUIDToAssetPath(sAsset);
            		aConvos.Add((AudioClip)AssetDatabase.LoadAssetAtPath(sFullPath, typeof(AudioClip)));
            	}
            	asset.aConversation = aConvos.ToArray();
                asset.aSpeaker = aLineNames.ToArray();

                AssetDatabase.CreateAsset(asset,
                    string.Join("/", new string[] { sGeneratedConversationsFolder, sFlatName, sFileName + ".asset" }));
			}
			else
			{
				Debug.LogWarning("Could not parse tag name" + sPassageName + " in flat " + sFlatName);
			}
		}


    }
}
