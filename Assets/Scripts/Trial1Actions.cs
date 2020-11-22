
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


public class Trial1Actions : UdonSharpBehaviour
{
    //  public GameObject player;
    public GameObject trial1Text;
    public GameObject researcherTrial1Text;

    public GameObject spawnTrial1Cube;
    public GameObject controller;

    public Material startMat;

    public Material endMat;

    private GameObject[] created = new GameObject[1000];
    private int[] createdAlive = new int[1000];
    private float timer = 0.0f;
    private int state = 0;
    public int trialSize = 1;
    public int specialCube = 0;
    private int countNumberOfTrials = 0;
    private int timerLimit = 0;
    private int[] trialsUsed = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    public int trialNum = 100;
    private int soundOn = 0;
    public void Start()
    {

        // player = GameObject.FindGameObjectWithTag("MainCamera");
        for (int i = 0; i < 1000; i++)
        {
            this.createdAlive[i] = 0;
            this.created[i] = null;
        }
        timerLimit = Random.Range(2, 5);
        Debug.Log("timer Limit + " + timerLimit);

        // after these are done, some will collide and delete, which will happen in the next frame.
        // fill in the deleted ones 
    }


    public void setTrialSize(int trialSize)
    {
        this.trialSize = trialSize;
    }

    private int countNotNull()
    {
        //Debug.Log("logging the array");
        //Debug.Log(this.created);
        int count = 0;
        for (int i = 0; i < this.createdAlive.Length - 1; i++)
        {
            if (this.createdAlive[i] == 1)
            {
                count++;
            }
        }
        return count;
    }

    private int frameCounter = 0;
    public void Update()
    {

        if (state == 0) // we are still filling up the N nodes
        {


            m("This is the start of the practice hearing test to ensure your headphones can hear the object and are loud enough. Please stare ahead at the white dot and verbally say either left or right depending on which side the tone is coming from");
            gameObject.GetComponent<Renderer>().material = startMat;
            if (countNotNull() < trialSize) // if statement, not while loop, cause while loop is the update cycle/game clock
            {

                // could get rid couldNotNull() looping through twice, but I don't care enough
                for (int i = 0; i < trialSize; i++)
                {
                    Debug.Log("i" + i);
                    if (this.createdAlive[i] == 0)
                    {
                        this.created[i] = createPracticeObject(i);
                        specialCube = i;
                        Debug.Log("Special Cube" + specialCube);
                        break; // try only doing one per frame?
                    }
                }
                // let a frame run
            }
            else
            {
                // make sure the finished state is stable?
                this.frameCounter++;
                if (this.frameCounter > 20)
                {
                    // we have enough nodes, transition to state

                    transitionTo(1);
                }

            }
        }
        else if (state == 1)
        {
            // LIUDAS
            // do nothing, in this stage we are waiting for button press

            // set button press text to ~ "press button to start the test, it will then 
            // on button press, send out network event 
        }
        else if (state == 2)
        {

            this.timer += Time.deltaTime;

            // countdown timer random seconds until transition to next stage (actual testing stage)
            if (this.timer > timerLimit)
            {
                transitionTo(3);

            }
        }
        else if (state == 3)
        {
            // actual testing stage
            this.timer += Time.deltaTime;

            this.created[specialCube].SetActive(true);
            m("");

            // we also need a timer for this stage (for data recording)
        }
        else if (state == 4)
        {
            this.created[specialCube].SetActive(false);
        }

    }


    // this is where we launch the network actions.
    private void transitionTo(int newstate)
    {
        this.state = newstate;
        if (newstate == 0)
        {

        }
        else if (newstate == 1)
        {
            m("Please face forward until the test starts. Press button to start your random 1-5 second countdown, then start your trial");

        }
        else if (newstate == 2)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "EnteringState2");

        }
        else if (newstate == 3)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "EnteringState3");

        }
        else if (newstate == 4)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "EnteringState4");
        }
        else if (newstate == 5)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "EnteringState5");
        }
    }


    // don't call these directly, they are called by the SendCustomNetworkEvent method, which calls this
    // method on the client of the person who pressed the button AND the one who's watching
    public void EnteringState2()
    {
        this.timer = 0.0f;
        this.state = 2;
        m("Your trial will begin when these words disappear");
    }

    public void EnteringState3()
    {
        this.timer = 0.0f;
        this.state = 3;
        m("");

        // TODO make stimuli visible, probably want to reset their timers too so that they blink in sync

        // m("you are now testing! good luck"); // this will get overwritten by the timer immediately
    }


    public void EnteringState4()
    {
        this.state = 4;

        // m("you are now done with this trial! your time was " + this.timer);
        m("you are now done with this trial! your time was " + this.timer);
        R("you are now done with this trial! your time was " + this.timer);
    }


    public void EnteringState5()
    {
        Debug.Log("destry");
        // Destroy(this.created[specialCube]);
        Debug.Log("countNumberOf Trials: " + countNumberOfTrials);
        controller.GetComponent<startTrial1>().signalDone();




        countNumberOfTrials++;
        if (countNumberOfTrials == 8)
        {
            gameObject.SetActive(false);
        }

    }







    float sphereRadius = 37;

    private GameObject createPracticeObject(int count)
    {

        Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
        if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere

        // flip lower hemisphere

        if (onPlanet.y < sphereRadius * .75 && onPlanet.z < 0)
        {
            var newObject = VRCInstantiate(spawnTrial1Cube);
            newObject.transform.position = this.transform.position + onPlanet;
            // newObject.SetActive(true);
            // newObject.SetActive(true);
            // if (count == 0)
            // {
            //     newObject.GetComponent<Cylinder1>().SetSpecial(count);
            //     // make this one the special one
            // }
            // else
            // {
            //     newObject.GetComponent<Cylinder1>().SetUnSpecial(count);

            // }
            this.createdAlive[count] = 1;
            return newObject;
        }
        else
        {
            return null;
        }



    }

    public override void Interact()
    {
        if (state == 0)
        {
            Debug.Log(state);
        }
        else if (state == 1)
        {
            transitionTo(2);
            Debug.Log(state);
        }
        else if (state == 2)
        {
            Debug.Log(state);
            // presssing button does nothing in waiting for time section.

        }
        else if (state == 3)
        {
            Debug.Log(state);
            // if we press the button while actually performing the task, that means that we are done with the task
            transitionTo(4);

        }
        // transitioning to five is actually just deleting ourself, so that we may be born anew
        else if (state == 4)
        {
            Debug.Log(state);
            transitionTo(5);

            Destroy(this.created[specialCube]);
            Debug.Log("startTrial1");
            controller.GetComponent<startTrial1>().signalDone();
            Start();
            transitionTo(0);

        }
    }

    private void m(string msg)
    {
        trial1Text.GetComponent<Text>().text = msg;
    }
    private void R(string msg)
    {
        researcherTrial1Text.GetComponent<Text>().text = msg;
    }

}
