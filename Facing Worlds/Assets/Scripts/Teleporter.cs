using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    //Where to teleport to
    public Transform tpPoint;

    //Instantly update players position when they enter the volume
    private void OnTriggerEnter(Collider other)
    {
        //Disable character controller component to allow for position change
        if (other.gameObject.GetComponent<CharacterController>())
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
        }

        //Update position and rotation
        other.gameObject.transform.position = tpPoint.position;
        other.gameObject.transform.localRotation = tpPoint.localRotation;

        if (other.gameObject.GetComponent<CharacterController>())
        {
            other.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
