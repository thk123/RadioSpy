using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

	List<AudioSource> maConversations = new List<AudioSource>();
    List<Action.Names> maCurrentSpeaker = new List<Action.Names>();

	int iPlayingConversation;

	public int miActiveConversations
	{
		get;
		private set;
	}

    public delegate void SpeakerChangedEvent(int iRoomIndex, Action.Names eNewSpeaker);

    public event SpeakerChangedEvent OnSpeakerChanged;
	
	// Use this for initialization
	void Start () {
		iPlayingConversation = -1;
		miActiveConversations = 0;
	}

	public int LoadConversation(Conversation xConvo)
	{
		GameObject xAudioSourceObject = new GameObject();
		AudioSource xAudioSource = xAudioSourceObject.AddComponent<AudioSource>();
		xAudioSource.mute = true;
		maConversations.Add(xAudioSource);
        maCurrentSpeaker.Add(Action.Names.None);
        StartCoroutine(PlayConversation(xConvo, xAudioSource, maConversations.Count - 1));

		++miActiveConversations;

		return maConversations.Count - 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TuneInToConversation(int iHouseNumber)
	{
		if(iHouseNumber != iPlayingConversation)
		{
			if(iPlayingConversation != -1)
			{
				maConversations[iPlayingConversation].mute = true;
			}
			if(iHouseNumber < maConversations.Count)
			{
				maConversations[iHouseNumber].mute = false;
				iPlayingConversation = iHouseNumber;
			}
		
		}
	}

	IEnumerator PlayConversation(Conversation xConversation, AudioSource xSource, int iHouseNumber) 
	{
        int iIndex = 0;
        
	 	foreach(AudioClip statement in xConversation.aConversation)
	 	{
	 		xSource.clip = statement;
	 		xSource.Play();
            maCurrentSpeaker[iHouseNumber] = xConversation.aSpeaker[iIndex];
            OnSpeakerChanged(iHouseNumber, xConversation.aSpeaker[iIndex]);

	 		yield return new WaitForSeconds(statement.length);

            ++iIndex;
	 	}

	 	--miActiveConversations;
	}

    public Action.Names GetCurrentSpeaker(int iRoomIndex)
    {
        return maCurrentSpeaker[iRoomIndex];
    }
}
