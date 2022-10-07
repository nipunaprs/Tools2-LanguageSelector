using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{

    List<string> textFromMyInputs = new List<string>();
    Text[] texts;
    
    string[] keyNames;
    string[] languages;
    Dictionary<string, LanguageGroup> groupDict = new Dictionary<string, LanguageGroup>();
    string currentSelectedLang = "";


    //Public variables
    public Dropdown mainDropdown;


    // Start is called before the first frame update
    void Start()
    {
        texts = FindObjectsOfType<Text>();
        LoadAllLangGroups();
        LoadLanguages();
        PopulateDropdown();
    }


    // Update is called once per frame
    void Update()
    {
        //Update currentSelectedLang
        if (currentSelectedLang != mainDropdown.options[mainDropdown.value].text)
        {
            currentSelectedLang = mainDropdown.options[mainDropdown.value].text;
            UpdateTextLabels();
        }
            


    }

    void LoadLanguages()
    {
        string languagesPath = "langDatabase";
        Object[] objects = Resources.LoadAll(languagesPath, typeof(LanguageType)); //this already points to the resources folder so u don't need the full path
        if (objects.Length != 0)
        {
            languages = new string[objects.Length];
            int index1 = 0;

            //Take each language name and put into array
            foreach (LanguageType lang in objects)
            {
                languages[index1] = lang.langName;
                index1++;
            }
        }


    }


    void LoadAllLangGroups()
    {
        string languagesPath = "langGroupDatabase";
        Object[] objects = Resources.LoadAll(languagesPath, typeof(LanguageGroup));
        Debug.Log(objects.Length);

        if (objects.Length != 0)
        {
            int index1 = 0;
            keyNames = new string[objects.Length];

            

            foreach (LanguageGroup langGroup in objects)
            {
                //Add key to keys array
                keyNames[index1] = langGroup.key;
                
                //Add each langGroup to dictionary alongside key
                groupDict.Add(langGroup.key, langGroup);

                
                index1++;
            }
        }
        
    }


    void UpdateTextLabels()
    {
        foreach (Text gameText in texts)
        {
            //If txtfields key is in the keys
            string nameofObject = gameText.name;
            //Debug.Log(keyNames[0]);
            
            if (keyNames.Any(nameofObject.Contains))
            {
                
                foreach (LanguageDetails langDetails in groupDict[nameofObject].langTextArr)
                {
                    Debug.Log(langDetails.langText);

                    if (langDetails.langName == currentSelectedLang)
                    {
                        gameText.text = langDetails.langText;
                    }
                }
                    //groupDict[nameofObject].langTextArr[mainDropdown.value].langName == currentSelectedLang

                
            }
            


        }
    }

    void PopulateDropdown()
    {

        List<string> options = new List<string>();
        foreach (string languageName in languages)
        {
            options.Add(languageName); // Or whatever you want for a label
        }
        mainDropdown.ClearOptions();
        mainDropdown.AddOptions(options);

    }

}
