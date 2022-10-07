using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;

public class LanguageSelectorWindow : EditorWindow //Type editorWindow not Monobehavior
{
    //Textures, Rectangles, Colors
    Texture2D headerSectionTexture;
    Texture2D mainSectionTexture;

    Rect headerSection;
    Rect mainSection;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);
    Color mainSectionColor = new Color(80f / 255f, 82f / 255f, 81f / 255f);

    //Create instance of the scriptableobject 
    static LanguageGroup languageGroup;

    //Array holding languages
    LanguageType[] existinglanguagesArr;
    static LanguageType test1Lang;

    public static LanguageType Lang1Data { get { return test1Lang; } }

    //Methods to return
    public static LanguageGroup LanguageInfo { get { return languageGroup;  } }


    //Languages
    int selectedInd = 0;
    string[] languages;
    static LanguageSelectorWindow window;


    //Menu Item will be under Window
    [MenuItem("Window/Language Selector")]
    static void OpenWindow()
    {
        window = (LanguageSelectorWindow)GetWindow(typeof(LanguageSelectorWindow));
        window.minSize = new Vector2(600, 100);
        window.Show();
    }



    //Similar to start function in reg code
    public void OnEnable()
    {
        InitData();
        InitTextures();
        LoadLanguages();

    }

    public static void InitData()
    {
        //Cast scriptable object created
        languageGroup = (LanguageGroup)ScriptableObject.CreateInstance(typeof(LanguageGroup));
        test1Lang = (LanguageType)ScriptableObject.CreateInstance(typeof(LanguageType));
        

    }

    public void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        mainSectionTexture = new Texture2D(1, 1);
        mainSectionTexture.SetPixel(0, 0, mainSectionColor);
        mainSectionTexture.Apply();


        //mainSectionTexture = Resources.Load<Texture2D>("textures/t1");
    }

    void LoadLanguages()
    {
        string languagesPath = "langDatabase";
        Object[] objects = Resources.LoadAll(languagesPath,typeof(LanguageType)); //this already points to the resources folder so u don't need the full path
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
            //Debug.Log(languages[0]);
        }


    }

    void SaveNewLanguage()
    {
        string newName = test1Lang.langName;
        if (!languages.Any(newName.Contains))
        {
            AssetDatabase.CreateAsset(LanguageSelectorWindow.Lang1Data, "Assets/resources/langDatabase/" + newName + ".asset");
            window.Close(); //Closing window to reload properly
        }
        else
        {
            Debug.Log("Sorry couldn't add that, please close the window and then try to add the language again or check if you ALREADY created the language.");
        }

       
    }


    //Similar to update function
    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMainArea();
    }

    void DrawLayouts()
    {
        //Header drawing
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width; //scales with the window
        headerSection.height = 50;

        //Main section
        mainSection.x = 0;
        mainSection.y = 50;
        mainSection.width = Screen.width;
        mainSection.height = Screen.height - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(mainSection, mainSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Language Selector");

        GUILayout.EndArea();
    }

    void DrawMainArea()
    {
        GUILayout.BeginArea(mainSection);

        //Creating Languages Section
        GUILayout.Label("P1 - Creating Languages");
        GUILayout.Label("Below you will see the current languages you can add matching text for each key.");
        GUILayout.Label("If you do not see any languages -- create one below");
        
        
        EditorGUILayout.BeginHorizontal();
        if (languages.Length != 0)
        {
            EditorGUILayout.Popup("Current Languages", selectedInd, languages);
            
        }
        EditorGUILayout.EndHorizontal();



        GUILayout.Label("Here you can create a language");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Language Name: ");
        test1Lang.langName = EditorGUILayout.TextField(test1Lang.langName);


        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Create Language!", GUILayout.Height(40)))
        {
            //Save language name and update the box at the top
            //((LanguageType)ScriptableObject.CreateInstance(typeof(LanguageType))).langName = EditorGUILayout.TextField(test1Lang.langName)
            Debug.Log(test1Lang.langName);
            SaveNewLanguage();
        }
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        GUILayout.Label("");
        GUILayout.Label("P2 - Creating Language Groups!");

        //Key section
        GUILayout.Label("Please enter a key");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Key: ");
        languageGroup.key = EditorGUILayout.TextField(languageGroup.key);
        GUILayout.EndHorizontal();

        /*
        GUILayout.BeginHorizontal();
        GUILayout.Label("No Of Languages: ");
        //languageGroup.noOfLanguages = EditorGUILayout.Slider(languageGroup.noOfLanguages,1.0f,5.0f);
        //languageGroup.noOfLanguages = Mathf.Round(languageGroup.noOfLanguages);
        GUILayout.EndHorizontal();*/


        if(GUILayout.Button("Create Language Group!",GUILayout.Height(40)) && languageGroup.key.Length != 0)
        {
            LanguageSettings.OpenWindow(languageGroup.key);
        }
        else
        {
            Debug.Log("Make sure you entered a key");
        }


        GUILayout.EndArea();

    }

}

