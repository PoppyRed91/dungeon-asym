using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _upperDoor, _lowerDoor;
    private BoxCollider _boxCollider;
    public bool IsOneWay;
    public bool CanOpen;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }


    public void OnInteract(Player player)
    {
        if (!CanOpen) return;
        _upperDoor.transform.DOLocalMoveY(1.6f, 1);
        _lowerDoor.transform.DOLocalMoveY(-1.6f, 1);
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        CanOpen = !IsOneWay;
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(2);
        _boxCollider.enabled = true;
        _upperDoor.transform.DOLocalMoveY(0.5f, 1);
        _lowerDoor.transform.DOLocalMoveY(-0.5f, 1);
    }
}
