using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    AudioSource m_AudioSource;
    public GameObject errorObj;
    void Update()
    {
        
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void ShowErrorMessage(string message)
    {
        errorObj.SetActive(true);
        errorObj.GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(DisableAfterTime(errorObj, 1.5f));
    }
    public void BuyShield()
    {
        PlayerMovement playerInfo = References.player.GetComponent<PlayerMovement>();
        if (playerInfo.myCoinsNumber >= 1 && playerInfo.shieldsCount < 3)
        {
            playerInfo.shieldsCount++;
            playerInfo.myCoinsNumber -= 1;
            //play Ka-Ching sound
            m_AudioSource.Play();

        }
        else if(playerInfo.myCoinsNumber == 0)
        {
            ShowErrorMessage("not enough coins");
        }else if(playerInfo.shieldsCount >= 3)
        {
            ShowErrorMessage("too many shields");
        }
    }

    public void BuyKnife()
    {
        PlayerMovement playerInfo = References.player.GetComponent<PlayerMovement>();
        if (playerInfo.myCoinsNumber >= 2 && playerInfo.knivesCount < 3)
        {
            playerInfo.knivesCount++;
            playerInfo.myCoinsNumber -= 2;

            //play Ka-Ching sound
            m_AudioSource.Play();

        }
        else if( playerInfo.myCoinsNumber < 2)
        {
            ShowErrorMessage("not enough coins");
        }else if(playerInfo.knivesCount >= 3)
        {
            ShowErrorMessage("too many knives");
        }
    }
    public void BuyAutoShield()
    {
        PlayerMovement playerInfo = References.player.GetComponent<PlayerMovement>();
        if (playerInfo.myCoinsNumber >= 5 && !playerInfo.autoShielded)
        {
            playerInfo.autoShielded = true;
            playerInfo.myCoinsNumber -= 5;

            //play Ka-Ching sound
            m_AudioSource.Play();

        }
        else if (playerInfo.myCoinsNumber < 5)
        {
            ShowErrorMessage("can't afford it");
        }
        else if (playerInfo.autoShielded)
        {
            ShowErrorMessage("can only wear 1");
        }
    }
    IEnumerator DisableAfterTime(GameObject attackObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        attackObj.SetActive(false);
    }
}
