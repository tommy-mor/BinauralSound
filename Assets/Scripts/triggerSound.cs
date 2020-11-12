
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class triggerSound : UdonSharpBehaviour
{
    public GameObject audioObject;
    public spawnCube mainbutton;

    public override void Interact()
    {
      //  var sound = gameObject.GetComponent<AudioSource>();
      //  Debug.Log("playting" + sound.isPlaying);

    //   sound.Play();
     //   Debug.Log("playting" + sound.isPlaying);


        mainbutton.DeleteSquares();
    }
}
