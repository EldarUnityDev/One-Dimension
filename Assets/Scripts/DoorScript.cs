using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public List<Stages> myTwoRooms; //0 - inside room, 1 - ouside (saves every time)
    public bool playerOutside; //if this then load insideRoom
    public bool backgroundChanged;
    public GameObject backgroundLineIn;
    public GameObject backgroundLineOut;
    // go through Level.children - save in list
    // if 'door'.returnable, don't delete
    // if 'player', don't delete, don't save
    private void Start()
    {
        References.levelManager.GetComponent<LevelManager>().doors.Add(this);
    }
    public void ScanTheRoom()
    {
        // Loop through all children of this GameObject
        Stages currentRoom = new Stages();
        foreach (Transform currentChild in transform.parent)
        {
            GameObject child = currentChild.gameObject;
            if (child.activeInHierarchy && !child.CompareTag("Unchangeable") && !child.CompareTag("Player"))
            {
                currentRoom.enemies.Add(child);
                currentRoom.position.Add(child.transform.position.x);
                child.SetActive(false);
            }
        }
        if (playerOutside)
        {
            myTwoRooms[1] = currentRoom;
        }
        else myTwoRooms[0] = currentRoom;
    }
    public void LoadRoom()
    {
        Stages stageToSpawn = new Stages();
        if (playerOutside)
        {
            stageToSpawn = myTwoRooms[0];
            playerOutside = false;
        }
        else
        {

            stageToSpawn = myTwoRooms[1];
            playerOutside = true;
        }
        for (int i = 0; i < stageToSpawn.enemies.Count; i++)
        {
            float xCoord = stageToSpawn.position[i];
            Vector2 spawnPosition = new Vector2(xCoord, transform.position.y);
            if(stageToSpawn.enemies[i] != null)
            {
                stageToSpawn.enemies[i].SetActive(true);
            }
            else stageToSpawn.enemies.Remove(stageToSpawn.enemies[i]);
        }
    }

    public void UseDoor()
    {
        ScanTheRoom();
        LoadRoom();
    }



}
