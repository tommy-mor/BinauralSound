
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

    private GameObject[] created = new GameObject[500];
    private bool sentEvent = false;

    // todo
    // do a L/R easy test to start
    //x make them change colors/change orientations
    // make the participant/runner separate
    // think of way to make them press button
    // x height limit of 75% radius
    // -- move behavior of sound one into normal class
    //  handle collisions?

    // solution to sync problem: use synced variable random seed, so it generates all of the same things. send out network events. (good) will this work?
    // OR hvae a set of nodes, just move them around using shared variables. (bad)
    //

    public void StartTrial() {
        Debug.Log("received event");
        Random.InitState(1234);
        this.sentEvent = true;
        Debug.Log("testing");

        var t = text.GetComponent<Text>().text = "button pressed";
        Debug.Log(t);




        int count = 0;
        int goalCount = 100;
        while (count < goalCount)
        {


            //newObject.
            Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
            if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere

            if (onPlanet.y < sphereRadius * .75)
            {
                var newObject = VRCInstantiate(spawnItem);
                newObject.transform.position = this.transform.position + onPlanet;
                created[count] = newObject;





                if (count == 0)
                {
                    newObject.GetComponent<Cylinder1>().SetSpecial();
                    // make this one the special one
                } else
                {
                    newObject.GetComponent<Cylinder1>().SetUnSpecial();

                }
                count++;
            }
        }

        gameObject.SetActive(false);
    }
    

    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "StartTrial");
    }

    public void DeleteSquares()
    {
        if (this.sentEvent)
        {
            for (int i = 0; i < 251; i++)
            {
                Debug.Log("deleting");
                if(created[i] != null)
                {
                    Destroy(created[i]);

                }


            }
        }
    }

}
