
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Cube : UdonSharpBehaviour
{
    // adapted from https://github.com/Martichoras/Pip-Pop-Experiment/blob/main/Assets/Scripts/PinController.cs

    private int[] angle = new int[12] { 15, 20, 30, 60, 70, 75, -15, -20, -30, -60, -70, -75 }; // predetermined array of possible angles 
    private int random;
    private int localAngle;
    //private Vector3 rot;

    //private GameObject sphere;

    // Use this for initialization
    void Start()
    {

        random = Random.Range(0, 12); // picks a random angle for this specific clone
        localAngle = angle[random];  // picks a random angle for this specific clone

        transform.LookAt(Vector3.zero); // turn pin towards origo

        transform.Rotate(0, 0, localAngle); // rotates pin to the selected z-angle

    }

    void OnCollisionEnter(Collision col)
    {

        // This function is primarily to avoid overlaying pins by destroying this pin if it collides with another pin.
        // PinSpawner.cs makes sure pins keep spawning like this untill there's a total of e.g. 400 pins in the scene.

        //if (col.gameObject.tag == "Pin")
        //{ //if colliding object is of type 'Pin'
        Destroy(gameObject); //destroy this game object.
                             //Debug.Log("Object destroyed!");
                             // }
                             //  else if (col.gameObject.tag == "PipPopPin")
                             //  { // makes sure PipPopPin destroys this object. could be an || statement, but meh. I like spaghetti.
                             //      Destroy(this.gameObject);
                             //     Debug.Log("Pip-Pop Pin destroyed other pin!");
                             //  }
    }
}