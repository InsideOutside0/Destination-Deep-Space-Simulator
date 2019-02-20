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

    private int redScore;
    private int blueScore;

    public AudioSource musicSource;

    public Transform botTempalte;
    public Transform rocket;
    public Transform cargoBay;

    public Transform redCargoShip;
    public Transform blueCargoShip;

    public Transform canvas;
    public Transform timeLeft;
    public Transform redScorekeeper;
    public Transform blueScorekeeper;

    public Vector2[] botPositions = // determined in Unity
    {
        new Vector3(), // red 1
        new Vector3(), // red 2
        new Vector3(), // red 3
        new Vector3(), // blue 1
        new Vector3(), // blue 2
        new Vector3(), // blue 3
    };

    public Vector3[] rocketPositions =
    {
        new Vector3(), // bottom red
        new Vector3(), // top red
        new Vector3(), // bottom blue
        new Vector3(), // top blue
    };

    public Vector3[] redCargoBayPositions =
    {
        new Vector3(), // face down
        new Vector3(),
        new Vector3(),
        new Vector3(), // face up
        new Vector3(),
        new Vector3(),
        new Vector3(), // face left
        new Vector3(),
    };

    public Vector3[] blueCargoBayPositions =
    {
        new Vector3(), // face down
        new Vector3(),
        new Vector3(),
        new Vector3(), // face up
        new Vector3(),
        new Vector3(),
        new Vector3(), // face right
        new Vector3(),
    };

    public Transform[] bots;
    public Transform[] rockets;
    public Transform[] redCargoBays;
    public Transform[] blueCargoBays;

    void Start()
    {
        musicSource.Play();
        PlaceGamePieces();
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
        if (gameTime >= 0)
            timeLeft.GetComponent<TextMeshProUGUI>().text = ReturnTime((int)gameTime);
        if (gameTime <= 0) ToggleEngines();
        redScorekeeper.GetComponent<TextMeshProUGUI>().text = "" + redScore;
        blueScorekeeper.GetComponent<TextMeshProUGUI>().text = "" + blueScore;
    }

    void UpdateScore(int id, int s)
    {
        if (id<3) redScore += s;
        else blueScore += s;
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
            if (bots[i].GetComponent<MoveBot>().started == 0)
                bots[i].GetComponent<MoveBot>().started = 1;
            else bots[i].GetComponent<MoveBot>().started = 0;
        }
    }

    void PlaceGamePieces()
    {
        rockets = new Transform[4];
        for (int i = 0; i<4; i++)
        {
            rockets[i] = Instantiate(rocket, rocketPositions[i], Quaternion.identity);
            if (i > 1)
            {
                for (int j = 0; j<5; j++)
                {
                    rockets[i].GetChild(j).GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
                }
            }
            if (i % 2 == 1) rockets[i].transform.Rotate(Vector3.forward * 180);
        }

        redCargoBays = new Transform[8];
        blueCargoBays = new Transform[8];
        for (int i = 0; i < 8; i++)
        {
            blueCargoBays[i] = Instantiate(cargoBay, blueCargoBayPositions[i], Quaternion.identity, blueCargoShip);
            redCargoBays[i] = Instantiate(cargoBay, redCargoBayPositions[i], Quaternion.identity, redCargoShip);
            if (i % 2 == 0)
            {
                redCargoBays[i].GetComponent<SpriteRenderer>().color = new Color(210 ,210 ,210);
                blueCargoBays[i].GetComponent<SpriteRenderer>().color = new Color(210, 210, 210);
            }
            if (i > 5)
            {
                redCargoBays[i].transform.Rotate(Vector3.forward * 90);
                blueCargoBays[i].transform.Rotate(Vector3.forward * 90);
            }
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
            bots[i].GetComponent<MoveBot>().id = i;
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

                bots[i].GetComponent<RobotActions>().lowCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().medCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().highCargoAcc = 1;
                bots[i].GetComponent<RobotActions>().lowPanelAcc = 1;
                bots[i].GetComponent<RobotActions>().medPanelAcc = 1;
                bots[i].GetComponent<RobotActions>().highPanelAcc = 1;

                bots[i].GetComponent<RobotActions>().lowCargoSpeed = 1; // in seconds
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
