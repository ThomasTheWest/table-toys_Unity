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
    [Header("Dice")]
    [SerializeField] GameObject[] qualityDice; // Lists the dice scene objects
    [SerializeField] Mesh[] diceShape; // This includes each dice type, 0 is d2, 1 is d4, 2 is d6, 3 is d8, 4 is d10, 5 is d12

    [Header("UI")]
    [SerializeField] private TMP_Text qualityPointsDisplay; // Header that displays your remaining quality points
    [SerializeField] private GameObject rollingUI; // Button to roll dice
    [SerializeField] private GameObject confirmDiceUI; // Button to confirm selected dice
    [SerializeField] private GameObject[] setupUI; // Character building interface
    [SerializeField] private GameObject[] rolledUI; // Interface for after you've rolled the dice
    [SerializeField] private TMP_Text[] rolledTextUI; // dice roll values are displayed on these text bits. Might be redundant.

    [Header("Other")]
    [SerializeField] private fxAbacus_mult[] abaci; // Abacus scripts that dice values are sent to
    [SerializeField] private bool allowanceOn = false; // Decides if players get a base allowance per turn
    [SerializeField] private int allowancePerTurn = 1; 
    [SerializeField] private bool zeroesAllowed = true; // Decides if dice roll ranges start from 0 or 1
    [SerializeField] private int qualityPointsLeft = 0; // Right now, 3 d6es are the default character build, making 9 points the current max. Still leaving this here just in case we want to balance more.
    private int qualityPointsMax; //This is the max smth can be, set in Start()
    [SerializeField] private int actionsPerRound = 2; //Amount of actions players can take between regular rolls.

    [HideInInspector] public int qualityPointsB, qualityPointsW, qualityPointsC;
    private int[] output; // These are used by the dice roll coroutines to output values to be used by the abacus scripts.
    private int actionCounter; // 2 for now
    private bool diceSelected; // Just checks if you've confirmed your dice selection

    void Start()
    {
        // Hides the button to roll dice before you've decided which ones you want
        rollingUI.SetActive(false);

        // Players are given three d6s (worth 3 quality pts) by default, leaving them with no spare quality pts to upgrade dice.
        qualityPointsLeft = 0;
        qualityPointsB = 3;
        qualityPointsW = 3;
        qualityPointsC = 3;

        // This just clears UI elements displaying dice values that haven't been rolled yet
        foreach (GameObject element in rolledUI)
            element.SetActive(false);

        // Since output[] is private, this just makes sure RollDice() knows it has 3 elements to write to
        output = new int[4];

        //Adds up all points that exist in the system
        qualityPointsMax = qualityPointsB + qualityPointsC + qualityPointsW + qualityPointsLeft;
    }

    void Update()
    {
        // Makes sure this updates the remaining quality points for player
        qualityPointsDisplay.text = "Quality Points remaining: " + qualityPointsLeft.ToString();

        // Once you choose 2 dice, you can roll again to get new values
        if (actionCounter == actionsPerRound)
        {
            ConfirmDice();

            foreach (GameObject element in rolledUI)
                element.SetActive(false);

            actionCounter = 0;
        }

        // Confirm button only appears if you've spent all your spare Quality Points
        if (qualityPointsLeft == 0 && !diceSelected)
            confirmDiceUI.SetActive(true);
        else if (qualityPointsLeft != 0)
            confirmDiceUI.SetActive(false);

        // This bit just sets the dice up/downgrade buttons to disappear if they hit their max/min quality pts
        // Check SetupUI[] for what this is making (in)active. ik it doesn't look pretty
        if (!diceSelected)
        {
            // This minus 2 is just the fact that if you want to max smth out, there's two points still assigned to the remaining d2s
            if (qualityPointsB == qualityPointsMax - 2 || qualityPointsLeft == 0)
                setupUI[0].SetActive(false); //B+ button
            else if (qualityPointsB < qualityPointsMax)
                setupUI[0].SetActive(true);

            if (qualityPointsB == 1)
                setupUI[1].SetActive(false); //B-
            else if (qualityPointsB > 1)
                setupUI[1].SetActive(true);

            if (qualityPointsW == qualityPointsMax - 2 || qualityPointsLeft == 0)
                setupUI[2].SetActive(false); //W+ button
            else if (qualityPointsW < qualityPointsMax)
                setupUI[2].SetActive(true); 

            if (qualityPointsW == 1)
                setupUI[3].SetActive(false); //W-
            else if (qualityPointsW > 1) 
                setupUI[3].SetActive(true);

            if (qualityPointsC == qualityPointsMax - 2 || qualityPointsLeft == 0)
                setupUI[4].SetActive(false); //C+ button
            else if (qualityPointsC < qualityPointsMax)
                setupUI[4].SetActive(true);

            if (qualityPointsC == 1)
                setupUI[5].SetActive(false); //C-
            else if (qualityPointsC > 1)
                setupUI[5].SetActive(true);
        }
    }

    public void IncreaseValue(int qualityIndex)
    {// When this is called by a UI button, the button should be set up with the right index. 0 is Bravery, 1 is Wit, 2 is Charm. 

        if (qualityPointsLeft != 0 && ((qualityIndex == 0 && qualityPointsB != diceShape.Length) || (qualityIndex == 1 && qualityPointsW != diceShape.Length) || (qualityIndex == 2 && qualityPointsC != diceShape.Length)))
        {
            MeshFilter currentFilter = qualityDice[qualityIndex].GetComponent<MeshFilter>();

            // This bit sees which quality dice is being upgraded and upgrades the appropriate int
            if (qualityIndex == 0)
                qualityPointsB++;
            else if (qualityIndex == 1)
                qualityPointsW++;
            else if (qualityIndex == 2)
                qualityPointsC++;

            qualityPointsLeft--;

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

            qualityPointsLeft++;

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
    {// Once you press the confirm button, you're locked into the dice you have and can now roll them

        foreach (GameObject element in setupUI)
            element.SetActive(false);

        rollingUI.SetActive(true);

        diceSelected = true;
    }

    public void RollDice()
    {// All three dice are rolled, giving a different range of values depending on what kind of dice they are. Called on UI button press

        // First int for the Roll coroutine is for the quality type, second for the type of dice.
        StartCoroutine(Roll(0, qualityPointsB));
        StartCoroutine(Roll(1, qualityPointsW));
        StartCoroutine(Roll(2, qualityPointsC));

        if (allowanceOn)
        {
            foreach (fxAbacus_mult element in abaci)
                element.TransmitPositiveValue(allowancePerTurn);
        }

        foreach (GameObject element in rolledUI)
            element.SetActive(true);

        rollingUI.SetActive(false);
    }

    private IEnumerator Roll(int qualityIndex, int diceIndex)
    {
        // diceIndex/qualityPointsX is equal to the dice value divided by two eg. diceIndex 3 corresponds to a d6
        // As usual, qualityIndex 0 is Bravery, 1 is Wit, 2 is Charm
        int roll;

        if (zeroesAllowed)
            roll = UnityEngine.Random.Range(0, diceIndex * 2);
        else
            roll = UnityEngine.Random.Range(1, (diceIndex * 2) + 1);

        output[qualityIndex] = roll;

        rolledTextUI[qualityIndex].text = roll.ToString();

        yield return null;
    }

    public void ChooseDice(int qualityIndex)
    {// Lets you select which dice to add to your abacus. Zeroes can't be selected as an action, so it just does nothing if you click those rolls

        if (qualityIndex <= 2 && output[qualityIndex] != 0)
        {
            abaci[qualityIndex].TransmitPositiveValue(output[qualityIndex]);
            rolledUI[qualityIndex].SetActive(false);
        }        

        //Since you can only select two dice max, there's a counter to make sure you're not selecting all three
        if (output[qualityIndex] != 0 || qualityIndex > 2)
            actionCounter++;
    }
}