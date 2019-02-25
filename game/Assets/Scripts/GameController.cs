using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum Action { None, CollectCargo, CollectPanel, CargoLow, CargoMid, CargoHigh,
        PanelLow, PanelMid, PanelHigh, CargoInBay, PanelInBay };

    const string quickplayConfig = "quickplay.cfg";
    const string defaultConfig = "quickplay.cfg";

    private float warmupTime = 8;
    private float gameTime = 150;
    private float endDelay = 2;
    private bool gameStarted;

    private int redRemainingCargo = 24;
    private int blueRemainingCargo = 24;
    private int redRemainingPanels = 24;
    private int blueRemainingPanels = 24;
    public Vector3 offscreen = new Vector3(50, 50, 0);

    public AudioSource musicSource;

    public Transform botTempalte;
    public Transform rocket;
    public Transform cargoBay;
    public Transform redCargoShip;
    public Transform blueCargoShip;
    public Transform cargo;
    public Transform panel;

    public Transform redBottomLowHab;
    public Transform redTopLowHab;
    public Transform redHighHab;
    public Transform blueBottomLowHab;
    public Transform blueTopLowHab;
    public Transform blueHighHab;
    public bool habsHaveColliders;

    public Transform canvas;
    public Transform timeLeft;
    public Transform redScorekeeper;
    public Transform blueScorekeeper;
    public Transform redCargoLeft;
    public Transform blueCargoLeft;
    public Transform redPanelsLeft;
    public Transform bluePanelsLeft;

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
         // face downn
        new Vector3(), new Vector3(), new Vector3(),
        // face up
        new Vector3(), new Vector3(), new Vector3(),
        // face left
        new Vector3(), new Vector3(),
    };

    public Vector3[] blueCargoBayPositions =
    {
         // face down
        new Vector3(), new Vector3(), new Vector3(),
         // face up
        new Vector3(), new Vector3(), new Vector3(),
         // face right
        new Vector3(), new Vector3(),
    };

    public Vector3[] redCargoPositions =
    {
         // bottom half
        new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(),
         // top half
        new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(),
    };

    public Vector3[] blueCargoPositions =
    {
         // bottom half
        new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(),
         // top half
        new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(), new Vector3(),
    };

    public Transform[] bots;
    public Transform[] rockets;
    public Transform[] redCargos;
    public Transform[] blueCargos;
    public Transform[] redPanels;
    public Transform[] bluePanels;
    public Transform[] redCargoBays;
    public Transform[] blueCargoBays;

    void Start()
    {
        musicSource.Play();
        PlaceGamePieces();
        PlaceBots();
        warmupTime -= Time.deltaTime;
        habsHaveColliders = false;
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
        if (gameTime < 147 && !habsHaveColliders)
        {
            redTopLowHab.gameObject.AddComponent<BoxCollider2D>();
            redBottomLowHab.gameObject.AddComponent<BoxCollider2D>();
            redHighHab.gameObject.AddComponent<BoxCollider2D>();
            blueBottomLowHab.gameObject.AddComponent<BoxCollider2D>();
            blueTopLowHab.gameObject.AddComponent<BoxCollider2D>();
            blueHighHab.gameObject.AddComponent<BoxCollider2D>();
            habsHaveColliders = true;
        }
        redScorekeeper.GetComponent<TextMeshProUGUI>().text = "" + GlobalVariables.redScore;
        blueScorekeeper.GetComponent<TextMeshProUGUI>().text = "" + GlobalVariables.blueScore;
        redCargoLeft.GetComponent<TextMeshProUGUI>().text = RemainingPieces("red", "cargo");
        blueCargoLeft.GetComponent<TextMeshProUGUI>().text = RemainingPieces("blue", "cargo");
        redPanelsLeft.GetComponent<TextMeshProUGUI>().text = RemainingPieces("red", "panel");
        bluePanelsLeft.GetComponent<TextMeshProUGUI>().text = RemainingPieces("blue", "panel");
    }

    void UpdateScore(int id, int s)
    {
        if (id<3) GlobalVariables.redScore += s;
        else GlobalVariables.blueScore += s;
    }

    string RemainingPieces(string team, string type)
    {
        int sum = 0;
        if (type=="cargo")
        {
            if (team == "red")
            {
                foreach (Transform cargo in redCargos)
                {
                    if (!cargo.GetComponent<PieceProperties>().active) sum++;
                }
            }
            if (team == "blue")
            {
                foreach (Transform cargo in blueCargos)
                {
                    if (!cargo.GetComponent<PieceProperties>().active) sum++;
                }
            }
        }
        if (type == "panel")
        {
            if (team == "red")
            {
                foreach (Transform panel in redPanels)
                {
                    if (!panel.GetComponent<PieceProperties>().active) sum++;
                }
            }
            if (team == "blue")
            {
                foreach (Transform panel in bluePanels)
                {
                    if (!panel.GetComponent<PieceProperties>().active) sum++;
                }
            }
        }
        return "" + sum;
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

        redCargos = new Transform[24];
        blueCargos = new Transform[24];
        redPanels = new Transform[24];
        bluePanels = new Transform[24];
        for (int i = 0; i<24; i++)
        {
            if (i < 12)
            {
                redCargos[i] = Instantiate(cargo, redCargoPositions[i], Quaternion.identity);
                redCargos[i].GetComponent<PieceProperties>().id = i;
                redCargos[i].GetComponent<PieceProperties>().active = true;
                blueCargos[i] = Instantiate(cargo, blueCargoPositions[i], Quaternion.identity);
                blueCargos[i].GetComponent<PieceProperties>().id = i;
                blueCargos[i].GetComponent<PieceProperties>().active = true;
            } else
            {
                redCargos[i] = Instantiate(cargo, offscreen, Quaternion.identity);
                redCargos[i].GetComponent<PieceProperties>().id = i;
                redCargos[i].GetComponent<PieceProperties>().active = false;
                blueCargos[i] = Instantiate(cargo, offscreen, Quaternion.identity);
                blueCargos[i].GetComponent<PieceProperties>().id = i;
                blueCargos[i].GetComponent<PieceProperties>().active = false;
            }
            redPanels[i] = Instantiate(cargo, offscreen, Quaternion.identity);
            redPanels[i].GetComponent<PieceProperties>().id = i;
            redPanels[i].GetComponent<PieceProperties>().active = false;
            bluePanels[i] = Instantiate(cargo, offscreen, Quaternion.identity);
            bluePanels[i].GetComponent<PieceProperties>().id = i;
            bluePanels[i].GetComponent<PieceProperties>().active = false;
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
                bots[i].GetComponent<MoveBot>().cargoLevel = 3;
                bots[i].GetComponent<MoveBot>().panelLevel = 3;
                bots[i].GetComponent<MoveBot>().hasVision = true;
                bots[i].GetComponent<MoveBot>().hasRamp = false;

                bots[i].GetComponent<MoveBot>().lowCargoAcc = 1;
                bots[i].GetComponent<MoveBot>().medCargoAcc = 1;
                bots[i].GetComponent<MoveBot>().highCargoAcc = 1;
                bots[i].GetComponent<MoveBot>().lowPanelAcc = 1;
                bots[i].GetComponent<MoveBot>().medPanelAcc = 1;
                bots[i].GetComponent<MoveBot>().highPanelAcc = 1;

                bots[i].GetComponent<MoveBot>().lowCargoSpeed = 1; // in seconds
                bots[i].GetComponent<MoveBot>().medCargoSpeed = 2;
                bots[i].GetComponent<MoveBot>().highCargoSpeed = 3;
                bots[i].GetComponent<MoveBot>().lowPanelSpeed = 1;
                bots[i].GetComponent<MoveBot>().medPanelSpeed = 2;
                bots[i].GetComponent<MoveBot>().highPanelSpeed = 3;
                bots[i].GetComponent<MoveBot>().sideAutoSpeedMod = 1;
                bots[i].GetComponent<MoveBot>().centerAutoSpeedMod = 1;
            }
        }
    }

    void LoadAction(Action action, int robotId)
    {

    }

    void LoadAction(Action action, int robotId, int structID)
    {

    }
}
