
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Cylinder1 : UdonSharpBehaviour
{
    // adapted from https://github.com/Martichoras/Pip-Pop-Experiment/blob/main/Assets/Scripts/PinController.cs

    private int[] angle = new int[12] { 15, 20, 30, 60, 70, 75, -15, -20, -30, -60, -70, -75 }; // predetermined array of possible angles
    private int[] chosenAngles = new int[2] { 90, 0 }; // predetermined array of possible angles 

    private int random;
    private int state; // state 0 = red, state 1 = green;
    private float timer;
    private AudioSource sound;
    private int localangle;

    public float changeEvery;
    public Material redMat;
    public Material greenMat;
    public Material yellow;




    public GameObject homeCube;
    //private Vector3 rot;

    //private GameObject sphere;

    // Use this for initialization
    void Start()
    {
        this.timer = 0.0f;
        this.state = Random.Range(0, 2); 

    }

    private void Update()
    {
        this.timer += Time.deltaTime;

        // commented code here makes the special one blue
        // if (this.sound != null)
        // {
        //     if(this.timer > this.changeEvery)
        //     {
        //         gameObject.GetComponent<Renderer>().material = yellow;
        //
        //        this.timer = 0.0f;
        //        this.sound.Play();
        //   }
        //    return;
        //
        // }
        if (this.state == 0) // we are red right now
        {
            if (this.timer > this.changeEvery)
            {
                this.state = 1; // switch to green
                this.timer = 0.0f;
                gameObject.GetComponent<Renderer>().material = greenMat;
                if(this.sound != null)
                {
                    this.sound.Play();
                }
            }
        } else if (this.state == 1) // we are green right now
        {
            if (this.timer > this.changeEvery)
            {
                this.state = 0; // switch to red
                this.timer = 0.0f;
                gameObject.GetComponent<Renderer>().material = redMat;

                if(this.sound != null) 
                {
                    this.sound.Play();
                }

            }
        }

    }

    public void SetSpecial()
    {
        Debug.Log("i am special");
        // we are the special one.

        // chooses between 90 and 0 deg
        var random2 = Random.Range(0, 2);
        var specialAngle = chosenAngles[random2];

        transform.LookAt(homeCube.transform); // turn pin towards origo

        transform.Rotate(0, 0, specialAngle);

        // enable sound object so it can play on state change, this.sound == null is checked                                     
        this.sound = gameObject.GetComponent<AudioSource>();
    }

    public void SetUnSpecial()
    {
        random = Random.Range(0, 12); // picks a random angle for this specific clone
        this.localangle = angle[random];  // picks a random angle for this specific clone

        transform.LookAt(homeCube.transform); // turn pin towards origo

        transform.Rotate(0, 0, this.localangle); // rotates pin to the selected z-angle

    }

    public void OnCollisionEnter(Collision col)
    {

        // This function is primarily to avoid overlaying pins by destroying this pin if it collides with another pin.
        // PinSpawner.cs makes sure pins keep spawning like this untill there's a total of e.g. 400 pins in the scene.

        //    //if (col.gameObject.tag == "Pin")
        //    //{ //if colliding object is of type 'Pin'
        //    Debug.Log("found collision");
        //   //Destroy(gameObject); //destroy this game object.
        //                         //Debug.Log("Object destroyed!");
        //                         // }
        //                         //  else if (col.gameObject.tag == "PipPopPin")
        //                         //  { // makes sure PipPopPin destroys this object. could be an || statement, but meh. I like spaghetti.
        //                         //      Destroy(this.gameObject);
        //                         //     Debug.Log("Pip-Pop Pin destroyed other pin!");
        //                         //  }
        Debug.Log("collided");
    }

}
