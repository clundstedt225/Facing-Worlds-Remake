using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class EnemyBot : MonoBehaviour
{
    public int health = 100;
    NavMeshAgent agent;
    Animator anim;
    bool hasDied = false;
    public AudioClip deathSound, respawnSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (health <= 0 && !hasDied)
        {
            //dead
            BotKilled();
        } else
        {
            anim.SetFloat("movement", agent.velocity.magnitude);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    public void BotKilled()
    {
        GameManager.Instance.AddBlueKill();

        GetComponent<AudioSource>().clip = deathSound;
        GetComponent<AudioSource>().Play();

        hasDied = true;
        anim.SetBool("isDead", true);
        GetComponent<BehaviorTree>().DisableBehavior();

        //Start respawn timer 
        StartCoroutine(RespawnTimer());
    }

    public void Respawn()
    {
        GetComponent<AudioSource>().clip = respawnSound;
        GetComponent<AudioSource>().Play();

        health = 100;
        hasDied = false;
        anim.SetBool("isDead", false);
        GetComponent<BehaviorTree>().EnableBehavior();

        //(fetch point from manager and set position to point)
        int count = GameManager.Instance.redSpawnPoints.Count;
        transform.position = GameManager.Instance.redSpawnPoints[Random.Range(0, count)].position;
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(5f);
        Respawn();
    }
}
