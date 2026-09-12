using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ColorPicker;

public class chChargen : MonoBehaviour
{//Includes UI and data loading

    public static chChargen instance;
    

    [Header("Data Files")]
    CosmeticStatus cosmeticStatus;
    string defaultCode = "ADD8E68B00005C2B24000009";//blue skin, red eyes, auburn hair, first outfit, first head, ninth sprite
    string code;
    string filePath;
    const string fileName = "Cosmetics.json";

    private int torsoIndex, headIndex, spriteIndex;
    [HideInInspector] public Color skinColour, eyeColour, hairColour;

    [Header("UI")]
    [SerializeField] private Image testImage0;
    [SerializeField] private Image testImage1;
    [SerializeField] private Image testImage2;
    [SerializeField] private Button[] buttons;

    [SerializeField] private GameObject[] colourPickers;
    private ColorPicker.ColorPicker activePicker;
    private int oldPicker = 3;

    public struct CosmeticStatus
    {
        public string characterCode;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        filePath = Application.persistentDataPath;
        cosmeticStatus = new CosmeticStatus();
        Debug.Log(filePath);
        LoadCosmeticData(false);        

        //Button assignment
        buttons[0].onClick.AddListener(delegate { navigateCosmetics(true, true); });
        buttons[1].onClick.AddListener(delegate { navigateCosmetics(true, false); });
        buttons[2].onClick.AddListener(delegate { navigateCosmetics(false, true); });
        buttons[3].onClick.AddListener(delegate { navigateCosmetics(false, false); });
    }

    private void Update()
    {
        if (oldPicker < 3)
        {
            chOutfits.instance.ChangeColour(activePicker.type, activePicker.CurrentSelectedColor);

            if (oldPicker == 0)
                testImage0.color = skinColour;
            else if (oldPicker == 1)    
                testImage1.color = eyeColour;
            else if (oldPicker == 2)
                testImage2.color = hairColour;
        }
    }

    public void LoadCosmeticData(bool loadDefaults)
    {//If loading defaults, it saves them at the same time

        string loadedJson = File.ReadAllText(filePath + "/" + fileName);

        cosmeticStatus = JsonUtility.FromJson<CosmeticStatus>(loadedJson);
        code = cosmeticStatus.characterCode;

        if (!loadDefaults)
        {
            if (File.Exists(filePath + "/" + fileName))
            {
                if (code.Length < 24)
                {
                    code = defaultCode;
                    Debug.Log("Cosmetic code length invalid. Loading default values");
                }
                else
                    Debug.Log("Cosmetic info found, loading " + code);
            }
            else
            {
                code = defaultCode;
                Debug.Log("Cosmetic info not found. Loading default values");
            }
        }
        else
        {
            code = defaultCode;

            string cosmeticStatusJson = JsonUtility.ToJson(cosmeticStatus);
            File.WriteAllText(filePath + "/" + fileName, cosmeticStatusJson);
            Debug.Log("Defaults loaded & preferences overwritten");
        }

        //Turns whatever was loaded from the above function into variables for the script
        string skinColourSubstring = code.Substring(0, 6);
        ColorUtility.TryParseHtmlString("#" + skinColourSubstring, out skinColour);

        string eyeColourSubstring = code.Substring(6, 6);
        ColorUtility.TryParseHtmlString("#" + eyeColourSubstring, out eyeColour);

        string hairColourSubstring = code.Substring(12, 6);
        ColorUtility.TryParseHtmlString("#" + hairColourSubstring, out hairColour);

        string torsoIndexSubstring = code.Substring(18, 2);
        torsoIndex = Int32.Parse(torsoIndexSubstring);

        string headIndexSubstring = code.Substring(20, 2);
        headIndex = Int32.Parse(headIndexSubstring);

        string spriteIndexSubstring = code.Substring(22, 2);
        spriteIndex = Int32.Parse(spriteIndexSubstring);

        testImage0.color = skinColour;
        //Debug.Log(skinColour + " " + skinColourSubstring);
        testImage1.color = eyeColour;
        //Debug.Log(eyeColour + " " + eyeColourSubstring);
        testImage2.color = hairColour;
        //Debug.Log(hairColour + " " + hairColourSubstring);

        chOutfits.instance.loadTorso(torsoIndex);
        chOutfits.instance.loadHead(headIndex);
    }

