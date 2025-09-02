using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsñript : MonoBehaviour
{
    public bool startedMoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private IEnumerator RemoveMeFromScreen(float x, float duration)
    {
        Vector3 startPos = References.player.transform.position;
        Vector3 endPos = new Vector3(x, startPos.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // transitionToggled = false; //for later enable 
    }
    // Update is called once per frame
    void Update()
    {
        if(!startedMoving && References.player.GetComponent<PlayerMovement>().ableToMove)
        {
            startedMoving = true;
            StartCoroutine(RemoveMeFromScreen(-10, 2));
        }
    }
}
