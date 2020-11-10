
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class spawnCube : UdonSharpBehaviour
{
    public GameObject spawnItem;
    public GameObject audioObject;

    float sphereRadius = 10;


    public override void Interact()
    {



        for (int i = 0; i < 250; i++)
        {
            var newObject = VRCInstantiate(spawnItem);
            Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
            newObject.transform.position = onPlanet;
        }
        var newAudioObject = VRCInstantiate(audioObject);
        newAudioObject.transform.position = new Vector3((Random.Range(-10.0f, 10.0f)), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

    }
}
