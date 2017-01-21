using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

	Conversation[] aConversations;
	// Use this for initialization
	void Start () {
		aConversations = (Conversation[]) Resources.FindObjectsOfTypeAll(typeof(Conversation));
		LoadConversation(1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadConversation(int iHouseNumber, int iDay)
	{
		Conversation xConversation = aConversations[iHouseNumber - 1];
		StartCoroutine(PlayConversation(xConversation));
	}

	IEnumerator PlayConversation(Conversation xConversation) 
	{
	 	foreach(AudioClip statement in xConversation.aConversation)
	 	{
	 		AudioSource.PlayClipAtPoint(statement, Vector3.zero);
	 		yield return new WaitForSeconds(statement.length);
	 	}
	}
}
