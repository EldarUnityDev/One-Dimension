using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    public float speed = 10f;
    private int direction = 1;
    private void Start()
    {
        Destroy(gameObject,1);
    }

    public void SetDirection(bool facingRight)
    {
        direction = facingRight ? 1 : -1;

        // Flip sprite if needed
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction;
        transform.localScale = localScale;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}
