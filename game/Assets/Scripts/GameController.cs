﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum Action { None, CollectCargo, CollectCargoFromGround, CollectPanel, CargoLow, CargoMid, CargoHigh,
        PanelLowLeft, PanelLowRight, PanelMidLeft, PanelMidRight,
        PanelHighLeft, PanelHighRight, CargoInBay, PanelInBay };

    const string quickplayConfig = "quickplay.cfg";
    const string defaultConfig = "quickplay.cfg";

    private float warmupTime = 8;
    private float gameTime = 150;
    private float endDelay = 2;
    private bool gameStarted;

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
    public Transform allianceStationLine;

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

    public Vector3[] allianceStationLinePositions =
    {
        new Vector3(), new Vector3(), // red
        new Vector3(), new Vector3(), // blue
    };

    public Transform[] bots;
    public Transform[] rockets;
    public Transform[] redCargos;
    public Transform[] blueCargos;
    public Transform[] redPanels;
    public Transform[] bluePanels;
    public Transform[] redCargoBays;
    public Transform[] blueCargoBays;
    public Transform[] allianceStationLines;

    void Start()
    {
        musicSource.Play();
        PlaceGamePieces();
        PlaceBots();
        warmupTime -= Time.deltaTime;
        habsHaveColliders = false;
        GlobalVariables.quickplay = true; // temporary, for debug purposes EDIT: probably not temporary anymore lmao
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
            redPanels[i] = Instantiate(panel, offscreen, Quaternion.identity);
            redPanels[i].GetComponent<PieceProperties>().id = i;
            redPanels[i].GetComponent<PieceProperties>().active = false;
            bluePanels[i] = Instantiate(panel, offscreen, Quaternion.identity);
            bluePanels[i].GetComponent<PieceProperties>().id = i;
            bluePanels[i].GetComponent<PieceProperties>().active = false;
        }

        allianceStationLines = new Transform[4];
        for (int i = 0; i < 4; i++)
            allianceStationLines[i] = Instantiate(allianceStationLine, allianceStationLinePositions[i], Quaternion.identity);

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

            if (GlobalVariables.quickplay) // let's be real, manual setup is not gonna happen :/
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

    public void LoadAction(Action action, int robotId)
    {
        if (action == Action.CollectCargo)
        {
            for (int i = 0; i<24; i++)
            {
                if (robotId < 3) if (!redCargos[i].GetComponent<PieceProperties>().active)
                    {
                        redCargos[i].GetComponent<PieceProperties>().active = true;
                        redCargos[i].GetComponent<PieceProperties>().onRobot = true;
                        redCargos[i].GetComponent<CircleCollider2D>().enabled = false;
                        redCargos[i].GetComponent<BoxCollider2D>().enabled = false;
                        redCargos[i].transform.SetPositionAndRotation(bots[robotId].transform.position, Quaternion.identity);
                        redCargos[i].parent = bots[robotId];
                        bots[robotId].GetComponent<MoveBot>().cargoID = i;
                        break;
                    }
                if (!blueCargos[i].GetComponent<PieceProperties>().active)
                {
                    blueCargos[i].GetComponent<PieceProperties>().active = true;
                    blueCargos[i].GetComponent<PieceProperties>().onRobot = true;
                    blueCargos[i].GetComponent<CircleCollider2D>().enabled = false;
                    blueCargos[i].GetComponent<BoxCollider2D>().enabled = false;
                    blueCargos[i].transform.SetPositionAndRotation(bots[robotId].transform.position, Quaternion.identity);
                    blueCargos[i].parent = bots[robotId];
                    bots[robotId].GetComponent<MoveBot>().cargoID = i;
                    break;
                }
            }
        } else if (action == Action.CollectPanel)
        {
            for (int i = 0; i < 24; i++)
            {
                if (robotId < 3) if (!redPanels[i].GetComponent<PieceProperties>().active)
                    {
                        redPanels[i].GetComponent<PieceProperties>().active = true;
                        redPanels[i].GetComponent<PieceProperties>().onRobot = true;
                        redPanels[i].GetComponent<CircleCollider2D>().enabled = false;
                        redPanels[i].transform.SetPositionAndRotation(bots[robotId].transform.position, Quaternion.identity);
                        redPanels[i].parent = bots[robotId];
                        bots[robotId].GetComponent<MoveBot>().panelID = i;
                        break;
                    }
                if (!bluePanels[i].GetComponent<PieceProperties>().active)
                {
                    bluePanels[i].GetComponent<PieceProperties>().active = true;
                    bluePanels[i].GetComponent<PieceProperties>().onRobot = true;
                    bluePanels[i].GetComponent<CircleCollider2D>().enabled = false;
                    bluePanels[i].transform.SetPositionAndRotation(bots[robotId].transform.position, Quaternion.identity);
                    bluePanels[i].parent = bots[robotId];
                    bots[robotId].GetComponent<MoveBot>().panelID = i;
                    break;
                }
            }
        }
    }

    public void LoadAction(Action action, int robotId, int objID)
    {
        Transform bot = bots[robotId];
        Transform c = bot.GetChild(bot.childCount - 1);
        bool isRed = (robotId < 3);
        switch (action)
        {
            case Action.CargoLow:
                RocketProperties r1 = rockets[objID].GetComponent<RocketProperties>();
                if (r1.cargoOccupied[0] > -1) r1.cargoOccupied[1] = bot.GetComponent<MoveBot>().otherID;
                else r1.cargoOccupied[0] = bot.GetComponent<MoveBot>().otherID;
                c.parent = null; // this works I guess
                c.transform.position = offscreen;
                UpdateScore(robotId, 3);
                bot.GetComponent<MoveBot>().cargoID = -1;
                break;
            case Action.CargoMid:
                RocketProperties r2 = rockets[objID].GetComponent<RocketProperties>();
                if (r2.cargoOccupied[2] > -1) r2.cargoOccupied[3] = bot.GetComponent<MoveBot>().otherID;
                else r2.cargoOccupied[2] = bot.GetComponent<MoveBot>().otherID;
                c.parent = null;
                c.transform.position = offscreen;
                UpdateScore(robotId, 3);
                bot.GetComponent<MoveBot>().cargoID = -1;
                break;
            case Action.CargoHigh:
                RocketProperties r3 = rockets[objID].GetComponent<RocketProperties>();
                if (r3.cargoOccupied[4] > -1) r3.cargoOccupied[5] = bot.GetComponent<MoveBot>().otherID;
                else r3.cargoOccupied[4] = bot.GetComponent<MoveBot>().otherID;
                c.parent = null;
                c.transform.position = offscreen;
                UpdateScore(robotId, 3);
                bot.GetComponent<MoveBot>().cargoID = -1;
                break;
            case Action.CargoInBay:
                CargoBayProperties c1;
                if (isRed) c1 = redCargoBays[objID].GetComponent<CargoBayProperties>();
                else c1 = blueCargoBays[objID].GetComponent<CargoBayProperties>();
                c1.cargoOccupied = bot.GetComponent<MoveBot>().otherID;
                c.transform.parent = null;
                c.transform.position = c1.transform.position; // that's confusing, will fix later
                UpdateScore(robotId, 3);
                bot.GetComponent<MoveBot>().cargoID = -1;
                break;
            case Action.PanelLowLeft:
                break;
            case Action.PanelLowRight:
                break;
            case Action.PanelMidLeft:
                break;
            case Action.PanelMidRight:
                break;
            case Action.PanelHighLeft:
                break;
            case Action.PanelHighRight:
                break;
            case Action.PanelInBay:
                break;
            case Action.CollectCargoFromGround:
                Transform cargo;
                if (isRed) cargo = redCargos[objID]; // these two lines can cause problems if collecting wrong team's cargo
                else cargo = blueCargos[objID]; // no, it's not worth fixing
                cargo.GetComponent<PieceProperties>().onRobot = true;
                cargo.GetComponent<CircleCollider2D>().enabled = false;
                cargo.GetComponent<BoxCollider2D>().enabled = false;
                cargo.transform.SetPositionAndRotation(bots[robotId].transform.position, Quaternion.identity);
                cargo.parent = bots[robotId];
                bots[robotId].GetComponent<MoveBot>().cargoID = objID;
                break;

            default: break;
        }
        bot.GetComponent<MoveBot>().otherID = -1;
        bot.GetComponent<MoveBot>().panelID = -1;
    }
}
