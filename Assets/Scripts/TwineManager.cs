using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;

public class TwineManager : MonoBehaviour {
    // Use this for initialization
    int miCurrentDay;

 	[Serializable]
    public struct FillerEntry
    {
    	public string name;
    	public AudioClip clip;
    }
    public ConversationManager mConversationManager;
    CurrentRoomRadioController mCurrentRoomController;
    public List<FillerEntry> maFillers;

    public bool bGameFinished
    {
    	get;
    	private set;
    }

    Dictionary<string, AudioClip> mdFillerClips = new Dictionary<string, AudioClip>();

	void Start () {
        if(maFillers!=null)
        {
        	foreach(FillerEntry entry in maFillers)
        	{
        		mdFillerClips.Add(entry.name, entry.clip);
        	}
        }
	}

	// Update is called once per frame
    void Update () {
		if(mConversationManager.miActiveConversations == 0)		
		{
			bGameFinished = true;
		}
	}

	public void Begin(CurrentRoomRadioController xCurrentRoomController)
	{
		bGameFinished = false;
		mCurrentRoomController = xCurrentRoomController;
		miCurrentDay = 1;
        EndDay(Action.NoAction());
	}

    public void EndDay(Action xActionTaken)
    {
        EndDayForFlat(xActionTaken, "Revolutionaries", 2);
        ++miCurrentDay;
    }

    void EndDayForFlat(Action xActionTaken, string sFlatName, int iFlatNumber)
    {
    	Dictionary<string, Conversation> dRevConvs = LoadFlat(sFlatName, iFlatNumber, miCurrentDay, xActionTaken);
        if(mConversationManager)
        {
        	foreach(KeyValuePair<string, Conversation> kvpConv in dRevConvs)
        	{
            	int iChannel = mConversationManager.LoadConversation(kvpConv.Value);

            	if(mCurrentRoomController)
        		{
    				print("Reegistering room " + kvpConv.Key + "  " + iChannel.ToString());
        			mCurrentRoomController.RegisterChannel(iChannel, "Flat " + iFlatNumber.ToString(), 
        				kvpConv.Key);
    	    	}
        	}
        }
    }

	Dictionary<string, Conversation> LoadFlat(string sFlatName, int iFlatNumber, int iDay, Action xActionTaken)
	{
		XmlDocument xXml = new XmlDocument();
		xXml.Load("Assets/Twine/" + sFlatName + ".html");
		XmlNodeList xNodeList = xXml.GetElementsByTagName("tw-passagedata");
		
		Regex xNameMatcher = new Regex(@"^(?<day>\d+)\.(?<section>\d+):(?<room>[a-zA-Z]+)\d*");

        // For each room in the flat for this day we want a complete sequence 1...N
        Dictionary<string, List<Conversation>> dChainOfConvos = new Dictionary<string, List<Conversation>>();
        Dictionary<string, List<AudioClip>> fillerSounds = new Dictionary<string, List<AudioClip>>();

        int iNumSectionsTotal = 0;

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
                    iNumSectionsTotal = Mathf.Max(iSection, iNumSectionsTotal);

                    string sRoom = xMatch.Groups["room"].Value;
                    if(!dChainOfConvos.ContainsKey(sRoom))
                    {
                        dChainOfConvos[sRoom] = new List<Conversation>();
                        fillerSounds[sRoom] = new List<AudioClip>();
                    }

                    if(dChainOfConvos[sRoom].Count <= iSection)
                    {
                        // Resize the array to contain the value at iIndex
                        dChainOfConvos[sRoom].AddRange(Enumerable.Repeat<Conversation>(null, (1 + iSection) - dChainOfConvos[sRoom].Count)); 
                        fillerSounds[sRoom].AddRange(Enumerable.Repeat<AudioClip>(null, (1 + iSection) - fillerSounds[sRoom].Count)); 
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

                    Regex xFillerAudioFinder = new Regex(@"FillerAudio:(?<audioname>\w+)");
                    AudioClip xFillerClip = mdFillerClips["Silence"];
                    if(xPassageNode.InnerText != null)
                    {
                    	Match m = xFillerAudioFinder.Match(xPassageNode.InnerText);
                    	if(m.Success)
                    	{
                    		string sFillerName = m.Groups["audioname"].Value;
                    		
                    		if(mdFillerClips.ContainsKey(sFillerName))
                    		{
                    			xFillerClip = mdFillerClips[sFillerName];
                    		}
                    		else
                    		{
                    			Debug.LogWarning("No filler clip for audio name " + sFillerName);
                    		}
                    	}
                    }
                    fillerSounds[sRoom][iSection] = xFillerClip;
				}
			}
			else
			{
				Debug.LogWarning("Could not parse tag name" + sPassageName + " in flat " + sFlatName);
			}
		}

		for(int i = 0; i < iNumSectionsTotal; ++i)
		{
			float fMaxLength = 0.0f;
			foreach(string sRoom in dChainOfConvos.Keys)
			{
				Conversation xConvoInRoomAtTime = dChainOfConvos[sRoom][i];
				float fDuration = xConvoInRoomAtTime.aConversation.Sum(x => x.length);
				fMaxLength = Mathf.Max(fMaxLength, fDuration);
			}

			foreach(string sRoom in dChainOfConvos.Keys)
			{
				Conversation xConvoInRoomAtTime = dChainOfConvos[sRoom][i];
				float fDuration = xConvoInRoomAtTime.aConversation.Sum(x => x.length);
				float fFillerRequired = fMaxLength - fDuration;

				if(fFillerRequired > 0.0f)
				{
					AudioClip fillerClip = mdFillerClips["Silence"];
					if(fillerSounds.ContainsKey(sRoom))
					{						
						if(fillerSounds[sRoom][i] != null)
						{
							fillerClip = fillerSounds[sRoom][i];
						}
					}

					int iNumSeconds = (int)Mathf.Ceil(fFillerRequired / fillerClip.length );
					
					List<AudioClip> aFilledConvo = new List<AudioClip>(xConvoInRoomAtTime.aConversation);
					aFilledConvo.AddRange(Enumerable.Repeat<AudioClip>(fillerClip, iNumSeconds));
					xConvoInRoomAtTime.aConversation = aFilledConvo.ToArray();
				}

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
}
