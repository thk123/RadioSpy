using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Channel Down"))
        {
            GameObject xKnob = GameObject.Find("Knob");
            if (xKnob)
            {
                xKnob.transform.Rotate(new Vector3(0, -10, 0));
            }
        }
	}
}
