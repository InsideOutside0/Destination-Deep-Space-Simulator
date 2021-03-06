﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SharpConfig;
using System.IO;
using TMPro;


public class MenuController : MonoBehaviour
{

    // this script is BEEFY so I'm gonna be nice and separate different sections using comments

    const string quickplayConfig = "quickplay.cfg";
    const string defaultConfig = "quickplay.cfg";

    public float z = -0.1f;
    public int mainFontSize = 500;
    public int setupFontSize = 400;
    public Vector3 mainTextPosition = new Vector3(0, 3.5f, -0.1f);
    public Vector3 setupTextPosition = new Vector3(0, 4f, -0.1f);

    private int quickplayMenuNum;
    public Transform button;
    public Transform staticBot;
    public Transform menuInput;
    public Transform canvas;
    public Transform controllerDropdown;

    public List<string> cargoOrHatch;

    // Basic Unity stuff

    void Start()
    {
        quickplayMenuNum = 1;
        if (!File.Exists(defaultConfig)) GenerateDefaultConfig();
        LoadMainMenu();
    }

    void Update()
    {
        OnClick();
    }

    // Config

    void GenerateDefaultConfig() // how we will store all values associated with each bot
    {
        // will be written after first playable build is released
    }

    void GenerateQuickplayConfig() // this will be a severely reduced form of the main config
    {
        File.Create(quickplayConfig);
        Configuration cfg = new Configuration();
        for (int i = 1; i<=6; i++)
        {
            cfg["Robot-" + i]["team-number"].StringValue = "";
            cfg["Robot-" + i]["controller-type"].StringValue = "";
            cfg["Robot-" + i]["robot-item"].StringValue = "";
            cfg["Robot-" + i]["cargo-bay-item"].StringValue = "";
        }
        cfg.SaveToFile(quickplayConfig);
    }

    void SaveQuickplayData()
    {
        Configuration cfg = Configuration.LoadFromFile(quickplayConfig);
        GameObject teamInput = GameObject.FindGameObjectWithTag("menuInput");
        GameObject robotItem = GameObject.FindGameObjectWithTag("robotItem");
        GameObject cargoBayItem = GameObject.FindGameObjectWithTag("cargoBayItem");

        string teamInputText = teamInput.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        string ctrlText = "";
        switch (quickplayMenuNum)
        {
            case 1:
                ctrlText = "WASD";
                break;
            case 2:
                ctrlText = "ArrowKeys";
                break;
            case 3:
                ctrlText = "Joy1";
                break;
            case 4:
                ctrlText = "Joy2";
                break;
            case 5:
                ctrlText = "Joy3";
                break;
            case 6:
                ctrlText = "Joy4";
                break;
            default: break;
        }

        string robotItemText = "";
        switch (robotItem.GetComponent<TMP_Dropdown>().value)
        {
            case 0:
                robotItemText = "cargo";
                break;
            case 1:
                robotItemText = "panel";
                break;
            default: break;
        }
        string cargoBayItemText = "";
        switch (cargoBayItem.GetComponent<TMP_Dropdown>().value)
        {
            case 0:
                cargoBayItemText = "cargo";
                break;
            case 1:
                cargoBayItemText = "panel";
                break;
            default: break;
        }

        cfg["Robot-" + quickplayMenuNum]["team-number"].StringValue = teamInputText;
        cfg["Robot-" + quickplayMenuNum]["controller-type"].StringValue = ctrlText;
        cfg["Robot-" + quickplayMenuNum]["robot-item"].StringValue = robotItemText;
        cfg["Robot-" + quickplayMenuNum]["cargo-bay-item"].StringValue = cargoBayItemText;
        cfg.SaveToFile(quickplayConfig);
    }

    // Manipulating Objects

    Transform CreateButton(string s, Vector3 position) // returns the button in event of manual override
    {
        Transform newButton = Instantiate(button, position, Quaternion.identity /* Indicates no rotation */ );
        newButton.GetComponentInChildren<TextMesh>().text = s;
        newButton.name = s;
        newButton.tag = "button";
        return newButton;
    }

    Transform CreateBot(Vector3 position)
    {
        Transform b = Instantiate(staticBot, position, Quaternion.identity);
        b.tag = "staticBot";
        return b;
    }

    Transform CreateMenuInput(Vector3 position, string titleText, string placeholder, int characterLimit, bool number)
    {
        Transform input = Instantiate(menuInput, position, Quaternion.identity, canvas);
        input.tag = "menuInput";
        input.GetComponent<TMP_InputField>().characterLimit = characterLimit;
        if (number) input.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.IntegerNumber;
        Transform title = input.GetChild(1);
        title.GetComponent<TextMeshProUGUI>().text = titleText;
        return input;
    }

    Transform CreateDropdown(Vector3 position, string name, string tag, string title, List<string> options)
    {
        Transform dropdown = Instantiate(controllerDropdown, position, Quaternion.identity, canvas);
        dropdown.name = name;
        dropdown.tag = tag;
        dropdown.GetComponent<TMP_Dropdown>().AddOptions(options);
        Transform titleText = dropdown.GetChild(dropdown.childCount-1);
        titleText.GetComponent<TextMeshProUGUI>().text = title;
        return dropdown;
    }

    void ChangeText(string s, Vector3 position, int fontSize)
    {
        transform.GetComponent<TextMesh>().text = s;
        transform.GetComponent<TextMesh>().fontSize = fontSize;
        transform.SetPositionAndRotation(position, Quaternion.identity);
    }

