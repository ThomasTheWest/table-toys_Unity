using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;

public class fxAbacus_mult : MonoBehaviour
{
    [SerializeField] private GameObject[] beads;
    [SerializeField] private Transform[] posForward;
    [SerializeField] private Transform[] posBackward;

    //indexForwards represents which position is next to move to. 0 also corresponds with the first bead, 1 the second ect.
    //indexMax is defined by the total number of beads. (Length of the array -1)
    [HideInInspector] public int indexForward = 0;
    private int indexMax;

    [Header("UI")]
    [SerializeField] private string quality; //the name of the quality the field is used for (ie. Bravery, Charm, Wit)
    [SerializeField] private TMP_Text valueDisplay;
    [SerializeField] private TMP_InputField valueInput;
    [SerializeField] private Button buttonConfirm;
    private string qualityInput; // Quality Dice value, retrieved from numberInput

    private int oldValue = 0; // This value is compared to for any changes in qualityInput. Once the beads have moved, oldInput is set to qualityInput.
    private int newValue = 0; // This is an int converted from the string from numberInput.text

    //private bool isSliding = false; // Defined to be true when we're in the routine to make the beads slide - used to end the coroutine
    //[SerializeField] private float slideDuration = 0.5f; // Not a duration, an interpolation value for a transform lerp. 
    void Start()
    {
        indexMax = beads.Length - 1;
    }

    private void Update()
    {
        if (oldValue != newValue)
        {
            if (newValue < oldValue) //if the new input is lower than the current one, slide beads to the left
            {
                StartSlideLeft();
                oldValue = newValue;
            }
            else if (newValue > oldValue) //if the old value is lower than the new input, slide beads to the right
            {
                StartSlideRight();
                oldValue = newValue;
            }
        }
       
        if (oldValue != 0)
        {
            valueDisplay.text = newValue.ToString();
        }
        else
        {
            valueDisplay.text = quality;
        }
    }

    public void ConfirmValue()
    {
        if (valueInput.text == "true")
        {
            newValue = 10;
        }
        else
        {
            int.TryParse(valueInput.text, out newValue);
        }

        valueInput.text = "";
        //Debug.Log (newValue);
    }

    private IEnumerator Slide(bool ToRight, int toMove) 
    {
        // Check whether there are any beads remaining to move. If not, just return
        if (ToRight && indexForward > indexMax ) yield break;
        if (!ToRight && indexForward <= 0 ) yield break;

        // isSliding makes sure the bead has finished moving before allowing any other beads to move.
        //isSliding = true;

        float elapsed = 0f;

        for (int i = 0; i < toMove; i++) //the transform while loop is done as many times as toMove is set (which is determined by either adding or subtracting from oldValue
        {
            //while (elapsed <= slideDuration) //this while logic was written when I assumed the interpolation value for the lerp was a duration. it doesn't work like that
            //{ taking out this while loop since vector3.lerp doesn't work like I thought it did
            if (ToRight)
                //beads[indexForward].transform.position = Vector3.Lerp(beads[indexForward].transform.position, posForward[indexForward].position, slideDuration);
                //vector3.Lerp does not do what I think it does, just going to do a straight transform change
                beads[indexForward].transform.position = posForward[indexForward].position;
            else
                //beads[indexForward - 1].transform.position = Vector3.Lerp(beads[indexForward - 1].transform.position, posBackward[indexForward - 1].position, slideDuration);
                beads[indexForward - 1].transform.position = posBackward[indexForward - 1].position;

                elapsed += Time.deltaTime;
                yield return null;
            //}

            if (ToRight)
            {
                indexForward++;
            }
            else
            {
                indexForward--;
            }

            //isSliding = false;

            Debug.Log("indexForward: " + indexForward);
        }
    }

    public void StartSlideRight()
    {
        if (indexForward <= indexMax)
        {
            StartCoroutine(Slide(true, newValue - oldValue));
        }
        else
        {
            return;
        }
    }

    public void StartSlideLeft()
    {
        if (indexForward > 0)
        {
            StartCoroutine(Slide(false, oldValue - newValue));
        }
        else
        {
            return;
        }
    }
}
