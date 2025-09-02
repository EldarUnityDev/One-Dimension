using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public Button myButton; 
    public KeyCode triggerKey = KeyCode.Space; 

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            myButton.onClick.Invoke();
        }
    }
}
