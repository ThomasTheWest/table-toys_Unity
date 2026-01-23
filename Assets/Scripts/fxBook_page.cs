using System;
using UnityEngine;

public class fxBook_page : MonoBehaviour
{// This just handles animation events from the page's animator and sends them to the main book script

    [SerializeField] private fxBook mainScript;
    public void DisappearEvent(int prev)
    {
        if (prev == 1)
        {
            mainScript.Disappear(true);
        }
        else
        {
            mainScript.Disappear(false);
        }
    }
}
