
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


public class spawnCube : UdonSharpBehaviour
{
    public GameObject spawnItem;
    public GameObject audioObject;
    public GameObject text;

    float sphereRadius = 10;

    private GameObject[] created = new GameObject[251];
    // current goal: make obilque cylinders rotated at the same angles
    private bool sentEvent = false;
    // TODO make next stage a new level (look up tutorial on how to make levels in vrchat, otherwise make it area that opens up/gets enabled

    // do a L/R easy test to start

    private bool hasNull(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {

        }
        return false;
    }

    public override void Interact()
    {
        this.sentEvent = true;
        Debug.Log("testing");

        var t = text.GetComponent<Text>().text = "button pressed";
        Debug.Log(t);




        for (int i = 0; i < 100; i++)
        {

            if (created[i] == null)
            {
                var newObject = VRCInstantiate(spawnItem);
                //newObject.
                Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
                if (onPlanet.y < 0) onPlanet.y = onPlanet.y * -1; // flip lower hemisphere
               

                newObject.transform.position = this.transform.position + onPlanet;
                created[i] = newObject;
            }

        };



        var newAudioObject = VRCInstantiate(audioObject);
        created[250] = newAudioObject;

        newAudioObject.transform.position = this.transform.position + Random.onUnitSphere * sphereRadius;
        var sound = newAudioObject.GetComponent<AudioSource>();
        sound.loop = true;
        sound.Play();

        gameObject.SetActive(false);
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
