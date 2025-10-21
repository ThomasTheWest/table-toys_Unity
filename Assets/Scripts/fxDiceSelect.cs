using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;

public class fxDiceSelect : MonoBehaviour
{
    [Header("Arrays")]
    [SerializeField] GameObject[] qualityDice; // Lists the dice scene objects
    [SerializeField] Mesh[] diceShape; // This includes each dice type, 0 is d2, 1 is d4, 2 is d6, 3 is d8, 4 is d10, 5 is d12

    [Header("UI")]
    [SerializeField] private TMP_Text qualityPointsDisplay;
    [SerializeField] private GameObject[] setupUI;
    [SerializeField] private GameObject rollingUI;

    [Header("Other")]
    //private int qualityPointsMax = 9;
    [HideInInspector] public int qualityPoints, qualityPointsB, qualityPointsW, qualityPointsC;
    private int outB, outW, outC;

    [SerializeField] private fxAbacus_mult abacusB, abacusW, abacusC;
    void Start()
    {
        rollingUI.SetActive(false);

        // Players are given three d6s (worth 3 quality pts) by default, leaving them with no spare quality pts to upgrade dice.
        // To get more they need to use DecreaseValue() to get more points to use on other dice
        qualityPoints = 0;
        qualityPointsB = 3;
        qualityPointsW = 3;
        qualityPointsC = 3;
    }
    void Update()
    {
        qualityPointsDisplay.text = "Quality Points remaining: " + (qualityPoints).ToString();
    }
    public void IncreaseValue(int qualityIndex)
    {// When this is called by a UI button, the button should be set up with the right index. 0 is Bravery, 1 is Wit, 2 is Charm. 

        if (qualityPoints != 0 && (qualityIndex == 0 && qualityPointsB != diceShape.Length - 1) || (qualityIndex == 1 && qualityPointsW != diceShape.Length - 1) || (qualityIndex == 2 && qualityPointsC != diceShape.Length - 1))
        {
            MeshFilter currentFilter = qualityDice[qualityIndex].GetComponent<MeshFilter>();

            // This bit sees which quality dice is being upgraded and upgrades the appropriate int
            if (qualityIndex == 0)
                qualityPointsB++;
            else if (qualityIndex == 1)
                qualityPointsW++;
            else if (qualityIndex == 2)
                qualityPointsC++;

            qualityPoints--;

            // This bit sets currentFilter up with the right dice model. The -1 is so qualityIndex matches up with the array order that starts at 0
            if (qualityIndex == 0)
                currentFilter.mesh = diceShape[qualityPointsB - 1];
            else if (qualityIndex == 1)
                currentFilter.mesh = diceShape[qualityPointsW - 1];
            else if (qualityIndex == 2)
                currentFilter.mesh = diceShape[qualityPointsC - 1];
        }
    }

    public void DecreaseValue(int qualityIndex)
    {
        if ((qualityIndex == 0 && qualityPointsB != 1) || (qualityIndex == 1 && qualityPointsW != 1) || (qualityIndex == 2 && qualityPointsC != 1))
        {
            MeshFilter currentFilter = qualityDice[qualityIndex].GetComponent<MeshFilter>();

            if (qualityIndex == 0)
                qualityPointsB--;
            else if (qualityIndex == 1)
                qualityPointsW--;
            else if (qualityIndex == 2)
                qualityPointsC--;

            qualityPoints++;

            // Can probably move this to its own method and call it for both IncreaseValue() and DecreaseValue()
            if (qualityIndex == 0)
                currentFilter.mesh = diceShape[qualityPointsB - 1];
            else if (qualityIndex == 1)
                currentFilter.mesh = diceShape[qualityPointsW - 1];
            else if (qualityIndex == 2)
                currentFilter.mesh = diceShape[qualityPointsC - 1];
        }
    }

    public void ConfirmDice()
    {
        foreach (GameObject element in setupUI)
        {
            element.SetActive(false);
        }

        rollingUI.SetActive(true);
    }

    public void RollDice()
    {// All three dice are rolled, giving a different range of values depending on what kind of dice they are

    }

    // Here be dice
    private void RollResult(int resultB, int resultW, int resultC)
    {

    }
    private IEnumerator Rolld2(int qualityIndex)
    {
        int roll = UnityEngine.Random.Range(1, 2);

        if (qualityIndex == 0)
            outB = 0;

        yield return null;
    }
}
