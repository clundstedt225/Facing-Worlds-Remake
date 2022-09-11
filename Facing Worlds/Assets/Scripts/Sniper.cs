using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sniper : MonoBehaviour
{
    //Camera refs
    public Camera MainCam, RigCam;
    float defaultZoom = 0f;
    bool isZoomed = false;
    public LayerMask IgnoreMe;

    public ParticleSystem shellEffect;
    public GameObject muzzleFlashObject;
    public GameObject ImpactParticle, bloodImpact;
    public GameObject bigCross;
    public TextMeshProUGUI smallCross;
    public Transform rayOrigin;
    public Transform barrelTip;
    AudioSource aSource;
    Animator anim;

    Player playerInfo;

    bool canShoot = true;
    float coolDown = 0.5f;

    //In case player was mid shot while killed
    private void OnEnable()
    {
        canShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Save default FOV
        defaultZoom = MainCam.fieldOfView;

        playerInfo = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canShoot) return;

        if (Input.GetButtonDown("Fire1") && playerInfo.ammo > 0)
        {
            RifleFire();
        }


        if (isZoomed)
        {
            //Unzoom back to normal view
            if(Input.GetButtonDown("Fire2"))
            {
                //reset
                UnZoom();
            }
        } else
        {
            //Not zoomed in yet. If still hasn't let go then keep going...
            if (Input.GetButton("Fire2"))
            {
                RigCam.enabled = false;
                smallCross.enabled = false;
                bigCross.SetActive(true);

                MainCam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().renderPostProcessing = true;

                if (MainCam.fieldOfView > 12)
                {
                    MainCam.fieldOfView -= (Time.deltaTime * 45f);
                }
            }
        }

        //Let go of zoom button
        if (Input.GetButtonUp("Fire2"))
        {
            //Flag as now zoomed or unzoomed
            isZoomed = !isZoomed;
        }
    }

    //Resets players view to normal
    public void UnZoom()
    {
        //reset
        RigCam.enabled = true;
        smallCross.enabled = true;
        bigCross.SetActive(false);
        MainCam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().renderPostProcessing = false;
        MainCam.fieldOfView = defaultZoom;
    }

    void RifleFire()
    {
        StartCoroutine(Shoot());
    }

    //Coroutine for shooting which allows for certain effects
    IEnumerator Shoot()
    {
        canShoot = false;

        playerInfo.UseAmmo();

        //Play Sound and Animation
        aSource.Play();
        anim.SetTrigger("shoot");
        shellEffect.Play();

        //Enable flash object
        muzzleFlashObject.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlashObject.SetActive(false);

        //Shoot raycast
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, 5000, ~IgnoreMe))
        {
            if (hit.collider.gameObject.GetComponent<hitBox>())
            {
                hit.collider.gameObject.GetComponent<hitBox>().DealDamage();

                //Hit enemy, create blood impact
                GameObject impact = Instantiate(bloodImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(impact, 3f); //Clean up in world after 3 seconds
            } else
            {
                //Create standard impact for bullets
                GameObject impact = Instantiate(ImpactParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(impact, 3f); //Clean up in world after 3 seconds
            }
        }

        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }

}
