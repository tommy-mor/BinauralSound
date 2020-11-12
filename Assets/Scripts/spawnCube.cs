
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
            Debug.Log("1");


            if (created[i] == null)
            {
            var newObject = VRCInstantiate(spawnItem);
            //newObject.
            Debug.Log("2");

            Vector3 onPlanet = Random.onUnitSphere * sphereRadius;
            Debug.Log("3");

            newObject.transform.position = this.transform.position + onPlanet;
            Debug.Log("4");
            created[i] = newObject;
            Debug.Log("5");

            }

        };
        Debug.Log("6");




        Debug.Log("6");
        var newAudioObject = VRCInstantiate(audioObject);
        Debug.Log("7");
        created[250] = newAudioObject;
        Debug.Log("8");

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
                //Destroy(created[i]);

            }
        }
    }

}
