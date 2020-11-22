
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
public class startTrial1 : UdonSharpBehaviour
{
    public GameObject trial;

    private GameObject currentTrial;

    private int numberOfStimuli = 1;

    private bool ready = true;

    void Start()
    {
        Debug.Log("number of stimuli: ");
    }

    public override void Interact()
    {
        Debug.Log("interacted with cube");
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle");
        // SendCustomNetworkEvent(NetworkEventTarget.All, "Toggle");
        Toggle();
    }

    public void Toggle()
    {
        // this.ready = true;
        Debug.Log("number of stimuli: " + this.numberOfStimuli);
        if (this.ready) // after a trial has reached its last stage, it becomes incactive
        {
            this.ready = false;
            // numberOfStimuli = numberOfStimuli * 2;
            // currentTrial = VRCInstantiate(trial);
            currentTrial = trial;

            //Debug.Log("number of stimuli");
            currentTrial.GetComponent<Trial1Actions>().setTrialSize(numberOfStimuli);
            // TODO make setter that sets desirable properties about the trial, (its number of stimuli, timing settings, etc)
            //currentTrial.GetComponent<spawnCube>().setSettings(100, .4);
            // currentTrial.transform.position = trial.transform.position;
            // Destroy(trial);
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
        Debug.Log(this.ready);
    }
}
