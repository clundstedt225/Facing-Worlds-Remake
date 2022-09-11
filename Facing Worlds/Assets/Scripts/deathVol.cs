using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script kills the player on entering the volume

public class deathVol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Check for player only to enter trigger volume
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().killPlayer();
        }
    }
}
