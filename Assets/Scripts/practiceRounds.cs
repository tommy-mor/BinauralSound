
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class practiceRounds : UdonSharpBehaviour
{
    public GameObject hideRing;
    public GameObject spawnItem;
    public GameObject researcherPracticeText;
    public GameObject text;
    public GameObject controller;

    public Material startMat;

    public Material endMat;
    float sphereRadius = 37;
    private int trialSize = 100;
    private int[] trialArray = new int[4];


    private int soundOn = 0;
    private float timer = 0.0f;

    private GameObject[] created = new GameObject[1000];
    private int[] createdAlive = new int[1000];

    private int countNumberOfTrials = 0;
    private int timerLimit = 0;
    // states
    // 0 = beginner state (fill in the stimuli)
    // 1 = stimuli filled, waiting for trial start button to be pressed
    // 2 = in the random waiting time section
    // 3 = currently running (uhides, starts /updates timer)
    // 4 = done
    private int state = 0;
    private int[] trialsUsed = new int[8] { 10, 10, 10, 10, 10, 10, 10, 10 };
    public int trialNum = 100;
    // todo
    // do a L/R easy test to start
    // x make them change colors/change orientations
    // make the participant/runner separate
    // think of way to make them press button
    // x height limit of 75% radius
    // x move behavior of sound one into normal class
    // x handle collisions?
    // create object above us that can make more trials and etc, we are controlled by him.
    // x refactor,
    // add invisibility

    // x solution to sync problem: use synced variable random seed, so it generates all of the same things. send out network events. (good) will this work?
    // x OR hvae a set of nodes, just move them around using shared variables. (bad)
    //

    public void Start()
    {
        Debug.Log("starting");
        // var trialNum = 100;
        for (int i = 0; i < 1000; i++)
        {
            this.createdAlive[i] = 0;
            this.created[i] = null;
        }
        Random.InitState(Random.Range(0, 1000));
        getTrialType();





        m("This is the start of the practice rounds. There will be 16 test that ensure you are able to identify the target");

        Debug.Log("timerLimit" + timerLimit);
        // after these are done, some will collide and delete, which will happen in the next frame.
        // fill in the deleted ones 
    }
    public void getTrialType()
    {
        if (countNumberOfTrials % 2 == 0)
        {
            Debug.Log("check trial before");
            checkTrialToComplete(Random.Range(0, 8));
            Debug.Log("check trial after");
            if (trialNum == 0)
            {
                setTrialSize(50);
                soundOn = 0;

            }
            else if (trialNum == 1)
            {
                setTrialSize(100);
                soundOn = 1;
            }
            else if (trialNum == 2)
            {
                setTrialSize(200);
                soundOn = 0;
            }
            else if (trialNum == 3)
            {
                setTrialSize(400);
                soundOn = 1;
            }

        }

        Debug.Log("trialNum " + trialNum);
        Debug.Log("trialSize" + trialSize);


        timerLimit = Random.Range(2, 5);
    }
    public int checkTrialToComplete(int number)
    {
        for (int i = 0; i < trialsUsed.Length; i++)
        {
            if (trialsUsed[i] == number)
            {
                checkTrialToComplete(Random.Range(0, 8));
            }
            else
            {
                this.trialNum = number;
            }
        }

        for (int i = 0; i < trialsUsed.Length; i++)
        {
            if (trialsUsed[i] == 0)
            {
                trialsUsed[i] = number;
                break;
            }
        }
        return this.trialNum;


    }







    public void setTrialSize(int trialSize)
    {
        this.trialSize = trialSize;
    }
    private int frameCounter = 0;
    public void Update()
    {
        // Debug.Log(state);
        // Debug.Log(this.created.Length);
        // Debug.Log(this.created);
        if (state == 0) // we are still filling up the N nodes
        {
            // R("Sound on is " + soundOn + " number of stimuli " + trialSize);
            gameObject.GetComponent<Renderer>().material = startMat;
            if (countNotNull() < trialSize) // if statement, not while loop, cause while loop is the update cycle/game clock
            {
                hideRing.SetActive(true);

                // could get rid couldNotNull() looping through twice, but I don't care enough
                for (int i = 0; i < trialSize; i++)
                {
                    if (this.createdAlive[i] == 0)
                    {
                        this.created[i] = createPin(i);
                        // this.created[i].SetActive(false);
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
            for (int i = 0; i < trialSize; i++)
            {
                this.created[i].SetActive(false);
                // Debug.Log("i" + i);

            }
            if (soundOn == 0)
            {
                this.created[0].GetComponent<AudioSource>().volume = 1;
            }
            hideRing.SetActive(false);
            // LIUDAS
            // do nothing, in this stage we are waiting for button press

            // set button press text to ~ "press button to start the test, it will then 
            // on button press, send out network event 
            // for (int i = 0; i < this.created.Length; i++)
            // {
            //     this.created[i].SetActive(false);

            // }
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

            // we also need a timer for this stage (for data recording)
            for (int i = 0; i < trialSize; i++)
            {
                this.created[i].SetActive(true);

            }
        }
        else if (state == 4)
        {
            for (int i = 0; i < trialSize; i++)
            {
                this.created[i].SetActive(false);

            }
            gameObject.GetComponent<Renderer>().material = endMat;
        }
    }

    // this is where we launch the network actions.
    private void transitionTo(int newstate)
    {
        this.state = newstate;
        if (newstate == 0)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "EnteringState0");

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

    public void EnteringState0()
    {

        R("Sound on is " + soundOn + " number of stimuli " + trialSize);

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
        // for (int i = 0; i < this.created.Length; i++)
        // {
        //     this.created[i].SetActive(true);
        //     Debug.Log("activeSet");
        // }
        // TODO make stimuli visible, probably want to reset their timers too so that they blink in sync

        m(""); // this will get overwritten by the timer immediately
    }
    public void EnteringState4()
    {
        this.state = 4;

        m("you are now done with this trial! your time was " + this.timer);
        R("you are now done with this trial! your time was " + this.timer);
        gameObject.GetComponent<Renderer>().material = endMat;
    }

    public void EnteringState5()
    {
        // controller.GetComponent<Control>().signalDone();
        // m("wait for next trial");
        // gameObject.SetActive(false);
        // DestroyImmediate(gameObject);

    }



    private GameObject createPin(int count)
    {
        Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
        if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere

        if (onPlanet.y < sphereRadius * .75)
        {
            var newObject = spawnItem;
            if (count == 0)
            {
                if (onPlanet.z > 0) onPlanet.z = onPlanet.z * -1;
                var position = this.transform.position + onPlanet;
                var checkResult = Physics.OverlapSphere(position, spawnItem.GetComponent<CapsuleCollider>().radius);

                if (checkResult.Length == 0)
                {
                    newObject = VRCInstantiate(spawnItem);
                    newObject.transform.position = this.transform.position + onPlanet;
                    newObject.GetComponent<Cylinder1>().SetSpecial(count);
                    // newObject.SetActive(false);
                    newObject.GetComponent<AudioSource>().volume = 0;

                    // make this one the special one
                }
                else
                {
                    return null;
                }
            }

            else
            {
                var position = this.transform.position + onPlanet;
                var checkResult = Physics.OverlapSphere(position, spawnItem.GetComponent<CapsuleCollider>().radius);

                if (checkResult.Length == 0)
                {
                    newObject = VRCInstantiate(spawnItem);
                    newObject.transform.position = this.transform.position + onPlanet;
                    newObject.GetComponent<Cylinder1>().SetUnSpecial(count);

                }
                else
                {
                    return null;
                }
                // Debug.Log(newObject.activeSelf);




            }
            this.createdAlive[count] = 1;
            return newObject;
        }
        else
        {
            return null;
        }
    }




    public void deleteSquare(int idx)
    {
        //   //this.created.GetValue()
        //   if(this.created[idx] != null)
        //   {
        //       Destroy(this.created[idx]);
        //       //this.created[idx] = null;
        //   }
        //this.created[idx] = null;
        Debug.Log("idx to delete: " + idx);
        Debug.Log(createdAlive);
        Debug.Log(createdAlive.Length);

        Debug.Log(createdAlive[idx]);


        createdAlive[idx] = 0;
        // Destroy(this.created[idx]);
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
            // if we press the button while actually performing the task, that means that we are done with the task
            transitionTo(4);
            Debug.Log(state);


        }
        // transitioning to five is actually just deleting ourself, so that we may be born anew
        else if (state == 4)
        {
            deleteSquares();
            transitionTo(5);
            Debug.Log(state);
            countNumberOfTrials++;
            if (countNumberOfTrials == 16)
            {
                gameObject.SetActive(false);
                hideRing.SetActive(false);
                // Destroy(gameObject);
            }
            m("wait for next trial");
            Start();
            transitionTo(0);
        }

    }

    public void deleteSquares()
    {

        Debug.Log("nuddddmber of nulls" + countNotNull());



        if (this.created != null)
        {
            // foreach (var item in this.created)
            // {
            //     if (item == null)
            //     {
            //         Debug.Log(item);
            //         Destroy(item);
            //     }
            // }
            Debug.Log("deleting");
            for (int i = 0; i < 500; i++)
            {
                if (created[i] == null)
                {
                    // do nothing
                }
                else
                {
                    Debug.Log(created[i]);

                    Destroy(created[i]);
                    //created[i] = null;
                }
            }
        }

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

    private void m(string msg)
    {
        text.GetComponent<Text>().text = msg;
        // researcherPracticeText.GetComponent<Text>().text = msg;
    }
    private void R(string msg)
    {
        researcherPracticeText.GetComponent<Text>().text = msg;
    }
}
