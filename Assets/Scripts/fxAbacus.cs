using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class fxAbacus : MonoBehaviour
{
    [SerializeField] private GameObject[] beads;
    [SerializeField] private Transform[] posForward;
    [SerializeField] private Transform[] posBackward;

    //indexForwards represents which position is next to move to. 0 also corresponds with the first bead, 1 the second ect.
    //indexBackwards works similarly but counting inwards from 0
    //indexMax is defined by the total number of beads. (Length of the array -1)
    //beadsRight keeps track of how many beads are on the right
    public int indexForward;
    private int indexMax;

    private bool isSliding = false; // Defined to be true when we're in the routine to make the beads slide - used to end the coroutine
    [SerializeField] private float slideDuration = 2.0f; // ### What is the unit of slide duration? What is the meaning?
    void Start()
    {
        indexMax = beads.Length - 1;
        indexForward = 0;
    }

    void Update()
    {

    }
    
    private IEnumerator Slide(bool ToRight) 
    {
        // Check whether there are any beads remaining to move. If not, just return
        if (ToRight && indexForward > indexMax ) yield break;
        if (!ToRight && indexForward <= 0 ) yield break;

        // There is a bead to be moved, so we tell the coroutine to keep returning until we've moved it
        isSliding = true;

        float elapsed = 0f;

        while (elapsed <= slideDuration)
        {
            if (ToRight) 
                beads[indexForward].transform.position = Vector3.Lerp(beads[indexForward].transform.position, posForward[indexForward].position, slideDuration);
            else
                beads[indexForward-1].transform.position = Vector3.Lerp(beads[indexForward-1].transform.position, posBackward[indexForward-1].position, slideDuration);


            elapsed += Time.deltaTime;
            yield return null;
        }
            
        if (ToRight)
        {
            indexForward++;
        }
        else
        {
            indexForward--;
        }


        isSliding = false;

        Debug.Log("indexForward: " + indexForward);

    }

    public void StartSlideRight()
    {
        if (indexForward <= indexMax && !isSliding )
        {
            StartCoroutine(Slide(true));
        }
        else
        {
            return;
        }
    }

    public void StartSlideLeft()
    {
        if (indexForward > 0 && !isSliding)
        {
            StartCoroutine(Slide(false));
        }
        else
        {
            return;
        }
    }
}
