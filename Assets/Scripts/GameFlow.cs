using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour {

	// Use this for initialization
	public RadioTextDisplayer mxRadioTextDisplayer;
	public TextAsset mxBreifing;
	void Start () {
		StartCoroutine(RunGame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator RunGame()
	{
		mxRadioTextDisplayer.DisplayText("RadioTap - Press SPACE to start", 
			mxBreifing.text.Replace(Environment.NewLine, " "));
		while (!Input.GetKeyDown(KeyCode.Space))
		{
         	yield return null;
        }

        var xCurrentRoomRadioController = 
        	mxRadioTextDisplayer.gameObject.AddComponent<CurrentRoomRadioController>();

        gameObject.GetComponent<RadioControls>().xChannelDisplayer = xCurrentRoomRadioController;

        TwineManager xTwineManager = gameObject.GetComponent<TwineManager>();
        xTwineManager.Begin(xCurrentRoomRadioController);

        while(!xTwineManager.bGameFinished)
        {
        	yield return null;
        }

        Destroy(xCurrentRoomRadioController);

        var xEndGameController = 
        	mxRadioTextDisplayer.gameObject.AddComponent<EndGameController>();

        xEndGameController.xTextDisplayer = mxRadioTextDisplayer;

        yield return null;
	}
}
