using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool ableToMove;
    public GameObject attackObj;
    public GameObject shieldObj;
    public GameObject shieldBreakContainer;
    public GameObject dashObj;
    public GameObject throwingKnife;

    public float moveSpeed;
    public float dashLength;
    private Rigidbody2D rb;
    private float moveInput;
    public bool facingRight;
    public bool shielded;
    public float blockOffset;
    public bool attackDisableTimer;
    public float attackLongevity;

    public int myCoinsNumber;
    public int shieldsCount;
    public int knivesCount;

    AudioSource audioSource;

    public bool dashEligible; // for DASH cooldown

    public bool counterEligible;
    public GameObject currentDash;
    public GameObject counterTarget;
    public GameObject errorObj;
    public GameObject doorPrompt;


    public GameObject coinUI;
    public GameObject shieldUI;
    public GameObject knivesUI;


    public bool autoShielded;

    float playerHalfWidth;
    private void Awake()
    {
        References.player = gameObject;
    }
    void Start()
    {
        //ableToMove = true;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        rb.drag = 5f;
        dashLength = dashObj.transform.localScale.x;

        playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void Update()
    {
        if (ableToMove)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput > 0)
                facingRight = true;
            else if (moveInput < 0)
                facingRight = false;

            // ATTACK
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
            // DASH
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash();
            }
            // COUNTER ATTACK
            //if (Input.GetKeyDown(KeyCode.E))
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (counterEligible)
                {
                    CounterAttack();
                }
            }
            // SHIELD
            if (Input.GetKey(KeyCode.Q))
            {
                if (shieldsCount > 0) // or KeyCode.Space, etc.
                {
                    Vector2 currPosition = transform.position;
                    float shieldHalfWidth = shieldObj.GetComponent<SpriteRenderer>().bounds.extents.x;
                    if (facingRight)
                    {
                        currPosition += Vector2.right * (playerHalfWidth + shieldHalfWidth);
                    }
                    else
                    {
                        currPosition += Vector2.left * (playerHalfWidth + shieldHalfWidth);
                    }
                    shieldObj.transform.position = currPosition;
                    shieldObj.SetActive(true);
                    shielded = true;
                }
                else { ShowErrorMessage("no shields left"); }
            }
            else { shieldObj.SetActive(false); }
            //Throwing knife
            if (Input.GetKeyDown(KeyCode.F)) // Or whatever button you want
            {
                ThrowKnife();
            }
        }
        RefreshResources();

        DoorScript nearestDoorSoFar = null;
        float nearestDistance = 3; //max pickup distance
        foreach (DoorScript thisDoor in References.levelManager.GetComponent<LevelManager>().doors)
        {
            //how far is this one from the player?
            float thisDistance = Vector3.Distance(transform.position, thisDoor.transform.position);
            //is it closer than anything else we've found?
            if (thisDistance <= nearestDistance)
            {
                //if it's THIS now it's the closest one
                nearestDoorSoFar = thisDoor;
                nearestDistance = thisDistance;
            }
            //+++ challenge - check if it's in front of us
        }
        if (nearestDoorSoFar != null)
        {
            //show USE prompt
            //References.canvas.usePromptSignal = true;
            if(!doorPrompt.activeInHierarchy) doorPrompt.SetActive(true);
            if (Input.GetButtonDown("Use"))
            {
                nearestDoorSoFar.UseDoor();
            }
        }else if(nearestDoorSoFar == null && doorPrompt.activeInHierarchy) doorPrompt.SetActive(false);
    }
    public void ShowErrorMessage(string message)
    {
        errorObj.SetActive(true);
        errorObj.GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(DisableAfterTime(errorObj, 1.5f));
    }
    public void CounterAttack()
    {
        if (counterTarget != null && currentDash != null)
        {
            if (transform.position.x < counterTarget.transform.position.x)
            {
                // Player is to the left of the enemy
                facingRight = true;
                transform.position = new Vector2(counterTarget.transform.position.x - blockOffset * 2, transform.position.y);
            }
            else
            {
                // Player is to the right of the enemy
                facingRight = false;
                transform.position = new Vector2(counterTarget.transform.position.x + blockOffset * 2, transform.position.y);
            }

            Attack(); //Add an auto-hit
                      //disable DASH
            Destroy(currentDash);

            counterTarget = null; // you've got 1 jump
            currentDash = null;

        }
        else { counterEligible = false; }
    }
    public void Attack()
    {
        audioSource.Play();

        Vector2 currPosition = transform.position;
        float attackHalfWidth = attackObj.GetComponent<SpriteRenderer>().bounds.extents.x;

        if (facingRight)
        {
            currPosition += Vector2.right * (playerHalfWidth + attackHalfWidth);
        }
        else
        {
            currPosition += Vector2.left * (playerHalfWidth + attackHalfWidth);
        }
        attackObj.transform.position = currPosition;
        attackObj.SetActive(true);
        StartCoroutine(DisableAfterTime(attackObj, attackLongevity));
    }

    public void ThrowKnife()
    {
        if (knivesCount > 0)
        {
            Vector2 currPosition = transform.position;
            float knifeHalfWidth = throwingKnife.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (facingRight)
            {
                currPosition += Vector2.right * (playerHalfWidth + knifeHalfWidth);
            }
            else
            {
                currPosition += Vector2.left * (playerHalfWidth + knifeHalfWidth);
            }

            GameObject knife = Instantiate(throwingKnife, currPosition, Quaternion.identity);
            ThrowingKnifeScript knifeScript = knife.GetComponent<ThrowingKnifeScript>();
            knifeScript.SetDirection(facingRight);
            knivesCount--;
        }
        else { ShowErrorMessage("no knives left"); }


    }
    public void Dash()
    {
        if (currentDash == null)
        {
            Vector2 currPosition = transform.position;
            if (facingRight)
            {
                if (dashObj != null)
                {
                    currPosition += Vector2.right * dashObj.transform.localScale.x / 2; //blockOffset = object.x???

                    GameObject newDash = Instantiate(dashObj, currPosition, Quaternion.identity);
                    DashBehavior dashBehavior = newDash.GetComponent<DashBehavior>();
                    if (dashBehavior != null)
                    {
                        dashBehavior.player = gameObject;//REMEMBER WHO INSTANTIATED
                    }
                    currentDash = newDash;
                }

                //Reset for player movement
                currPosition = transform.position;
                currPosition += Vector2.right * dashLength; //blockOffset = object.x???
                if (currPosition.x >= 9)
                {
                    currPosition.x = 8;
                }
                transform.position = currPosition;
            }
            else
            {
                if (dashObj != null)
                {
                    currPosition += Vector2.left * dashObj.transform.localScale.x / 2; //blockOffset = object.x???
                    GameObject newDash = Instantiate(dashObj, currPosition, Quaternion.identity);
                    DashBehavior dashBehavior = newDash.GetComponent<DashBehavior>();
                    if (dashBehavior != null)
                    {
                        dashBehavior.player = gameObject;//REMEMBER WHO INSTANTIATED
                    }
                    currentDash = newDash;
                }

                //Reset for player movement
                currPosition = transform.position;
                currPosition += Vector2.left * dashLength;
                if (currPosition.x <= -9)
                {
                    currPosition.x = -8;
                }
                transform.position = currPosition;
            }
        }
    }
    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float smoothSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, 0.1f);
        rb.velocity = new Vector2(smoothSpeed, rb.velocity.y);
    }
    IEnumerator DisableAfterTime(GameObject attackObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        attackObj.SetActive(false);
    }
    public void RefreshResources()
    {
        coinUI.GetComponent<TextMeshProUGUI>().text = "coins: " + myCoinsNumber;
        shieldUI.GetComponent<TextMeshProUGUI>().text = "shields: " + shieldsCount;
        knivesUI.GetComponent<TextMeshProUGUI>().text = "knives: " + knivesCount;
    }
    public void Die()
    {
        if (!shielded && !autoShielded)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else if (shielded)
        {
            shieldBreakContainer.GetComponent<AudioSource>().Play();
            shielded = false;
            shieldObj.SetActive(false); //doesn't disable automatically
            shieldsCount--;
        }
        else if (autoShielded)
        {
            shieldBreakContainer.GetComponent<AudioSource>().Play();
            autoShielded = false;
        }
    }

}
