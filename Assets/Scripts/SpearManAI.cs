using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpearManAI : MonoBehaviour
{
    public bool attacking; //to start attack coroutine
    public bool recovering; //to pause between actions
    public bool actionChosen; //to stop from both moving and hitting

    public float moveSpeed;
    public float telegraphTime;
    public float attackDuration;
    public float attackDistance;
    public float recoveryTime;
    public int coinCount;
    public GameObject attackObj; // Set in inspector or instantiate on attack
    public GameObject player;
    public GameObject coinPrefab;

    public AudioClip slainSound;
    public AudioClip coinDropSound;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    Rigidbody2D rb;
    /*   private void Awake()
       {
           References.levelManager.GetComponent<LevelManager>().enemies.Add(gameObject);
           //        References.levelManager.GetComponent<LevelManager>().enemies.Add(gameObject);

       }*/

    private void Start()
    {
        References.levelManager.GetComponent<LevelManager>().enemies.Add(gameObject); //2nd in order of references
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = References.player;
    }

    private void Update()
    {
        //if player close -> attacking = true
        if (player != null)
        {
            float distance = (transform.position - player.transform.position).magnitude;
            if (distance < attackDistance && !recovering && !attacking)
            {
                attacking = true;
            }
            else if (distance > attackDistance && !attacking && !recovering)
            {
                Approach();
            }
        }

        if (!actionChosen) //if IDLE
        {
            if (attacking)
            {
                StartCoroutine(Attack());
                actionChosen = true;

            }
        }
    }
    private IEnumerator Attack()
    {
        //Debug.Log("Telegraphing...");
        float elapsed = 0f;
        Color startColor = Color.red;
        Color endColor = Color.black;

        while (elapsed < telegraphTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / telegraphTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }        //Debug.Log("Attacking!");

                                                          //THE ATTACK ITSELF
        if (attackObj != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 currPosition = transform.position;

            float enemyHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
            float attackHalfWidth = attackObj.GetComponent<SpriteRenderer>().bounds.extents.x;

            currPosition = transform.position + direction * (enemyHalfWidth + attackHalfWidth);
            attackObj.transform.position = currPosition;
            attackObj.SetActive(true);
        }

        yield return new WaitForSeconds(attackDuration);

        if (attackObj != null)
            attackObj.SetActive(false);
        attacking = false;
        recovering = true;
        spriteRenderer.color = startColor;

        yield return new WaitForSeconds(recoveryTime);
        recovering = false;
        actionChosen = false;
    }
    public void Approach()
    {
        // Calculate direction to player
        //Vector2 direction = (player.transform.position - transform.position).normalized;

        // Vector2 newPosition = rb.position + direction * moveSpeed * Time.deltaTime;
        //rb.MovePosition(newPosition);

        //2nd way
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    public void Die()
    {
        SpawnCoins();
        Destroy(gameObject);
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
    private void OnDestroy()
    {
        if (References.levelManager != null)
        {
            References.levelManager.GetComponent<LevelManager>().enemies.Remove(gameObject);

        }
    }
}
