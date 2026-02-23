using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class fxBook : MonoBehaviour
{// More advanced version of fxRuleBook for a more complex mesh (with animations!)

    private int index = 0; // This is the leaf index rather than the index for individual pages. Relationship with left page of corresponding leaf is that it's half of that page's element value (counting up from 0) easy peasy 
    private bool isTurning = false;

    [Header("Main Pages")]
    [SerializeField] private Texture[] pages; // Collect my pages. Make sure this has an even amount of items
    [SerializeField] private Material pageLeft, pageRight;

    [Header("Turning Pages")]
    [SerializeField] private GameObject pageForward;
    [SerializeField] private Material pageForwardFront, pageForwardBack;
    [SerializeField] private GameObject pageBackward;
    [SerializeField] private Material pageBackwardFront, pageBackwardBack;

    private Animator animatorForward, animatorBackward;

    void Start()
    {
        if (pages.Length % 2 == 1)
            Debug.Log("Uh uh girlfriend...that array does NOT have an even amount of elements...");

        // Makes sure that at runtime it's at the first two pages of the array
        pageLeft.mainTexture = pages[index*2];
        pageRight.mainTexture = pages[index*2 + 1];

        // animators!
        animatorForward = pageForward.GetComponent<Animator>();
        animatorBackward = pageBackward.GetComponent<Animator>();

        // Make sure those turning pages aren't visible at startup...
        pageForward.SetActive(false);
        pageBackward.SetActive(false);
    }

    public void TurnForward()
    {
        if (index + 1 != pages.Length / 2 && !isTurning)
            StartCoroutine(TurnPage(false));
    }

    public void TurnBackward()
    {
        if (index != 0 && !isTurning)
            StartCoroutine(TurnPage(true));
    }
    private IEnumerator TurnPage(bool back)
    {// yeah I kept the index checks in here too...oh well

        if (back && index != 0)
        {// turn page backward
            isTurning = true;

            pageBackward.SetActive(true);
            animatorBackward.SetBool("isTurning", true);
            pageBackwardFront.mainTexture = pages[index * 2];
            pageBackwardBack.mainTexture = pages[index * 2 - 1];

            pageLeft.mainTexture = pages[(index - 1) * 2];
        }
        else if (!back && index + 1 != pages.Length/2) //handy way of checking if you have any more pages left based on current index
        {// turn page forward
            isTurning = true;

            
            pageForward.SetActive(true);
            animatorForward.SetBool("isTurning", true);
            animatorForward.Update(0f);
            pageForwardFront.mainTexture = pages[(index + 1) * 2 - 1];
            pageForwardBack.mainTexture = pages[(index + 1) * 2];

            pageRight.mainTexture = pages[(index + 1) * 2 + 1];
        }
        else
            yield return null;
    }

    public void Disappear(bool back)
    {// Resets the turning page objects
        if (back)
        {
            index --;

            pageBackward.SetActive(false);
            pageRight.mainTexture = pages[index * 2 + 1];
            animatorBackward.SetBool("isTurning", false);
        }
        else
        {
            index++;

            pageForward.SetActive(false);
            pageLeft.mainTexture = pages[index * 2];
            animatorForward.SetBool("isTurning", false);
        }

        isTurning = false;
        //Debug.Log(index);
    }
}
