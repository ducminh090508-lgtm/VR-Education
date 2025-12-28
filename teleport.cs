using UnityEngine;
using System.Collections;

public class TeleportPad : MonoBehaviour
{
    public Transform player , destination;
    public GameObject playerg; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerg.SetActive(false);
            player.position = destination.position; 
            playerg.SetActive(true); 
        }
    }
}