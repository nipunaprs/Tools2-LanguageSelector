using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    static LanguageData languageData;

    //Methods to return
    public static LanguageData LanguageInfo { get { return languageData;  } }


    //Menu Item will be under Window
    [MenuItem("Window/Language Selector")]
    static void OpenWindow()
    {
        LanguageSelectorWindow window = (LanguageSelectorWindow)GetWindow(typeof(LanguageSelectorWindow));
        window.minSize = new Vector2(600, 100);
        window.Show();
    }


    //Similar to start function in reg code
    public void OnEnable()
    {
        InitData();
        InitTextures();


    }

    public static void InitData()
    {
        //Cast scriptable object created
        languageData = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
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
        GUILayout.Label("Please enter a key");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Key: ");
        languageData.key = EditorGUILayout.TextField(languageData.key);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("No Of Languages: ");
        languageData.noOfLanguages = EditorGUILayout.Slider(languageData.noOfLanguages,1.0f,5.0f);
        languageData.noOfLanguages = Mathf.Round(languageData.noOfLanguages);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Create Key!",GUILayout.Height(40)))
        {
            LanguageSettings.OpenWindow(languageData.key, languageData.noOfLanguages);
        }


        GUILayout.EndArea();

    }

}


public class LanguageSettings : EditorWindow
{
    static string key;
    static int ammount;
    static LanguageSettings window;
    LanguageDetails[] detailsArr;

    public static void OpenWindow(string selectedkey,float number)
    {
        key = selectedkey;
        ammount = (int) number;
        window = (LanguageSettings)GetWindow(typeof(LanguageSettings));
        window.minSize = new Vector2(800, 400);
        window.Show();
    }


    void InitData()
    {
        detailsArr = new LanguageDetails[ammount];

        for (int x = 0; x < ammount; x++)
        {
            detailsArr[x].langName = "null";
            detailsArr[x].langText = "null";

        }
    }

    public void OnEnable()
    {
        InitData();
    }

    private void OnGUI()
    {
        DrawSettings();
    }

    void DrawSettings()
    {
        //Array of language details based on the ammount requested
        string langName1 = "";
        string langText1 = "";

        if (ammount > 0 )
        {
            for (int x = 0; x < ammount; x++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("L "+x.ToString());
                detailsArr[x].langText = EditorGUILayout.TextField(langName1);
                detailsArr[x].langText = EditorGUILayout.TextField(langText1);// GUILayout.MinWidth(80f)
                GUILayout.EndHorizontal();
            }

            

           
        }

        

       

        /*
        if (ammount > 0)
        {
            

            



            
            //For loop to generate enough textboxes for key
            for (int x = 0; x < ammount; x++)
            {
                LanguageDetails currentLang = detailsArr[ammount];

                GUILayout.BeginHorizontal();
                GUILayout.Label("L " + (ammount + 1));
                currentLang.langName = EditorGUILayout.TextField(currentLang.langName);
                currentLang.langText = EditorGUILayout.TextField(currentLang.langText);// GUILayout.MinWidth(80f)
                GUILayout.EndHorizontal();
            }
        }*/

        



    }


}