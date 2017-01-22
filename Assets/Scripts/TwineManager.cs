using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;

public class TwineManager : MonoBehaviour {
    // Use this for initialization
    int miCurrentDay;

    public ConversationManager mConversationManager;

	void Start () {
        miCurrentDay = 1;
        EndDay(Action.NoAction());
	}

    public void EndDay(Action xActionTaken)
    {
        Dictionary<string, Conversation> dRevConvs = LoadFlat("Revolutionaries", 2, miCurrentDay, xActionTaken);

        if(mConversationManager)
        {
            var allConversations = dRevConvs.Select(xConv => xConv.Value);
            mConversationManager.LoadConversations(allConversations.ToArray());
        }

        ++miCurrentDay;
    }

	Dictionary<string, Conversation> LoadFlat(string sFlatName, int iFlatNumber, int iDay, Action xActionTaken)
	{
		XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/" + sFlatName + ".html");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		
		Regex xNameMatcher = new Regex(@"^(?<day>\d+)\.(?<section>\d+):(?<room>[a-zA-Z]+)\d*");

        // For each room in the flat for this day we want a complete sequence 1...N
        Dictionary<string, List<Conversation>> dChainOfConvos = new Dictionary<string, List<Conversation>>();

		foreach(XmlNode xPassageNode in xNodeList)
		{
			string sPassageName = xPassageNode.Attributes["name"].Value;
			Match xMatch = xNameMatcher.Match(sPassageName);

			if(xMatch.Success)
			{
				int iDayOfPassage = System.Int32.Parse(xMatch.Groups["day"].Value);
				if(iDay == iDayOfPassage)
				{
                    int iSection = System.Int32.Parse(xMatch.Groups["section"].Value) - 1;

                    string sRoom = xMatch.Groups["room"].Value;
                    if(!dChainOfConvos.ContainsKey(sRoom))
                    {
                        dChainOfConvos[sRoom] = new List<Conversation>();
                    }

                    if(dChainOfConvos[sRoom].Count <= iSection)
                    {
                        // Resize the array to contain the value at iIndex
                        dChainOfConvos[sRoom].AddRange(Enumerable.Repeat<Conversation>(null, (1 + iSection) - dChainOfConvos[sRoom].Count)); 
                    }

                    string[] tags = xPassageNode.Attributes["tags"].Value.Split(new char[]{ ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    string sActionString = xActionTaken.GetActionTag();

                    bool bExactMatch = tags.Contains(sActionString) ||
                        tags.Length == 0 && xActionTaken.Equals(Action.NoAction());

                    if (dChainOfConvos[sRoom][iSection] == null)
                    {
                        if(tags.Length == 0 || bExactMatch)
                        {
                            dChainOfConvos[sRoom][iSection] = FindConversationByName(sFlatName, sPassageName);
                        }
                    }
                    else
                    {
                        if(bExactMatch)
                        {
                            dChainOfConvos[sRoom][iSection] = FindConversationByName(sFlatName, sPassageName);
                        }
                    }
				}
			}
			else
			{
				Debug.LogWarning("Could not parse tag name" + sPassageName + " in flat " + sFlatName);
			}
		}

        Dictionary<string, Conversation> dCombinedConversations = new Dictionary<string, Conversation>();
        foreach (KeyValuePair<string, List<Conversation>> kvpRoom in dChainOfConvos)
        {
            Conversation xCombinedConversation = ProcessRoom(kvpRoom.Key, kvpRoom.Value);
            dCombinedConversations.Add(kvpRoom.Key, xCombinedConversation);
        }

        return dCombinedConversations;
	}

    static Conversation FindConversationByName(string sFlatName, string sPassageName)
    {
        string sFileName = sPassageName.Replace('.', '_').Replace(':', '_');
        return Resources.Load<Conversation>("Generated/" + sFlatName + "/" + sFileName);
    }

    Conversation ProcessRoom(string sRoom, List<Conversation> aConversations)
    {
        if (aConversations.Any(x => x == null)) 
        {
            Debug.LogError("Room " + sRoom + " missing conversation indices:");
            int index = 0;
            foreach(Conversation xConv in aConversations)
            {
                if(xConv == null)
                {
                    Debug.LogError(index);
                }
                ++index;
            }
        }
        List<AudioClip> aAllClips = new List<AudioClip>();
        foreach(Conversation xConv in aConversations)
        {
            if (xConv != null) 
            {
                if(xConv.aConversation == null)
                {
                    xConv.aConversation = new AudioClip[] { };
                }
                aAllClips.AddRange(xConv.aConversation);
            }
        }

        Conversation xCombinedConversation = ScriptableObject.CreateInstance<Conversation>();
        xCombinedConversation.aConversation = aAllClips.ToArray();
        return xCombinedConversation;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
