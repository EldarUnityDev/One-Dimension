using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    public GameObject deathEffect;
    public bool playerAttack;
    public bool oneHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerAttack && other.CompareTag("Player"))
        {
            //plain destroy + PLAY A SOUND
            other.GetComponent<PlayerMovement>().Die();
            Instantiate(deathEffect);
        }
        if (playerAttack && other.CompareTag("Enemy"))
        {
            //plain destroy + PLAY A SOUND
            if(other.GetComponent<EnemyBehavior>() != null)
            {
                other.GetComponent <EnemyBehavior>().SpawnEffectIni();
                other.GetComponent<EnemyBehavior>().Die();

            }
            else
            {
                other.GetComponent<SpearManAI>().Die();
            }
            Instantiate(deathEffect);
            Instantiate(deathEffect);
            Instantiate(deathEffect);
            if (oneHit)
            {
                Destroy(gameObject);
            }
        }

        //PROBLEM!!! THE ENEMY DOESNT DIE IMMEDIATELY, SO THE EFFECT MULTIPLIES
        //IT"S AWESOME BUT SHOULD BE MORE CONTROLLED
        //Instantiate(deathEffect); //whoever dies - the sound plays
    }
}
