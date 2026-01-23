using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class fxCandleLife : MonoBehaviour
{// This script controls the scale of a candle object that represents the player's remaining life points. Should go on the scale point for the candle, not the candlestick or candle scene objects.

    [Header("Components")]
    [SerializeField] Light flame;
    [SerializeField] GameObject halo;
    [SerializeField] GameObject[] pegs;
    [SerializeField] GameObject pegProp;
    [SerializeField] private Animator flameAnimator;

    [Header("Values")]
    [SerializeField] private int health; // Current HP. Serialized just for testing.
    [SerializeField] int healthTotal = 12; //Max HP
    [SerializeField] int healthCritical = 3; // Dramatic lighting is triggered when you reach this health and below
    public Color colourCritical = new Color(1f, 0.52f, 0f);

    private float rangeInitial, heightInitial;


    void Start()
    {
        health = healthTotal;
        heightInitial = transform.localScale.y;
        rangeInitial = flame.range;

        flameAnimator.SetBool("started", true);
    }

    void Update()
    {
        Vector3 scale;

        // This tells the animator whether the player is at critical health or at 0 health
        if (health <= healthCritical) 
        {
            flameAnimator.SetBool("isCritical", true);
            flame.color = colourCritical;
        }

        if (health <= 0)
            flameAnimator.SetBool("lit", false);

        //Scale is changed depending on what the current health is. health = healthTotal means candle is at max height.
        scale = transform.localScale;
        scale.y = (heightInitial / healthTotal) * health;
        transform.localScale = scale;
    }

    public void DecreaseLife(int damage)
    {
        int checkValue;

        checkValue = health - damage;

        if (checkValue <= 0)
            health = 0;
        else
            health -= damage;
            StartCoroutine(DropPeg(healthTotal - health - 1));
    }

    private IEnumerator DropPeg(int index)
    {// Called when you lose an LP. Very simply finds the next peg at the top of the candle and replaces it with a physics version that falls

        if (health > 0)
        {
            Collider oldPeg;

            oldPeg = pegs[index].GetComponent<Collider>();

            oldPeg.enabled = !oldPeg.enabled;
            Instantiate(pegProp, pegs[index].transform.position, pegs[index].transform.rotation);
            Destroy(pegs[index].gameObject);

            yield break;
        }
    }
}