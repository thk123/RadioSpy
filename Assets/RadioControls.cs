using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControls : MonoBehaviour
{

    const float sfKnobStepRotationAngle = 5.0f;
    const float sfRadioIndicatorStepDistance = 0.125f;
    const float sfAnimationSpeed = 5.0f;

    public ConversationManager xConversationManager;
    public CurrentRoomRadioController xChannelDisplayer;

    int miChannel = 0;
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
            if (Input.GetButtonDown("Channel Down") && miChannel > 0)
            {
                miChannel--;
            }

            if (Input.GetButtonDown("Channel Up") && miChannel < suMaxChannels - 1)
            {
                miChannel++;
            }
        }

        float fTimeDelta = Time.deltaTime * sfAnimationSpeed;

        if (mfChannelTransition < miChannel - 0.05f)
        {

            mxKnob.transform.Rotate(new Vector3(0, sfKnobStepRotationAngle * fTimeDelta, 0));
            mxRadioIndicator.transform.Translate(new Vector3(sfRadioIndicatorStepDistance * fTimeDelta, 0, 0));
            mfChannelTransition += fTimeDelta;
        }        
        else if ( mfChannelTransition > miChannel + 0.05f)
        {
            mxKnob.transform.Rotate(new Vector3(0, -sfKnobStepRotationAngle * fTimeDelta, 0));
            mxRadioIndicator.transform.Translate(new Vector3(-sfRadioIndicatorStepDistance * fTimeDelta, 0, 0));
            mfChannelTransition -= fTimeDelta;

        }
        else if ( mfChannelTransition != miChannel)
        {
            mxKnob.transform.Rotate(new Vector3(0, sfKnobStepRotationAngle * (mfChannelTransition - miChannel), 0));
            mxRadioIndicator.transform.Translate(new Vector3(sfRadioIndicatorStepDistance * (mfChannelTransition - miChannel), 0, 0));
            mfChannelTransition = miChannel;
        }

        if(xConversationManager != null)
        {
            xConversationManager.TuneInToConversation(miChannel);
            if(xChannelDisplayer != null)
            {
                xChannelDisplayer.DisplayChannel(miChannel);
            }
        }
    }
}
