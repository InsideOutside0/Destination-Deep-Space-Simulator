using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;

public class MenuController : MonoBehaviour
{

    // script is big so I'll notate different sections

    [SerializeField] float z = -0.1f;
    [SerializeField] int mainFontSize = 500;
    [SerializeField] int setupFontSize = 400;
    [SerializeField] Vector3 mainTextPosition = new Vector3(0, 3.5f, -0.1f);
    [SerializeField] Vector3 setupTextPosition = new Vector3(0, 4f, -0.1f);
    [SerializeField] Vector3 botPosition = new Vector3(-3, 0, -0.1f);

    public Transform button;
    public Transform staticBot;

    // Unity Stuff

    void Start()
    {
        LoadMainMenu();
    }

    void Update()
    {
        OnClick();
    }

    // Object Modification

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

    // Load different screens

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

    void LoadSetup()
    {
        ClearScreen();
        ChangeText("Game Setup", setupTextPosition, setupFontSize);
        Transform x = CreateButton("BACK", new Vector3(0, -3.5f, z));
        x.name = "BACK-PLAY"; // this is why the function returns the button
        CreateBot(botPosition);
    }

    void LoadQuickplaySetup()
    {
        ClearScreen();
        ChangeText("Quickplay Setup", setupTextPosition, setupFontSize);
        Transform x = CreateButton("BACK", new Vector3(0, -3.5f, z));
        x.name = "BACK-PLAY";
    }

    // handle button clicks

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0)) // if left click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            // dost thou light that burneth the sky shineth on mine button?
            if (hit.collider != null)
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
                        LoadQuickplaySetup();
                        break;
                    case "GAME SETUP":
                        LoadSetup();
                        break;
                    
                    default: break;
                }

            }
        }
    }


}
