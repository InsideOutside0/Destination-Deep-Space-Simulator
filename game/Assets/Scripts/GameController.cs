using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;
using TMPro;

public class GameController : MonoBehaviour
{
    const string quickplayConfig = "quickplay.cfg";
    const string defaultConfig = "quickplay.cfg";

    private float warmupTime = 8;
    private float gameTime = 150;
    private float endDelay = 2;
    private bool gameStarted;

    public AudioSource musicSource;

    public Transform botTempalte;
    public Transform canvas;
    public Transform timeLeft;

    public Vector2[] botPositions = // determined in Unity
    {
        new Vector3(),
        new Vector3(),
        new Vector3(),
        new Vector3(),
        new Vector3(),
        new Vector3(),
    };

    public Transform[] bots;

    void Start()
    {
        musicSource.Play();
        PlaceBots();
        warmupTime -= Time.deltaTime;
    }

    void Update()
    {
        if (warmupTime <= 0)
        {
            gameStarted = true;
            ToggleEngines();
        }

        if (!gameStarted) warmupTime -= Time.deltaTime;
        else gameTime -= Time.deltaTime;
        if (gameTime >= 0) timeLeft.GetComponent<TextMeshProUGUI>().text = ReturnTime((int)gameTime);
        if (gameTime <= 0)
        {
            ToggleEngines();
        }
    }

    string ReturnTime(int t)
    {
        string m = (t / 60).ToString();
        string s = (t % 60).ToString();
        if (t % 60 < 10) s = "0" + s;
        return m + ":" + s;
    }

    void ToggleEngines()
    {
        for (int i = 0; i < 6; i++)
        {
            if(bots[i].GetComponent<MoveBot>().started == 0)
                bots[i].GetComponent<MoveBot>().started = 1;
            else bots[i].GetComponent<MoveBot>().started = 0;
        }
    }

    void PlaceBots()
    {
        Configuration cfg = Configuration.LoadFromFile(quickplayConfig);
        bots = new Transform[6];
        for (int i = 0; i < 6; i++)
        {
            int n = i + 1;
            bots[i] = Instantiate(botTempalte, botPositions[i], Quaternion.identity);
            if (i < 3) // Red Alliance
            {
                bots[i].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                bots[i].GetChild(0).GetComponent<SpriteRenderer>().color = new Color(144, 0, 255);
            }
            else // Blue Alliance
            {
                bots[i].transform.Rotate(Vector3.forward * 180);
                bots[i].GetChild(1).transform.Rotate(Vector3.forward * 180);
            }
            bots[i].GetChild(1).GetComponent<TextMeshPro>().text = cfg["Robot-" + n]["team-number"].StringValue;
            bots[i].GetComponent<MoveBot>().controllerName = cfg["Robot-" + n]["controller-type"].StringValue;

            if (GlobalVariables.quickplay)
            {
                bots[i].GetComponent<RobotActions>().cargoLevel = 3;
                bots[i].GetComponent<RobotActions>().panelLevel = 3;
                bots[i].GetComponent<RobotActions>().hasVision = true;
                bots[i].GetComponent<RobotActions>().hasRamp = false;
                bots[i].GetComponent<RobotActions>().hasSideAuto = false;
                bots[i].GetComponent<RobotActions>().hasCenterAuto = false;

                bots[i].GetComponent<RobotActions>().lowCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().medCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().highCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().lowPanelAcc = 1;
                bots[i].GetComponent<RobotActions>().medPanelAcc = 1;
                bots[i].GetComponent<RobotActions>().highPanelAcc = 1;
                bots[i].GetComponent<RobotActions>().sideAutoAccMod = 1;
                bots[i].GetComponent<RobotActions>().centerAutoAccMod = 1;

                bots[i].GetComponent<RobotActions>().lowCargoSpeed = 1;
                bots[i].GetComponent<RobotActions>().medCargoSpeed = 2;
                bots[i].GetComponent<RobotActions>().highCargoSpeed = 3;
                bots[i].GetComponent<RobotActions>().lowPanelSpeed = 1;
                bots[i].GetComponent<RobotActions>().medPanelSpeed = 2;
                bots[i].GetComponent<RobotActions>().highPanelSpeed = 3;
                bots[i].GetComponent<RobotActions>().sideAutoSpeedMod = 1;
                bots[i].GetComponent<RobotActions>().centerAutoSpeedMod = 1;
            }
        }
    }

}
