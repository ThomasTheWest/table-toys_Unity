using UnityEngine;

public class acSealTest : MonoBehaviour
{
    [SerializeField] private Texture[] seals;
    [SerializeField] private Material mat;
    private int index;

    private void Start()
    {
        index = 0;
    }
    void Update()
    {// This does change the actual material file's properties but that's okay for the purposes of this exercise
        mat.SetTexture("_Seal", seals[index]);    
    }

    public void ProgressSeal(bool prev)
    {
        if (prev && index != 0)
            index--;    
        else if (!prev && index != seals.Length - 1)
            index++;
    }
}
