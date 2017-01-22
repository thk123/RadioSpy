using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

	List<AudioSource> maConversations;

	int iPlayingConversation;
	
	// Use this for initialization
	void Start () {
		iPlayingConversation = -1;
	}

	public void LoadConversations(Conversation[] aConversations)
	{
		maConversations = new List<AudioSource>();
		print("Loading from " + aConversations.Length);
		foreach(Conversation xConvo in aConversations)
		{
			GameObject xAudioSourceObject = new GameObject();
			AudioSource xAudioSource = xAudioSourceObject.AddComponent<AudioSource>();
			xAudioSource.mute = true;
			maConversations.Add(xAudioSource);
			StartCoroutine(PlayConversation(xConvo, xAudioSource));
		}
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

	IEnumerator PlayConversation(Conversation xConversation, AudioSource xSource) 
	{
	 	foreach(AudioClip statement in xConversation.aConversation)
	 	{
	 		xSource.clip = statement;
	 		xSource.Play();
	 		yield return new WaitForSeconds(statement.length);
	 	}
	}
}
