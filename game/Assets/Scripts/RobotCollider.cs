using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCollider : MonoBehaviour
{

    public MoveBot m;

    void Start()
    {
        m = transform.parent.GetComponent<MoveBot>();
    }

    private void OnTriggerStay2D(Collider2D collision) // this is where all the magic happens, baby
    { // a LOT of "if" chains...oh dear god
        Transform o = collision.transform;
        Transform p = o;
        if (o.root != o) p = o.parent;
        if (m.pressedAction)
        {
            if (m.pressedMid)
            {
                if (m.cargoID > -1) // has a ball
                {
                    if (o.tag == "centerHitBox" && p.GetComponent<RocketProperties>().cargoOccupied[1] < 0)
                        m.StartAction(GameController.Action.CargoMid, m.medCargoSpeed, p.GetComponent<RocketProperties>().id);
                }
                else if (m.panelID > -1)
                {
                    if (o.tag == "leftHitbox" && p.GetComponent<RocketProperties>().panelOccupied[2] < 0)
                        m.StartAction(GameController.Action.PanelMidLeft, m.medPanelSpeed, p.GetComponent<RocketProperties>().id);
                    if (o.tag == "rightHitBox" && p.GetComponent<RocketProperties>().panelOccupied[3]<0)
                        m.StartAction(GameController.Action.PanelMidRight, m.medPanelSpeed, p.GetComponent<RocketProperties>().id);
                }
                else
                {
                    if (o.tag == "allianceStationLine")
                        m.StartAction(GameController.Action.CollectCargo, 2);
                }
            }
            else if (m.pressedHigh)
            {
                if (m.cargoID > -1)
                {
                    if (o.tag == "centerHitBox" && p.GetComponent<RocketProperties>().cargoOccupied[3] < 0)
                        m.StartAction(GameController.Action.CargoHigh, m.highCargoSpeed, p.GetComponent<RocketProperties>().id);
                }
                else if (m.panelID > -1)
                {
                    if (o.tag == "leftHitbox" && p.GetComponent<RocketProperties>().panelOccupied[4] < 0)
                        m.StartAction(GameController.Action.PanelHighLeft, m.highPanelSpeed, p.GetComponent<RocketProperties>().id);
                    if (o.tag == "rightHitBox" && p.GetComponent<RocketProperties>().panelOccupied[5] < 0)
                        m.StartAction(GameController.Action.PanelHighRight, m.highPanelSpeed, p.GetComponent<RocketProperties>().id);
                }
                else
                {
                    if (o.tag == "allianceStationLine")
                        m.StartAction(GameController.Action.CollectPanel, 2);
                }
            }
            else // no other buttons
            {
                if (m.cargoID > -1)
                {
                    if (o.tag == "centerHitBox" && p.GetComponent<RocketProperties>().cargoOccupied[5] < 0)
                        m.StartAction(GameController.Action.CargoLow, m.lowCargoSpeed, p.GetComponent<RocketProperties>().id);
                    if (o.tag == "cargoBayHitBox" && p.GetComponent<CargoBayProperties>().cargoOccupied < 0)
                        m.StartAction(GameController.Action.CargoInBay, m.lowCargoSpeed, p.GetComponent<CargoBayProperties>().id);
                }
                else if (m.panelID > -1)
                {
                    if (o.tag == "leftHitbox" && p.GetComponent<RocketProperties>().panelOccupied[0] < 0)
                        m.StartAction(GameController.Action.PanelLowLeft, m.lowPanelSpeed, p.GetComponent<RocketProperties>().id);
                    if (o.tag == "rightHitBox" && p.GetComponent<RocketProperties>().panelOccupied[1] < 0)
                        m.StartAction(GameController.Action.PanelLowRight, m.lowPanelSpeed, p.GetComponent<RocketProperties>().id);
                    if (o.tag == "cargoBayHitBox" && p.GetComponent<CargoBayProperties>().panelOccupied < 0)
                        m.StartAction(GameController.Action.PanelInBay, m.lowCargoSpeed, p.GetComponent<CargoBayProperties>().id);
                }
                else
                {
                    if (o.tag == "cargo")
                        m.StartAction(GameController.Action.CollectCargoFromGround, 2, o.GetComponent<PieceProperties>().id);
                }
            }
        }
    }

}
