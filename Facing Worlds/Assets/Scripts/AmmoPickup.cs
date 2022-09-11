using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    //How much ammo to award player
    public int amount = 6;

    private void OnTriggerEnter(Collider other)
    {
        //Check for player only
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().AwardAmmo(amount);
            Destroy(gameObject);
        }
    }
}
