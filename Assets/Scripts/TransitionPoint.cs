using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    private float shimmerSpeed = 2f;
    private float shimmerTimer; 
    private SpriteRenderer spriteRenderer;
    public Color baseColor;
    public Color shineColor;
    public GameObject levelManager;
    public GameObject shop;
    public GameObject transitionText;

    // Start is called before the first frame update
    void Start()
    {
        transitionText.SetActive(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        shimmerTimer += Time.deltaTime * shimmerSpeed;
        float t = (Mathf.Sin(shimmerTimer) + 1f) / 2f;
        spriteRenderer.color = Color.Lerp(baseColor, shineColor, t);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            References.levelManager.GetComponent<LevelManager>().currentStageNumber++;
            levelManager.GetComponent<LevelManager>().SaveResources();

            shop.SetActive(true); //OPEN SHOP

            PlayerMovement playerInfo = References.player.GetComponent<PlayerMovement>();
            playerInfo.ableToMove = false;
            gameObject.SetActive(false); //reusable

            //Time.timeScale = 0; // STOP TIME
            // References.levelManager.GetComponent<LevelManager>().inMainMenu = true;
            //coroutine to move the player to START position on the left
            //take away player control
            //move + enable SHOP
            //spawn enemies
            //dialogue count ++
            //current level ++
            //give back player control
            //check list <enemies to spawn>, list<x.coordinates for each enemy>
        }
    }
    private void OnDisable()
    {
        if(transitionText != null)
        transitionText.SetActive(false);
    }
}
