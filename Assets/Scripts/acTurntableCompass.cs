using System.Xml;
using System.Collections;
using TMPro;
using UnityEngine;

public class acTurntableCompass : MonoBehaviour
{// This script is just an example of a social encounter. The same encounter shows up regardless of what direction chosen tee hee (for now)

    [SerializeField] private GameObject[] directionsUI;
    [SerializeField] private GameObject[] choicesUI;
    [SerializeField] private TextMeshProUGUI monologue;
    [SerializeField] private Material matRelqua;

    private void Start()
    {
        foreach (GameObject choices in choicesUI)
            choices.SetActive(false);
    }
    public void DirectionPressed(int direction)
    {// 0 is East, 1 is South, 2 is West. It doesn't actually matter which one you choose for this example.

        foreach (GameObject directions in directionsUI)
            directions.SetActive(false);

        foreach (GameObject choices in choicesUI)
            choices.SetActive(true);

        monologue.text = "You come across some starving soldiers. Too emaciated to fight, but with their alliegience unclear, you can't let them walk.";
    }

    public void doSomething(int choice)
    {// This function is called by every choice button, with a different assigned choice value explained below

        if (choice == 0)
        {// Paying them off

            monologue.text = "The men take your gold and march back north behind you. Your show of mercy reflects a more human character. +1 Charm -50 Gold";
        }
        else if (choice == 1)
        {// Swearing them in

            GameObject cards = choicesUI[5]; // These are the card models. Make sure they are element 5 in the inspector!

            foreach (var renderer in cards.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = matRelqua;
                Debug.Log("Did something!");
            }

            monologue.text = "The soldiers are grateful to be in your army. While not the strongest, they nonetheless lend their restored strength to the Crown's cause.";
        }
        else if (choice == 2)
        {// Execute them

            monologue.text = "You hang the bodies of the soldiers by the road as you march on. It's a simple and smart message to those that might see you as a lesser leader. +1 Intelligence";
        }
    }
}
