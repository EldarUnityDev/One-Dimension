using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public TextMeshProUGUI myText;
    public bool musicOn;
    // Start is called before the first frame update
public void musicSwitch()
    {
        if (!musicOn)
        {
            musicOn = true;
            myText.text = "music ON";
        }
        else
        {
            musicOn = false;
            myText.text = "music OFF";
        }
        References.levelManager.GetComponent<LevelManager>().MusicButton();
    }
}
