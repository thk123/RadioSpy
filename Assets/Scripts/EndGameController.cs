using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;

public class EndGameController : MonoBehaviour {

	public RadioTextDisplayer xTextDisplayer;

	// Use this for initialization
	void Start () {
		StringBuilder sbNames = new StringBuilder();
		int iButton = 0;
		foreach(Action.Names name in 
			Enum.GetValues(typeof(Action.Names)).Cast<Action.Names>())
		{
			if(name != Action.Names.None)
			{
				sbNames.Append("Press " + iButton.ToString() + " to arrest " + name.ToString());
				sbNames.Append("    ");
			}

			++iButton;
		}
		xTextDisplayer.DisplayText("Choose who to arrest",
			sbNames.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
