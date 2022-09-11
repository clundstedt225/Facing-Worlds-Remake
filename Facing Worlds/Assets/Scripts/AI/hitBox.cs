using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBox : MonoBehaviour
{
    EnemyBot eb;
    public int damageAmount = 50;

    // Start is called before the first frame update
    void Start()
    {
        eb = GetComponentInParent<EnemyBot>();
    }

    //Called by raycast bullet from player
    public void DealDamage()
    {
        eb.TakeDamage(damageAmount);
    }
}
