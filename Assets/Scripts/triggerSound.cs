
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class triggerSound : UdonSharpBehaviour
{
    public GameObject audioObject;

    public override void Interact()
    {
        // audio = audioObject.GetComponent<AudioSource>();
        // audio.enabled = !audio.enabled;
        audioObject.SetActive(!audioObject.activeSelf);
        Debug.Log("hello");

    }
}
