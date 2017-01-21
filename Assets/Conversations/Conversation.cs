using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation/New Conversation", order = 1)]
public class Conversation : ScriptableObject {

	public AudioClip[] aConversation;	
	public string sConversationName;
}