    public void SaveCosmeticStatus()
    {
        //Turns the edited ints into substrings to be reabsorbed back into the Big String
        string skinColourSubstring = ColorUtility.ToHtmlStringRGB(skinColour);

        string eyeColourSubstring = ColorUtility.ToHtmlStringRGB(eyeColour);

        string hairColourSubstring = ColorUtility.ToHtmlStringRGB(hairColour);

        string torsoIndexSubstring = torsoIndex.ToString();
        if (torsoIndexSubstring.Length == 1)
            torsoIndexSubstring = "0" + torsoIndexSubstring;

        string headIndexSubstring = headIndex.ToString();
        if (headIndexSubstring.Length == 1)
            headIndexSubstring = "0" + headIndexSubstring;

        string spriteIndexSubstring = headIndex.ToString();
        if (spriteIndexSubstring.Length == 1)
            spriteIndexSubstring = "0" + spriteIndexSubstring;

        cosmeticStatus.characterCode = skinColourSubstring + eyeColourSubstring + hairColourSubstring + torsoIndexSubstring + headIndexSubstring + spriteIndexSubstring;
        Debug.Log(cosmeticStatus.characterCode);

        string cosmeticStatusJson = JsonUtility.ToJson(cosmeticStatus);
        File.WriteAllText(filePath + "/" + fileName, cosmeticStatusJson);
        Debug.Log("Cosmetics saved");
    }

    //UI functions, should probably move this to a dedicated manager 
    public void navigateCosmetics(bool forwards, bool isHead)
    {

        int torsoAmount = chOutfits.instance.torsoAmount();// This is so if more cosmetics are added to the outfits manager script, nothing needs to be changed here
        int headAmount = chOutfits.instance.headAmount();

        if (isHead)
        {
            if (forwards)
            {
                if (headIndex == headAmount - 1)
                    headIndex = 0;
                else
                    headIndex++;
            }
            else
            {
                if (headIndex == 0)
                    headIndex = headAmount - 1;
                else
                    headIndex--;
            }

            chOutfits.instance.loadHead(headIndex);

        }
        else
        {
            if (forwards)
            {
                if (torsoIndex == torsoAmount - 1)
                    torsoIndex = 0;
                else
                    torsoIndex++;
            }
            else
            {
                if (torsoIndex == 0)
                    torsoIndex = torsoAmount - 1;
                else
                    torsoIndex--;
            }

            chOutfits.instance.loadTorso(torsoIndex);

        }
    }

    public void ToggleColourPicker(int type)//0 for skin, 1 for eyes, 2 for hair
    {
        if (oldPicker == type)
        {
            colourPickers[type].SetActive(false);
            activePicker = null;
            oldPicker = 3;
        }
            
        else
        {
            colourPickers[type].SetActive(true);
            activePicker = colourPickers[type].GetComponent<ColorPicker.ColorPicker>();
            oldPicker = type;
        }


        if (type == 0)
        {
            colourPickers[1].SetActive(false);
            colourPickers[2].SetActive(false);
        }
        else if (type == 1)
        {
            colourPickers[0].SetActive(false);
            colourPickers[2].SetActive(false);
        }
        else if (type == 2)
        {
            colourPickers[0].SetActive(false);
            colourPickers[1].SetActive(false);
        }
        else
        {
            colourPickers[0].SetActive(false);
            colourPickers[1].SetActive(false);
            colourPickers[2].SetActive(false);
        }
    }
}
