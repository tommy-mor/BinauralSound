
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Control : UdonSharpBehaviour
{
    public GameObject trial;

    private GameObject currentTrial;

    private int numberOfStimuli = 10;

    void Start()
    {
        
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle");
    }

    public void Toggle()
    {
        Debug.Log("number of stimuli: " + this.numberOfStimuli);
        if (currentTrial == null || !currentTrial.activeSelf) // after a trial has reached its last stage, it becomes incactive
        {
            numberOfStimuli = numberOfStimuli * 2;
            currentTrial = VRCInstantiate(trial);
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
}
