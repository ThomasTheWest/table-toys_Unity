using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class chChargen : MonoBehaviour
{//Includes UI and data loading

    [SerializeField] chOutfits outfitsManager;

    [Header("Data Files")]
    CosmeticStatus cosmeticStatus;
    string filePath;
    const string fileName = "Cosmetics.json";

    public struct CosmeticStatus
    {
        public Color skinColour;
        public int torsoIndex;
        public int headIndex;
    }

    private void Start()
    {
        filePath = Application.persistentDataPath;
        cosmeticStatus = new CosmeticStatus();
        Debug.Log(filePath);
        LoadCosmeticStatus();
    }

    public void LoadCosmeticStatus()
    {
        if (File.Exists(filePath + "/" + fileName))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + fileName);

            cosmeticStatus = JsonUtility.FromJson<CosmeticStatus>(loadedJson);
            Debug.Log("Cosmetic Info found, loading");
        }
        else
        {
            cosmeticStatus.skinColour = new Vector4(0f, 0f, 0f);
            cosmeticStatus.torsoIndex = 0;
            cosmeticStatus.headIndex = 0;
            Debug.Log("Cosmetic Info not found. Switching to default values");
        }

        outfitsManager.loadTorso(cosmeticStatus.torsoIndex);
        outfitsManager.loadHead(cosmeticStatus.headIndex);
        //outfitsManager.loadSkintone(cosmeticStatus.skinColour);
    }

    public void SaveGameStatus()
    {
        string cosmeticStatusJson = JsonUtility.ToJson(cosmeticStatus);
        File.WriteAllText(filePath + "/" + fileName, cosmeticStatusJson);
        Debug.Log("Cosmetics saved");
    }
    
    public void navigateTorsos(bool forwards)
    {
        int torsoAmount = outfitsManager.torsoAmount();// This is so if more cosmetics are added to the outfits manager script, nothing needs to be changed here

        if (forwards)
        {
            if (cosmeticStatus.torsoIndex == torsoAmount - 1)
                cosmeticStatus.torsoIndex = 0;
            else
                cosmeticStatus.torsoIndex++;
        }
        else
        {
            if (cosmeticStatus.torsoIndex == 0)
                cosmeticStatus.torsoIndex = torsoAmount - 1;
            else
                cosmeticStatus.torsoIndex--;
        }

        outfitsManager.loadTorso(cosmeticStatus.torsoIndex);
    }

    public void navigateHeads(bool forwards)
    {
        int headAmount = outfitsManager.headAmount();

        if (forwards)
        {
            if (cosmeticStatus.headIndex == headAmount - 1)
                cosmeticStatus.headIndex = 0;
            else
                cosmeticStatus.headIndex++;
        }
        else
        {
            if (cosmeticStatus.headIndex == 0)
                cosmeticStatus.headIndex = headAmount - 1;
            else
                cosmeticStatus.headIndex--;
        }

        outfitsManager.loadHead(cosmeticStatus.headIndex);
    }
}
