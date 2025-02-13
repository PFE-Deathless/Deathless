using NUnit.Framework;
using UnityEngine;

public class CheatTP : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Transform[] listCheckpoint = new Transform[9];
    private Vector3 offset;


    void Start()
    {
        offset = cam.GetComponent<CameraBehavior>().offset;
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            player.position = listCheckpoint[0].position;
            cam.transform.position = listCheckpoint[0].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            player.position = listCheckpoint[1].position;
            cam.transform.position = listCheckpoint[1].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            player.position = listCheckpoint[2].position;
            cam.transform.position = listCheckpoint[2].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            player.position = listCheckpoint[3].position;
            cam.transform.position = listCheckpoint[3].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            player.position = listCheckpoint[4].position;
            cam.transform.position = listCheckpoint[4].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            player.position = listCheckpoint[5].position;
            cam.transform.position = listCheckpoint[5].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            player.position = listCheckpoint[6].position;
            cam.transform.position = listCheckpoint[6].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            player.position = listCheckpoint[7].position;
            cam.transform.position = listCheckpoint[7].position + offset;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            player.position = listCheckpoint[8].position;
            cam.transform.position = listCheckpoint[8].position + offset;
        }


    }
}