//NEW WINDOW

public class LanguageSettings : EditorWindow
{
    static string key;
    static LanguageSettings window;
    static LanguageGroup langGroup;
    string[] languages;

    public static LanguageGroup GetLanguageGroup { get { return langGroup; } }

    public static void OpenWindow(string selectedkey)
    {
        key = selectedkey;
        window = (LanguageSettings)GetWindow(typeof(LanguageSettings));
        window.minSize = new Vector2(800, 400);
        window.Show();
    }


    void LoadLanguages()
    {
        string languagesPath = "langDatabase";
        Object[] objects = Resources.LoadAll(languagesPath, typeof(LanguageType)); //this already points to the resources folder so u don't need the full path
        if (objects.Length != 0)
        {
            languages = new string[objects.Length];
            int index1 = 0;

            LanguageSettings.GetLanguageGroup.langTextArr = new LanguageDetails[languages.Length];
            //LanguageSettings.GetLanguageGroup.langDict = new Dictionary<string, string>();

            //Take each language name and put into array
            foreach (LanguageType lang in objects)
            {
                //Creating the objects inside the array
                LanguageSettings.GetLanguageGroup.langTextArr[index1] = (LanguageDetails)ScriptableObject.CreateInstance(typeof(LanguageDetails));
                languages[index1] = lang.langName;
                //LanguageSettings.GetLanguageGroup.langDict[lang.langName] = "none";
                //Setting a generic text
                LanguageSettings.GetLanguageGroup.langTextArr[index1].langText = "none";
                index1++;
            }

            
        }


    }

    void SaveNewLanguageGroup()
    {
        
      
        AssetDatabase.CreateAsset(LanguageSettings.GetLanguageGroup, "Assets/resources/langGroupDatabase/" + key + ".asset");
        window.Close(); //Closing window to reload properly
        

    }

    void SaveLangTexts()
    {
        int index1 = 0;
        foreach(LanguageDetails langText in LanguageSettings.GetLanguageGroup.langTextArr)
        {
            AssetDatabase.CreateAsset(langText, "Assets/resources/langTextData/" + key + "-" + languages[index1] + ".asset");
            index1++;
        }

        
        window.Close(); //Closing window to reload properly


    }


    void InitData()
    {
        langGroup = (LanguageGroup)ScriptableObject.CreateInstance(typeof(LanguageGroup));
        langGroup.key = key;
    }

    public void OnEnable()
    {
        InitData();
        LoadLanguages();
    }

    private void OnGUI()
    {
        DrawSettings();
    }

    void DrawSettings()
    {





        GUILayout.Label("P3 - Building Language Group");
        GUILayout.Label("Current Key: " + key);
        GUILayout.Label("Here for each language that you created -- enter the corresponding text relating to the key");

        for (int i = 0; i < languages.Length; i++)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label(languages[i]);
            LanguageSettings.GetLanguageGroup.langTextArr[i].langName = languages[i];
            //LanguageSettings.GetLanguageGroup.langDict[languages[i]] = EditorGUILayout.TextField(LanguageSettings.GetLanguageGroup.langDict[languages[i]]);
            LanguageSettings.GetLanguageGroup.langTextArr[i].langText = EditorGUILayout.TextField(LanguageSettings.GetLanguageGroup.langTextArr[i].langText);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Language Group!", GUILayout.Height(40)))
        {
            SaveLangTexts();
            SaveNewLanguageGroup();
        }


    }


}