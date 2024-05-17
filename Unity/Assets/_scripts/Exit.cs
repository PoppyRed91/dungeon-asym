using UnityEngine;

public class Exit : MonoBehaviour, IInteractable
{
    public void OnInteract(Player player)
    {
        player.SwitchLevel();
    }
}
