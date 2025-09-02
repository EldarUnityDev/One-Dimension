using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stages
{
    public List<GameObject> enemies;
    public List<float> position;
    public List<string> dialogueName;
    public List<string> dialogueLines;
    public bool transitionRight;
    public bool hasIntro;
    public Stages()
    {
        enemies = new List<GameObject>();
        position = new List<float>();
        dialogueName = new List<string>();
        dialogueLines = new List<string>();
    }
}
