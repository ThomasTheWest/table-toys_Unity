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
    public int indexForward, indexBackward;
    private int indexMax;
    private int beadsRight;

    private bool isSliding = false;
    [SerializeField] private float slideDuration = 2.0f;
    void Start()
    {
        indexMax = beads.Length - 1;
        indexBackward = 0;
        indexForward = 0;
        beadsRight = 0;
    }

    void Update()
    {

    }
    private IEnumerator SlideRight()
    {
        if (indexForward == indexMax && beadsRight == beads.Length) yield break;
        
        isSliding = true;
        
        float elapsed = 0f;
        
        while (elapsed <= slideDuration)
        {
            beads[indexForward].transform.position = Vector3.Lerp(beads[indexForward].transform.position, posForward[indexForward].position, slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (indexForward >= 1)
        {
            indexBackward++;
        }

        if (indexForward != indexMax)
        {
            indexForward++;
        }

        beadsRight++;

        isSliding = false;

        Debug.Log("indexForward: " + indexForward + " indexBackward: " + indexBackward + " Beads on Right: " + beadsRight);
    }

    private IEnumerator SlideLeft()
    {
        if (indexForward == 0 && beadsRight > 0) yield break;

        isSliding = true;

        float elapsed = 0f;

        while (elapsed <= slideDuration)
        {
            if (beadsRight == beads.Length && indexForward == indexMax)
            {
                beads[indexForward].transform.position = Vector3.Lerp(beads[indexForward].transform.position, posBackward[indexBackward].position, slideDuration);
            }
            else
            {
                beads[indexForward - 1].transform.position = Vector3.Lerp(beads[indexForward - 1].transform.position, posBackward[indexBackward].position, slideDuration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        beadsRight--;

        if (beadsRight != beads.Length || beadsRight != beads.Length - 1)
        {
            indexForward--;
        }
        
        indexBackward--;

        isSliding = false;

        Debug.Log("indexForward: " + indexForward + " New indexBackward: " + indexBackward + " Beads on Right: " + beadsRight);
    }

    public void StartSlideRight()
    {
        if (indexForward <= indexMax && !isSliding && beadsRight != beads.Length)
        {
            StartCoroutine(SlideRight());
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
            StartCoroutine(SlideLeft());
        }
        else
        {
            return;
        }
    }
}
