using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform tpPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>())
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
        }

        other.gameObject.transform.position = tpPoint.position;
        other.gameObject.transform.localRotation = tpPoint.localRotation;

        if (other.gameObject.GetComponent<CharacterController>())
        {
            other.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
