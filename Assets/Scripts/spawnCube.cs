
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

    float sphereRadius = 10;

    private GameObject[] created;

    
    // states
    // 0 = beginner state (fill in the overlapped) (hidden)
    // 1 = in the random waiting time section
    // 2 = currently running (uhides, starts /updates timer)
    // 3 = done
    private int state = 0;



    // todo
    // do a L/R easy test to start
    //x make them change colors/change orientations
    // make the participant/runner separate
    // think of way to make them press button
    // x height limit of 75% radius
    // x move behavior of sound one into normal class
    // x handle collisions?
    // create object above us that can make more trials and etc, we are controlled by him.
    // refactor, add invisibility

    // x solution to sync problem: use synced variable random seed, so it generates all of the same things. send out network events. (good) will this work?
    // x OR hvae a set of nodes, just move them around using shared variables. (bad)
    //

    public void Start()
    {
        this.created = new GameObject[500];
    }

    private int countNotNull()
    {
        int count = 0;
        for(int i = 0; i < 500; i++)
        {
            if(this.created[i] == null)
            {
                count++;
            }
        }
        return count;
    }

    private void createPin(int count)
    {
        Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
        if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere

        if (onPlanet.y < sphereRadius * .75)
        {
            var newObject = VRCInstantiate(spawnItem);
            newObject.transform.position = this.transform.position + onPlanet;
            created[count] = newObject;


            if (count == 0)
            {
                newObject.GetComponent<Cylinder1>().SetSpecial(count);
                // make this one the special one
            }
            else
            {
                newObject.GetComponent<Cylinder1>().SetUnSpecial(count);

            }
        }
    }

    public void StartTrial() {
        Debug.Log("received event");
        Random.InitState(1234);
        //this.sentEvent = true;
        Debug.Log("testing");

        var t = text.GetComponent<Text>().text = "button pressed";
        Debug.Log(t);

        Debug.Log("number of nulls" + countNotNull());




        int count = 0;
        int goalCount = 100;
        while (count < goalCount)
        {
            createPin(count);
            count++;
            //newObject.
           
        }
        Debug.Log("number of nulls" + countNotNull());


        gameObject.SetActive(false);
    }
    
    public void deleteSquare(int idx)
    {
        Destroy(this.created[idx]);
        this.created[idx] = null;
    }
    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "StartTrial");
    }

    public void DeleteSquaresEvent()
    {
        Debug.Log("deleting started");
        Debug.Log("nuddddmber of nulls" + countNotNull());
        Debug.Log("deleting d");

        if (this.sentEvent)
        {
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
                for(int i = 0; i < 500; i++)
                {
                    if (created[i] == null)
                    { 
                        // do nothing
                    }
                    else
                    {
                        Debug.Log(created[i]);
               
                        Destroy(created[i]);
                        created[i] = null;
                    }
                }   
            }
        }
    }

    public void DeleteSquares()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "DeleteSquaresEvent");
    }

}
