using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private float shimmerSpeed = 2f;
    private float shimmerTimer;

    private SpriteRenderer spriteRenderer;
    public Color baseColor;
    public Color shineColor;

    public GameObject mySoundEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Give it a random burst direction
        float forceX = Random.Range(-5f, 5f);
        rb.AddForce(new Vector2(forceX, 0), ForceMode2D.Impulse);
        StartCoroutine(StopMoving());
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && rb.velocity == Vector2.zero)
        {
            other.GetComponent<PlayerMovement>().myCoinsNumber++;
            Instantiate(mySoundEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        shimmerTimer += Time.deltaTime * shimmerSpeed;
        float t = (Mathf.Sin(shimmerTimer) + 1f) / 2f;
        spriteRenderer.color = Color.Lerp(baseColor, shineColor, t);
    }
    IEnumerator StopMoving()
    {
        float delay = 0.15f;
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return null;
    }
}
