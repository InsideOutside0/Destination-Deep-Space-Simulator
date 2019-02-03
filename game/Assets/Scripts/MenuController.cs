using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;
using System.IO;


public class MenuController : MonoBehaviour
{

    // this script is BEEFY so I'm gonna be nice and separate different sections using comments

    [SerializeField] float z = -0.1f;
    [SerializeField] int mainFontSize = 500;
    [SerializeField] int setupFontSize = 400;
    [SerializeField] Vector3 mainTextPosition = new Vector3(0, 3.5f, -0.1f);
    [SerializeField] Vector3 setupTextPosition = new Vector3(0, 4f, -0.1f);
    [SerializeField] Vector3 botPosition = new Vector3(-3, 0, -0.1f);

    public Transform button;
    public Transform staticBot;

    // Basic Unity stuff


    void Start()
    {
        if (!File.Exists("config.cfg")) GenerateDefaultConfig();
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
        Configuration cfg = new Configuration();
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
        foreach (GameObject b in buttons) Object.Destroy(b);
        foreach (GameObject b in bots) Object.Destroy(b);
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
        CreateBot(botPosition);
    }

    void LoadQuickplaySetup()
    {
        ClearScreen();
        ChangeText("Quick Setup", setupTextPosition, setupFontSize);
        Transform x = CreateButton("BACK", new Vector3(0, -3.5f, z));
        x.name = "BACK-PLAY"; // this is why the function returns the button
        if (!File.Exists("quickplay.cfg")) GenerateQuickplayConfig();
    }

    // Button detection

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
                        // var x = 1/0;
                        Application.Quit();
                        break;

                    // Back buttons
                    case "BACK":
                        LoadMainMenu();
                        break;
                    case "BACK-PLAY":
                        LoadPlay();
                        break;

                    // Play menu
                    case "QUICKPLAY":
                        GlobalVariables.quickplay = true;
                        LoadQuickplaySetup();
                        break;
                    case "GAME SETUP":
                        LoadManualSetup();
                        break;
                    
                    default: break;
                }

            }
        }
    }


}
