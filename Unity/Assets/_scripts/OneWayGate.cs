using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OneWayGate : MonoBehaviour
{
    [SerializeField] private GameObject _bars;
    [SerializeField] private BoxCollider _blocker;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    public void CloseDoor()
    {
        _bars.transform.DOLocalMoveY(-1f, 0.5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boxCollider.enabled = false;
            _blocker.enabled = true;
            CloseDoor();
        }
    }
}
