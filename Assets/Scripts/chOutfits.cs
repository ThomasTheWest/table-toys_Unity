using UnityEngine;

public class chOutfits : MonoBehaviour
{//Stores info on what meshes get enabled for each outfit and visually loads it on the character

    [SerializeField] GameObject[] torso0, torso1;
    [SerializeField] GameObject[] head0, head1, head2;

    [SerializeField] Material skinMat;

    private GameObject[][] torsoList, headList; 

    void Awake()
    {
        torsoList = new GameObject[][] { torso0, torso1 };
        headList = new GameObject[][] { head0, head1, head2 };
    }

    public void loadTorso(int index)
    {
        foreach (GameObject[] torso in torsoList)
            foreach (GameObject segment in torso)
                segment.SetActive(false);
            

        if (index == 0)
            foreach (GameObject segment in torso0)
                segment.SetActive(true);
        else if (index == 1)
            foreach (GameObject segment in torso1)
                segment.SetActive(true);
        else
            foreach (GameObject segment in torso0)
                segment.SetActive(true);

    }

    public void loadHead(int index)
    {
        foreach (GameObject[] head in headList)
            foreach (GameObject segment in head)
                segment.SetActive(false);


        if (index == 0)
            foreach (GameObject segment in head0)
                segment.SetActive(true);
        else if (index == 1)
            foreach (GameObject segment in head1)
                segment.SetActive(true);
        else if (index == 2)
            foreach (GameObject segment in head2)
                segment.SetActive(true);
        else
            foreach (GameObject segment in head0)
                segment.SetActive(true);
    }

    public void loadSkintone(Color skintone)
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
