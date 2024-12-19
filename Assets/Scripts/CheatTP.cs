using NUnit.Framework;
using UnityEngine;

public class CheatTP : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Transform[] listCheckpoint = new Transform[9];


    void Start()
    {
        
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) player.position = listCheckpoint[0].position;
        if (Input.GetKeyDown(KeyCode.Keypad1)) player.position = listCheckpoint[1].position;
        if (Input.GetKeyDown(KeyCode.Keypad2)) player.position = listCheckpoint[2].position;
        if (Input.GetKeyDown(KeyCode.Keypad3)) player.position = listCheckpoint[3].position;
        if (Input.GetKeyDown(KeyCode.Keypad4)) player.position = listCheckpoint[4].position;
        if (Input.GetKeyDown(KeyCode.Keypad5)) player.position = listCheckpoint[5].position;
        if (Input.GetKeyDown(KeyCode.Keypad6)) player.position = listCheckpoint[6].position;
        if (Input.GetKeyDown(KeyCode.Keypad7)) player.position = listCheckpoint[7].position;
        if (Input.GetKeyDown(KeyCode.Keypad8)) player.position = listCheckpoint[8].position;


    }
}
