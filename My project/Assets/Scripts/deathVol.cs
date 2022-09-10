using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathVol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().killPlayer();
        }
    }
}
