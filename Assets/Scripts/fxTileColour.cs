using UnityEngine;

public class fxTileColour : MonoBehaviour
{// For use with materials that use _shaderTileSelection. Script assigned to the tile itself to receive signals.
    [SerializeField] private int state = 0; //0 is no vfx, 1 is attack, 2 is defend, 3 is move
    [SerializeField] private Renderer mainRenderer;
    void Start()
    {// Sets to default colour
        mainRenderer = GetComponent<Renderer>();

        if (mainRenderer != null)
        {
            //Debug.Log("Renderer found!");
            mainRenderer.material.SetFloat("_Mode", 0);
        }
        //else
            //Debug.Log("No renderer found!");

    }
    void Update()
    {// Checks for any changes in state. Maybe a bit slower than just changing it anytime it's changed in TileRecieveSignal
        mainRenderer.material.SetFloat("_Mode", state);
    }

    public void TileRecieveSignal(int newState)
    {// Public function to change the state
        state = newState;
    }

    public void TileRecieveSignalButtonAdd(int add)
    {// This function is just for testing
        int newState;
        newState = state + add;

        if (newState <= 2)
            state = newState;
        else
            state = 3;

        Debug.Log(state);
    }

    public void TileRecieveSignalButtonSubtract(int subtract)
    {// This function is just for testing
        int newState;
        newState = state - subtract;

        if (newState >= 0)
            state = newState;
        else
            state = 0;

        Debug.Log(state);
    }
}
