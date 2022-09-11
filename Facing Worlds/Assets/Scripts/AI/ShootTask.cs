using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class ShootTask : Action {

    //Previously found player to shoot/aim at
    public SharedTransform target;

    public AudioClip shootSound;
    public Transform rayOrigin;
    public GameObject mFlash;
    public AudioSource aSource;
    public GameObject ImpactParticle;
    bool isShooting = false;

    public override TaskStatus OnUpdate()
    {
        GetComponent<NavMeshAgent>().enabled = false;

        // Determine which direction to rotate towards
        Vector3 targetDirection = target.Value.position - transform.position;
        Vector3 targetDirection2 = target.Value.position - rayOrigin.position;

        // Rotate the forward vector towards the target direction
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, (5.0f * Time.deltaTime), 0.0f);
        Vector3 newDirection2 = Vector3.RotateTowards(rayOrigin.forward, targetDirection2, (5.0f * Time.deltaTime), 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        rayOrigin.rotation = Quaternion.LookRotation(newDirection2);
        Quaternion myLook = Quaternion.LookRotation(newDirection);
        transform.rotation = Quaternion.Euler(0, myLook.eulerAngles.y, 0);

        if (!isShooting)
        {
            StartCoroutine(shootGun());
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        //Lost player, give controll back to navmesh
        GetComponent<NavMeshAgent>().enabled = true;
        base.OnEnd();
    }

    IEnumerator shootGun()
    {
        isShooting = true;

        //Play audio & visuals to indicate bot is shooting
        aSource.clip = shootSound;
        aSource.Play();

        mFlash.SetActive(true);
        yield return new WaitForSeconds(.01f);
        mFlash.SetActive(false);

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, 5000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.forward * 5000, Color.yellow);

            if (hit.collider.gameObject.GetComponent<Player>())
            {
                hit.collider.gameObject.GetComponent<Player>().ApplyDamage(50);
            }

            //Create impact for bullets
            GameObject impact = MonoBehaviour.Instantiate(ImpactParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            MonoBehaviour.Destroy(impact, 3f); //Clean up in world after 3 seconds
        } else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.forward * 5000, Color.red);
        }

        yield return new WaitForSeconds(Random.Range(0.5f, 1.8f));

        isShooting = false;
    }
}
