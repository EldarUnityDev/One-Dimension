using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShimmer : MonoBehaviour
{
    public Color baseColor;
    public Color shineColor;
    private float shimmerSpeed = 2f;
    private float shimmerTimer;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        shineColor = Color.Lerp(baseColor, Color.white, 0.3f);
        shimmerTimer = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        shimmerTimer += Time.deltaTime * shimmerSpeed;
        float t = (Mathf.Sin(shimmerTimer) + 1f) / 2f;
        sr.color = Color.Lerp(baseColor, shineColor, t);
    }
}
