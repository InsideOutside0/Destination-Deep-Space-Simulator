using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] float z = -0.1f;
    public Transform button;

    // Start is called before the first frame update
    void Start()
    {
        LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        OnClick();
    }

    void CreateButton(string s, Vector3 position)
    {
        Transform newButton = Instantiate(button, position, Quaternion.identity /* Indicates no rotation */ );
        newButton.GetComponentInChildren<TextMesh>().text = s;
        newButton.name = s;
        newButton.tag = "button";
    }

    void RemoveAllButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject b in buttons)
        {
            Object.Destroy(b);
        }
    }

    void LoadMainMenu()
    {
        RemoveAllButtons();
        CreateButton("PLAY", new Vector3(0, 1f, z));
        CreateButton("HELP", new Vector3(0, -0.5f, z));
        CreateButton("OPTIONS", new Vector3(0, -2f, z));
        CreateButton("QUIT", new Vector3(0, -3.5f, z));
    }

    void LoadHelp()
    {
        RemoveAllButtons();
        CreateButton("BACK", new Vector3(0, -3.5f, z));
    }

    void LoadOptions()
    {
        RemoveAllButtons();
        CreateButton("WIP", new Vector3(0, 1f, z));
        CreateButton("BACK", new Vector3(0, -3.5f, z));
    }

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
                    case "PLAY":
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
                    case "BACK":
                        LoadMainMenu();
                        break;
                    default: break;
                }

            }
        }
    }
}
