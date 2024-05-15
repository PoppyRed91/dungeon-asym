using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isLowerButton;
    [SerializeField] private GameObject _platform;

    private bool _isPressed;

    public void OnInteract(Player player)
    {
        if (!_isPressed) StartCoroutine(MoveElevator(_isLowerButton, player));
    }

    IEnumerator MoveElevator(bool up, Player player)
    {
        _isPressed = true;
        player.HasInput = false;
        player.transform.SetParent(_platform.transform);
        if (up) _platform.transform.DOLocalMoveY(2, 2).SetEase(Ease.Linear);
        else _platform.transform.DOLocalMoveY(0, 2).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2);
        player.transform.SetParent(null);
        player.HasInput = true;
        _isPressed = false;
    }
}
