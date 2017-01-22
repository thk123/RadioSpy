using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomRadioController : MonoBehaviour {

	public RadioTextDisplayer xTextDisplayer;

	struct Room
	{
		public string sFlat;
		public string sRoom;
	}

	Dictionary<int, Room> dRegistedRooms = new Dictionary<int, Room>();

	void Start () {
		xTextDisplayer = gameObject.GetComponent<RadioTextDisplayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterChannel(int iChannel, string sFlat, string sRoom)
	{
		dRegistedRooms[iChannel] = new Room { sFlat = sFlat, sRoom = sRoom };
	}

	public void DisplayChannel(int iChannel)
	{
		if(dRegistedRooms.ContainsKey(iChannel))
		{	
			Room sSelectedRoom = dRegistedRooms[iChannel];
			xTextDisplayer.DisplayText(sSelectedRoom.sFlat, sSelectedRoom.sRoom);
		}
		else
		{
			Debug.LogWarning("Unkown room for channel" + iChannel);
			xTextDisplayer.DisplayText("UNKNOWN", "UNKNOWN");
		}
	}
}
