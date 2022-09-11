using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool killed = false;
    public int ammo = 12;
    public int health = 100;
    public GameObject mainCam, cam2;
    public GameObject deathView;
    public GameObject bloodVis;
    public AudioClip deathSound, respawnSound, ammoSound;
    GameObject view;

    private void Start()
    {
        //Set ui to initial values
        GameManager.Instance.SetAmmoUI(ammo);
    }

    private void Update()
    {
        //Check for death state
        if (health <= 0 && !killed)
        {
            //debounce
            killed = true;

            //What happens after player death
            killPlayer();
        } else if (killed)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RespawnPlayer();
            }
        }
    }

    public void killPlayer()
    {
        //Set again in case of external method call
        killed = true;
        GameManager.Instance.AddRedKill();

        GetComponent<AudioSource>().clip = deathSound;
        GetComponent<AudioSource>().Play();

        //Disable movement + char controller collider
        gameObject.GetComponent<Movement>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false; //Makes player invis to bot

        //Return mouse controll/disable look rotation
        mainCam.GetComponent<MouseCam>().enabled = false;

        //Disable weapon cam and weapon
        cam2.SetActive(false);

        //Disable mainUI group and enable death menu group
        GameManager.Instance.mainUI.SetActive(false);
        GameManager.Instance.deathUI.SetActive(true);

        view = Instantiate(deathView, mainCam.transform.position, mainCam.transform.rotation);
        view.GetComponent<Rigidbody>().AddForce(view.transform.forward * 3, ForceMode.Impulse);
    }

    private void RespawnPlayer()
    {
        //Destroy death cam instance
        if (view != null) Destroy(view);

        //(fetch point from manager and set position to point)
        int count = GameManager.Instance.blueSpawnPoints.Count;
        transform.position = GameManager.Instance.blueSpawnPoints[UnityEngine.Random.Range(0, count)].position;

        //Re-enable movement + char controller collider
        gameObject.GetComponent<Movement>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = true;

        //Return mouse controll/disable look rotation
        mainCam.GetComponent<MouseCam>().enabled = true;

        //Enable mainUI group
        GameManager.Instance.mainUI.SetActive(true);
        GameManager.Instance.deathUI.SetActive(false);

        //Enable weapon cam and weapon
        cam2.SetActive(true);

        GetComponentInChildren<Sniper>().UnZoom();

        GetComponent<AudioSource>().clip = respawnSound;
        GetComponent<AudioSource>().Play();

        //Restore default variable values
        health = 100;
        ammo = 12;
        killed = false;

        GameManager.Instance.SetAmmoUI(ammo);
        GameManager.Instance.SetHealthUI(health);
    }

    public void UseAmmo()
    {
        //Update UI and use bullet
        if (ammo > 0)
        {
            ammo -= 1;
            GameManager.Instance.SetAmmoUI(ammo);
        }  
    }

    //When something hurts the player
    public void ApplyDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
        }

        bloodVis.GetComponent<Animator>().SetTrigger("hit");
        GameManager.Instance.SetHealthUI(health);
    }

    //Add ammo to players inventory
    public void AwardAmmo(int amount)
    {
        //Add to ammo and play sound
        ammo += amount;

        GetComponent<AudioSource>().clip = ammoSound;
        GetComponent<AudioSource>().Play();

        //Update UI
        GameManager.Instance.SetAmmoUI(ammo);
    }
}
