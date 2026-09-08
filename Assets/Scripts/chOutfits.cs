using UnityEngine;
using UnityEngine.UI;

public class chOutfits : MonoBehaviour
{//Stores info on what meshes get enabled for each outfit and visually loads it on the character

    public static chOutfits instance;

    [SerializeField] GameObject[] torso0, torso1;
    [SerializeField] GameObject[] head0, head1, head2;

    [SerializeField] Material skinMat, eyeMat;

    private GameObject[][] torsoList, headList;

    [Header("Data Files")]
    [SerializeField] private Sprite[] sprites;

    void Awake()
    {
        torsoList = new GameObject[][] { torso0, torso1 };
        headList = new GameObject[][] { head0, head1, head2 };

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void loadTorso(int index)
    {
        //Debug.Log(index);

        foreach (GameObject[] torso in torsoList)
            foreach (GameObject segment in torso)
                segment.SetActive(false);


        foreach (GameObject segment in torsoList[index])
        {
            segment.SetActive(true);

            SkinnedMeshRenderer segmentRenderer = segment.GetComponent<SkinnedMeshRenderer>();

            if (segmentRenderer != null)
            {
                Material[] materials = segmentRenderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name.Contains(skinMat.name))
                        materials[i].SetColor("_BaseColor", chChargen.instance.skinColour);

                    segmentRenderer.materials = materials;
                }
            }
        }


    }

    public void loadHead(int index)
    {
        //Debug.Log(index);

        foreach (GameObject[] head in headList)
            foreach (GameObject segment in head)
                segment.SetActive(false);


        foreach (GameObject segment in headList[index])
        {
            segment.SetActive(true);

            SkinnedMeshRenderer segmentRenderer = segment.GetComponent<SkinnedMeshRenderer>();
            MeshRenderer eyeRenderer = segment.GetComponent<MeshRenderer>();//this will be redundant soon. rn the eyes are just primitives I dropped in to make a scene

            if (segmentRenderer != null)
            {
                Material[] materials = segmentRenderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name.Contains(skinMat.name))
                        materials[i].SetColor("_BaseColor", chChargen.instance.skinColour);

                    segmentRenderer.materials = materials;
                }
            }
            else if (eyeRenderer != null)
            {
                Material[] materials = eyeRenderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].name.Contains(eyeMat.name))
                        materials[i].SetColor("_BaseColor", chChargen.instance.eyeColour);

                    eyeRenderer.materials = materials;
                }
            }
        }
    }

    //These are called when editing the colour only
    public void changeSkintone(Color skintone)
    {

    }

    public void changeEyeColour(Color skintone)
    {

    }

    public int torsoAmount()
    {
        return torsoList.Length;
    }

    public int headAmount()
    {
        return headList.Length;
    }
}
