using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
    public GameObject ammoPrefab;
    GameObject currentInstance;
    bool hasSpawned = false;
    bool timerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn initial ammo on level start
        SpawnAmmo();
    }

    // Update is called once per frame
    void Update()
    {

        //If no current ammo, start timer to make more...
        if (!currentInstance && !timerStarted)
        {
            timerStarted = true;
            StartCoroutine(SpawnTimer());
        }
    }

    private void SpawnAmmo()
    {
        currentInstance = Instantiate(ammoPrefab, transform.position, transform.rotation);
        timerStarted = false; //Allow timer to start again
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(60f);
        SpawnAmmo();
    }
}
