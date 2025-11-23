using System;
using UnityEngine;

public class fxRuleBook : MonoBehaviour
{// very simply just changes book page mat textures according to button presses

    [SerializeField] private Texture[] pages; // collect my pages
    [SerializeField] private Material pageLeft, pageRight;
    private int index;

    void Start()
    {
        index = 0;
    }

    void Update()
    {// This does change the actual material file's properties but that's okay for the purposes of this exercise

        pageLeft.mainTexture = pages[index];
        pageRight.mainTexture = pages[index + 1];
    }
    public void FlipPage(bool prev)
    {
        if (prev && index != 0)
            index -= 2;
        else if (!prev && index != pages.Length - 2)
            index += 2;
    }
}
