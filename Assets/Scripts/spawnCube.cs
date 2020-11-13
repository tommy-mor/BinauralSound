
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class spawnCube : UdonSharpBehaviour
{
    public GameObject spawnItem;
    public GameObject audioObject;
    public GameObject text;
    public GameObject controller;


    float sphereRadius = 10;
    private int trialSize = 100; 
    private float timer = 0.0f;

    private GameObject[] created = new GameObject[1000];
    private int[] createdAlive = new int[1000];



    // states
    // 0 = beginner state (fill in the stimuli)
    // 1 = stimuli filled, waiting for trial start button to be pressed
    // 2 = in the random waiting time section
    // 3 = currently running (uhides, starts /updates timer)
    // 4 = done
    private int state = 0;



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

        for(int i = 0; i < 1000; i++)
        {
            this.createdAlive[i] = 0;
            this.created[i] = null;
        }
        m("state: 0");
        Random.InitState(1234);

        // after these are done, some will collide and delete, which will happen in the next frame.
        // fill in the deleted ones 

    }

    public void setTrialSize(int trialSize)
    {
        this.trialSize = trialSize;
    }

    private int frameCounter = 0;
    public void Update()
    {
        if(this.createdAlive == null) { Debug.Log("CREATEDALIVE IS DEAD"); }
        if (state == 0) // we are still filling up the N nodes
        {
            if (countNotNull() < trialSize) // if statement, not while loop, cause while loop is the update cycle/game clock
            {
                Debug.Log("count not null: " + countNotNull());
                // could get rid couldNotNull() looping through twice, but I don't care enough
                for (int i = 0; i < trialSize; i++)
                {
                    if (this.createdAlive[i] == 0)
                    {
                        this.created[i] = createPin(i);
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
            m("seconds since entered state 2: " + this.timer);
            // countdown timer random seconds until transition to next stage (actual testing stage)
            if(this.timer > 5.0)
            {
                transitionTo(3);
            }
        }
        else if (state == 3)
        {
            // actual testing stage
            this.timer += Time.deltaTime;
            m("seconds since entered state 3: " + this.timer);
            // we also need a timer for this stage (for data recording)
        }
        else if (state == 4)
        {

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
            m("we are in state 1: press button to start your random 1-5 second countdown, then start your trial");

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
    }

    public void EnteringState3()
    {
        this.timer = 0.0f;
        this.state = 3;

        // TODO make stimuli visible, probably want to reset their timers too so that they blink in sync

        m("you are now testing! good luck"); // this will get overwritten by the timer immediately
    }


    public void EnteringState4()
    {
        this.state = 4;

        m("you are now done with this trial! your time was " + this.timer);

        deleteSquares();
    }

    public void EnteringState5()
    {
        controller.GetComponent<Control>().signalDone();
        m("wait for next trial");
        gameObject.SetActive(false);
        DestroyImmediate(gameObject);
    }




    private GameObject createPin(int count)
    {
        Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
        if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere

        if (onPlanet.y < sphereRadius * .75)
        {
            var newObject = VRCInstantiate(spawnItem);
            newObject.transform.position = this.transform.position + onPlanet;


            if (count == 0)
            {
                newObject.GetComponent<Cylinder1>().SetSpecial(count);
                // make this one the special one
            }
            else
            {
                newObject.GetComponent<Cylinder1>().SetUnSpecial(count);

            }
            this.createdAlive[count] = 1;
            return newObject;
        }
        else
        {
            // fail to create this node
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
    }

    public override void Interact()
    {
        if (state == 0)
        {

        }
        else if (state == 1)
        {
            transitionTo(2);
        }
        else if (state == 2)
        {
            // presssing button does nothing in waiting for time section.

        }
        else if (state == 3)
        {
            // if we press the button while actually performing the task, that means that we are done with the task
            transitionTo(4);

        }
        // transitioning to five is actually just deleting ourself, so that we may be born anew
        else if (state == 4)
        {
            transitionTo(5);

        }
    }

    public void deleteSquares()
    {
        Debug.Log("deleting started");
        Debug.Log("nuddddmber of nulls" + countNotNull());
        Debug.Log("deleting d");


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
    }

}
