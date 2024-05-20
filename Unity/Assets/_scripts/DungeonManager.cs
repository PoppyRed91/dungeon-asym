using System;
using System.Collections;
using DungeonArchitect;

using DungeonArchitect.Frameworks.Snap;

using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Dungeon Dungeon;
    public string DungeonCode;

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        if (Dungeon != null)
        {
            DungeonCode = String.Empty;
            if (Dungeon.IsLayoutBuilt) Dungeon.DestroyDungeon();
            Dungeon.Config.Seed = (uint)(UnityEngine.Random.value * int.MaxValue);
            Dungeon.Build();
            var modules = Dungeon.GetComponent<PooledDungeonSceneProvider>().itemParent.transform;
            foreach (Transform module in modules)
            {
                if (module.name == "Player(Clone)") return;
                var identifier = module.GetComponentInChildren<RoomIdentifier>().transform;
                string door = String.Empty;
                foreach (Transform connector in module)
                {
                    if (connector.GetComponent<SnapConnection>() != null)
                    {
                        var connectionState = connector.GetComponent<SnapConnection>().connectionState;

                        if (connectionState == SnapConnectionState.Door)
                            door += connector.gameObject.name + "-";

                        else if (connectionState == SnapConnectionState.DoorOneWay)
                            door += connector.gameObject.name + "_OW-";

                        else if (connectionState == SnapConnectionState.DoorLocked)
                            door += connector.gameObject.name + "_LK-";
                    }
                }
                DungeonCode += $"{module.name}#{identifier.position.x}#{identifier.position.z}#{Math.Round(module.eulerAngles.y)}#{door}|";
                DungeonCode = DungeonCode.Replace("(Clone)", "");
                DungeonCode = DungeonCode.Replace("Room-", "");
            }
        }

    }
}

/// <summary>
/// Player => Player character
/// Default => 4 Way room
/// Corridor => 2 Way room
/// Start => One way room
/// Exit => One way room
/// 
/// X => x coordinate on the map
/// Y => y coordinate on the map
/// R => rotation (default 0, see below)
/// 
/// ALL OF THE LOCATIONS BELOW ARE VALID BEFORE APPLYING ROTATION
/// Door_N => Door North
/// Door_S => Door South
/// Door_W => Door West
/// Door_E => Door East
/// Door_*_OW => Means that there is no door, only a passageway
/// Door_*_LK => Means that the door is locked
/// 
/// Separators => / - | # .
/// 
/// EXAMPLE: 
/// Start#X7.Y2.R90/Door_N-|
/// Default#X9.Y1.R270/Door_E-|
/// Corridor#X9.Y2.R270/Door_E-|
/// Default#X9.Y3.R270/Door_S-|
/// Default#X10.Y3.R270/Door_E-Door_W|
/// Default#X9.Y2.R0/Door_S-Door_E|
/// Corridor#X9.Y2.R90/Door_E|
/// Default#X10.Y1.R180/Door_E|
/// Default#X9.Y0.R270/Door_E_OW|
/// Corridor#X10.Y2.R0/Door_E|
/// Default#X12.Y2.R270/Door_E|
/// Corridor#X12.Y3.R270/Door_E|
/// Default#X12.Y5.R180/Door_E|
/// Corridor#X11.Y5.R180/Door_E_OW|
/// Default#X9.Y5.R90/Door_S-Door_W|
/// Corridor#X8.Y4.R0/Door_W|
/// Default#X7.Y4.R0/Door_W|
/// Default#X6.Y4.R0/Door_N|
/// Default#X7.Y6.R180/Door_W|
/// Corridor#X7.Y5.R0/Door_E_OW|
/// Default#X9.Y5.R270/Door_E|
/// Corridor#X9.Y6.R270/Door_E|
/// ExitX9.Y8.R180/|
/// Default#X10.Y5.R270/Door_N|
/// Plyer#X8.Y2.R90/|

///