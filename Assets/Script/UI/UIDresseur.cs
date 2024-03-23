using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDresseur : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField]
    private List<GameObject> screens;

    public void setOnlyScreen(string name)
    {
        foreach (GameObject screen in screens)
        {
            if (screen.name != name)
            {
                screen.SetActive(false);
            }
            else
            {
                screen.SetActive(true);
            }
        }
    }
    public void setScreen(string name)
    {
        foreach (GameObject screen in screens)
        {
            if (screen.name == name)
            {
                screen.SetActive(!screen.activeSelf);
            }
        }
    }
}
