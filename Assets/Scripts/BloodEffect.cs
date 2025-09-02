using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color baseColor = Color.white;

    public float effectTime;
    public float timer = 0f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = baseColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, timer / effectTime);

        if (spriteRenderer != null)
        {
            Color newColor = baseColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }

        if (timer >= effectTime)
        {
            Destroy(gameObject);
        }

    }

}
