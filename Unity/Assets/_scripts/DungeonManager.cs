using DungeonArchitect;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Dungeon Dungeon;

    private void Start()
    {
        if (Dungeon != null)
        {
            Dungeon.Config.Seed = (uint)(Random.value * int.MaxValue);
            Dungeon.Build();
        }
    }

}
