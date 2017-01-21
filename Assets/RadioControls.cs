using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControls : MonoBehaviour
{

    const float sfKnobStepRotationAngle = 10.0f;
    const float sfRadioIndicatorStepDistance = 0.5f;

    uint muChannel = 0;
    const uint suMaxChannels = 6; 

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        GameObject xKnob = GameObject.Find("Knob");
        GameObject xRadioIndicator = GameObject.Find("RadioIndicator");
        if (xKnob)
        {
            if (Input.GetButtonDown("Channel Down") && muChannel > 0)
            {
                xKnob.transform.Rotate(new Vector3(0, -sfKnobStepRotationAngle, 0));
                xRadioIndicator.transform.Translate(new Vector3(-sfRadioIndicatorStepDistance, 0, 0));
                muChannel--;
            }

            if (Input.GetButtonDown("Channel Up") && muChannel < suMaxChannels - 1 )
            {
                xKnob.transform.Rotate(new Vector3(0, sfKnobStepRotationAngle, 0));
                xRadioIndicator.transform.Translate(new Vector3(sfRadioIndicatorStepDistance, 0, 0));
                muChannel++;
            }
        }
	}
}
