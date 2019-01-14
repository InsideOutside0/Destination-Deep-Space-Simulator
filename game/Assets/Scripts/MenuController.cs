using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public Transform button;

    // Start is called before the first frame update
    void Start()
    {
        loadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createButton(string s, Vector3 position)
    {
        Transform newButton = Instantiate(button, position, Quaternion.identity);
        newButton.GetComponentInChildren<TextMesh>().text = s;
    }

    void loadMainMenu()
    {
        createButton("PLAY", new Vector3(0, 0.5f, -0.1f));
        createButton("HELP", new Vector3(0, -1.5f, -0.1f));
        createButton("QUIT", new Vector3(0, -3.5f, -0.1f));
    }
}
