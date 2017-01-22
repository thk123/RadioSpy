using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;

public class EndGameController : MonoBehaviour {

	public RadioTextDisplayer xTextDisplayer;

	HashSet<Action.Names> maArrested;
	bool mbDecisionsSelected;

	// Use this for initialization
	void Start () {
		mbDecisionsSelected = false;
		maArrested = new HashSet<Action.Names>();
		StartCoroutine(GameFinishFlow());
	}
	
	// Update is called once per frame
	void Update () {
		if(!mbDecisionsSelected)
		{
			if(Input.GetKeyDown(KeyCode.Alpha0))
			{
				maArrested.Add((Action.Names)0);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)0).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				maArrested.Add((Action.Names)1);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)1).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				maArrested.Add((Action.Names)2);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)2).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				maArrested.Add((Action.Names)3);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)3).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				maArrested.Add((Action.Names)4);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)4).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha5))
			{
				maArrested.Add((Action.Names)5);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)5).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha6))
			{
				maArrested.Add((Action.Names)6);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)6).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha7))
			{
				maArrested.Add((Action.Names)7);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)7).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha8))
			{
				maArrested.Add((Action.Names)8);		
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)8).ToString() + " arrested");
			}
			if(Input.GetKeyDown(KeyCode.Alpha9))
			{
				maArrested.Add((Action.Names)9);
				xTextDisplayer.DisplayText("Arrested", ((Action.Names)9).ToString() + " arrested");
			}
		}
	}

	IEnumerator GameFinishFlow()
	{
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


		while (!Input.GetKeyDown(KeyCode.Space))
		{
         	yield return null;
        }

        mbDecisionsSelected = true;

        StringBuilder newsTickerTape = new StringBuilder();

        foreach(Action.Names eName in maArrested)
        {
        	var guilty = new HashSet<Action.Names>{ Action.Names.F, Action.Names.G, Action.Names.D };
        	bool bInnocent = !guilty.Contains(eName);
        	newsTickerTape.Append(eName.ToString() + " Arrested: " + (bInnocent ? "INNOCENT" : "GUILTY") + "  --  ");
    	}

    	if(!maArrested.Contains(Action.Names.D))
    	{
			newsTickerTape.Append("THE LEADER ASSINATED BY " + Action.Names.D.ToString());
    	}
    	xTextDisplayer.DisplayText("Breaking News", newsTickerTape.ToString());
	}
}