    void ClearScreen()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        GameObject[] bots = GameObject.FindGameObjectsWithTag("staticBot");
        GameObject[] inputs = GameObject.FindGameObjectsWithTag("menuInput");
        GameObject[] robotItems = GameObject.FindGameObjectsWithTag("robotItem");
        GameObject[] cargoBayItems = GameObject.FindGameObjectsWithTag("cargoBayItem");
        foreach (GameObject o in buttons) Object.Destroy(o);
        foreach (GameObject o in bots) Object.Destroy(o);
        foreach (GameObject o in inputs) Object.Destroy(o);
        foreach (GameObject o in robotItems) Object.Destroy(o);
        foreach (GameObject o in cargoBayItems) Object.Destroy(o);


    }

    // Loading different screens

    void LoadMainMenu()
    {
        ClearScreen();
        CreateButton("PLAY", new Vector3(0, 1f, z));
        CreateButton("HELP", new Vector3(0, -0.5f, z));
        CreateButton("OPTIONS", new Vector3(0, -2f, z));
        CreateButton("QUIT", new Vector3(0, -3.5f, z));
        ChangeText("Deep Space Sim", mainTextPosition, mainFontSize);
    }

    void LoadHelp()
    {
        ClearScreen();
        CreateButton("BACK", new Vector3(0, -3.5f, z));
        ChangeText("Instructions", mainTextPosition, mainFontSize);
    }

    void LoadOptions()
    {
        ClearScreen();
        CreateButton("WIP", new Vector3(0, 1f, z));
        CreateButton("BACK", new Vector3(0, -3.5f, z));
        ChangeText("Options", mainTextPosition, mainFontSize);
    }

    void LoadPlay()
    {
        ClearScreen();
        CreateButton("QUICKPLAY", new Vector3(0, 1f, z));
        CreateButton("GAME SETUP", new Vector3(0, -1.25f, z));
        CreateButton("BACK", new Vector3(0, -3.5f, z));
        ChangeText("Play", mainTextPosition, mainFontSize);
    }

    void LoadManualSetup()
    {
        ClearScreen();
        ChangeText("Manual Setup", setupTextPosition, setupFontSize);
        Transform x = CreateButton("BACK", new Vector3(0, -3.5f, z));
        x.name = "BACK-PLAY"; // this is why the function returns the button
    }

    void LoadQuickplaySetup()
    {
        if (!File.Exists(quickplayConfig)) GenerateQuickplayConfig();
        Configuration cfg = Configuration.LoadFromFile(quickplayConfig);
        ClearScreen();
        ChangeText("Quick Setup", setupTextPosition, setupFontSize);

        Transform b = CreateButton("BACK", new Vector3(0, -3.5f, z));
        if (quickplayMenuNum==1) b.name = "BACK-PLAY";
        else b.name = "BACK-QUICK";

        Transform n = CreateButton("NEXT", new Vector3(0, -2f, z));
        if (quickplayMenuNum == 6) n.name = "TO-LEVEL-QUICK";
        else n.name = "NEXT-QUICK";

        Transform input = CreateMenuInput(new Vector3(0, -.5f, 0),
            "Robot " + quickplayMenuNum + " Team Number", "Enter number", 4, true);
        input.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text =
            cfg["Robot-" + quickplayMenuNum]["team-number"].StringValue;
        input.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.zero;

        Transform robotItem = CreateDropdown(new Vector3(-5, 1.6f, 0), "robotItem", 
            "robotItem", "Robot's item", cargoOrHatch);
        Transform cargoBayItem = CreateDropdown(new Vector3(5, 1.6f, 0), "cargoBayItem", "cargoBayItem", 
            "Cargo Bay's item", cargoOrHatch);
    }

    // Update events

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0)) // if left click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            // dost thou light that burneth the sky shineth on mine button?
            if (hit.collider != null) // if you clicked something with a collider
            {
                switch (hit.transform.name)
                {
                    // Main Menu
                    case "PLAY":
                        LoadPlay();
                        break;
                    case "HELP":
                        LoadHelp();
                        break;
                    case "OPTIONS":
                        LoadOptions();
                        break;
                    case "QUIT":
                        // var x = 1/0; haha nice
                        Application.Quit();
                        break;

                    // Back buttons
                    case "BACK":
                        LoadMainMenu();
                        break;
                    case "BACK-PLAY":
                        if (quickplayMenuNum>0) SaveQuickplayData();
                        quickplayMenuNum = 0;
                        LoadPlay();
                        break;

                    // Play menu
                    case "QUICKPLAY":
                        GlobalVariables.quickplay = true;
                        quickplayMenuNum = 1;
                        LoadQuickplaySetup();
                        break;
                    case "GAME SETUP":
                        LoadManualSetup();
                        break;

                    // Quick Setup menu
                    case "NEXT-QUICK":
                        SaveQuickplayData();
                        quickplayMenuNum++;
                        LoadQuickplaySetup();
                        break;
                    case "TO-LEVEL-QUICK":
                        SaveQuickplayData();
                        SceneManager.LoadScene("Level");
                        break;
                    case "BACK-QUICK":
                        SaveQuickplayData();
                        quickplayMenuNum--;
                        LoadQuickplaySetup();
                        break;

                    default: break;
                }

            }
        }
    }


}
