
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Control : UdonSharpBehaviour
{
    public GameObject trial;

    private GameObject currentTrial;

    private int numberOfStimuli = 10;

    private bool ready = true;

    void Start()
    {

    }

    public override void Interact()
    {
        Debug.Log("interacted with cube");
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle");
    }

    public void Toggle()
    {
        Debug.Log("number of stimuli: " + this.numberOfStimuli);
        if (this.ready) // after a trial has reached its last stage, it becomes incactive
        {
            this.ready = false;
            numberOfStimuli = numberOfStimuli * 2;
            currentTrial = VRCInstantiate(trial);

            //Debug.Log("number of stimuli");
            currentTrial.GetComponent<spawnCube>().setTrialSize(numberOfStimuli);
            // TODO make setter that sets desirable properties about the trial, (its number of stimuli, timing settings, etc)
            //currentTrial.GetComponent<spawnCube>().setSettings(100, .4);
            currentTrial.transform.position = trial.transform.position;
            currentTrial.SetActive(true);
        }
        else
        {

            // do nothing, because a non-null trial means that there is a trial happening
        }
        //trial.SetActive(true);
    }

    // our child is signalling that we are done
    public void signalDone()
    {
        this.ready = true;
    }
}