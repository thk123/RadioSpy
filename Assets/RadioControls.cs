using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControls : MonoBehaviour
{

    const float sfKnobStepRotationAngle = 5.0f;
    const float sfRadioIndicatorStepDistance = 0.125f;
    const float sfAnimationSpeed = 5.0f;

    uint muChannel = 0;
    const uint suMaxChannels = 24;
    GameObject mxKnob;
    GameObject mxRadioIndicator;

    float mfChannelTransition = 0;


    // Use this for initialization
    void Start ()
    {
        mxKnob = GameObject.Find("Knob");
        mxRadioIndicator = GameObject.Find("RadioIndicator");

    }

    // Update is called once per frame
    void Update()
    {
        if (mxKnob && mxRadioIndicator)
        {
            if (Input.GetButtonDown("Channel Down") && muChannel > 0)
            {
                muChannel--;
            }

            if (Input.GetButtonDown("Channel Up") && muChannel < suMaxChannels - 1)
            {
                muChannel++;
            }
        }

        float fTimeDelta = Time.deltaTime * sfAnimationSpeed;

        if (mfChannelTransition < muChannel - 0.05f)
        {

            mxKnob.transform.Rotate(new Vector3(0, sfKnobStepRotationAngle * fTimeDelta, 0));
            mxRadioIndicator.transform.Translate(new Vector3(sfRadioIndicatorStepDistance * fTimeDelta, 0, 0));
            mfChannelTransition += fTimeDelta;
        }        
        else if ( mfChannelTransition > muChannel + 0.05f)
        {
            mxKnob.transform.Rotate(new Vector3(0, -sfKnobStepRotationAngle * fTimeDelta, 0));
            mxRadioIndicator.transform.Translate(new Vector3(-sfRadioIndicatorStepDistance * fTimeDelta, 0, 0));
            mfChannelTransition -= fTimeDelta;

        }
        else if ( mfChannelTransition != muChannel)
        {
            mxKnob.transform.Rotate(new Vector3(0, sfKnobStepRotationAngle * (mfChannelTransition - muChannel), 0));
            mxRadioIndicator.transform.Translate(new Vector3(sfRadioIndicatorStepDistance * (mfChannelTransition - muChannel), 0, 0));
            mfChannelTransition = muChannel;
        }
    }
}
