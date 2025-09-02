using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : MonoBehaviour
{
    public float lifetime; // how long before disappear
    public float curLifetime; // to reset if it's HIT BY ENEMY

    private SpriteRenderer spriteRenderer;
    public Color hitColor;

    public GameObject player;
    //FADE
    float startAlpha;
    public float decayTime;
    float elapsed;
    
    bool isDecaying;

    void Start()
    {
        hitColor.a = 1;
        curLifetime = lifetime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startAlpha = spriteRenderer.color.a;
    }


    void Update()
    {
        curLifetime -= Time.deltaTime;
        if (curLifetime < 0)
        {
            Destroy(gameObject);
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().counterEligible = true; //make player UNABLE to counter
            }
        }
            if (curLifetime < decayTime)
        {
            if (!isDecaying)
            {
                isDecaying = true;
                elapsed = 0f; // Start fade timer
                startAlpha = spriteRenderer.color.a;
            }
            elapsed += Time.deltaTime;
            float changedAlpha = Mathf.Lerp(startAlpha, 0, elapsed / decayTime);
            Color newColor = spriteRenderer.color;
            newColor.a = changedAlpha;
            spriteRenderer.color = newColor;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            spriteRenderer.color = hitColor; //indicate the loaded dash
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().counterEligible = true; //make player able to counter
                player.GetComponent<PlayerMovement>().counterTarget = other.transform.parent.gameObject; //set the target
            }
        }
    }
}
