using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBlock : MonoBehaviour
{
    public GameObject squarePrefab;
    public Color baseColor;
    public Color myBrown;
    [Range(0f, 1f)] public float minBrightness = 0.1f;
    [Range(0f, 1f)] public float maxBrightness = 0.5f;
    SpriteRenderer sr;
    public bool fillNeeded;
    void Start()
    {
        if (squarePrefab == null)
        {
            Debug.LogError("Square prefab not assigned.");
            return;
        }

        // Get screen dimensions
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Get sprite size and apply scale
        sr = squarePrefab.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Square prefab needs a SpriteRenderer.");
            return;
        }

        Vector2 spriteSize = sr.sprite.bounds.size;
        Vector3 prefabScale = squarePrefab.transform.localScale;
        float squareWidth = spriteSize.x * prefabScale.x;

        int numberOfSquares = Mathf.FloorToInt(screenWidth / squareWidth);

        float totalWidth = numberOfSquares * squareWidth;
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2f - squareWidth / 2f, 0, 0);
        if (fillNeeded)
        {
            for (int i = 0; i < numberOfSquares; i++)
            {
                Vector3 position = startPosition + new Vector3(i * squareWidth, 0, 0);
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity, transform);

                SpriteRenderer squareSR = square.GetComponent<SpriteRenderer>();
                if (squareSR != null)
                {
                    float brightness = Random.Range(minBrightness, maxBrightness);
                    Color randomShade = baseColor * brightness;
                    randomShade.a = 1f;
                    squareSR.color = randomShade;
                }
            }
            SpawnRandomBrownSquares(5, squareWidth, screenWidth, screenHeight);
        }
    }
    void SpawnRandomBrownSquares(int treeCount, float squareWidth, float screenWidth, float screenHeight)
    {
        for (int i = 0; i < treeCount; i++)
        {
            // Get random position within the screen bounds
            float x = Random.Range(-screenWidth / 2f, screenWidth / 2f);

            Vector3 position = new Vector3(x, 0, 0f) + transform.position;

            GameObject brownSquare = Instantiate(squarePrefab, position, Quaternion.identity, transform);

            SpriteRenderer sr = brownSquare.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Set to a brown color (you can tweak this)
                sr.color = myBrown;
            }
        }
    }
}
