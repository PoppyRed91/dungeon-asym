using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MovementSpeed;
    public float TurnSpeed;
    [SerializeField] private Camera _camera;
    private bool _isMoving;
    private bool _isHittingFront;
    private bool _isHittingBack;
    private RaycastHit _frontHit, _backHit;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 2.5f, Color.red, 0);
        _isHittingFront = Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _frontHit, 2.1f);
        _isHittingBack = Physics.Raycast(_camera.transform.position, -_camera.transform.forward, out _backHit, 2.1f);
    }
    private void OnMove(InputValue input)
    {
        if (_isMoving) return;
        int direction = (int)input.Get<float>();

        if (direction > 0 && _isHittingFront) return;
        else if (direction < 0 && _isHittingBack) return;
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _isMoving = true);
        sequence.Append(transform.DOLocalMove(transform.position + (direction * _camera.transform.forward) * 2, MovementSpeed));
        sequence.AppendCallback(() => _isMoving = false);
    }

    private void OnTurn(InputValue input)
    {
        if (_isMoving) return;
        int direction = (int)input.Get<float>();
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _isMoving = true);
        sequence.Append(transform.DOLocalRotate(transform.eulerAngles + (direction * new Vector3(0, 90, 0)), TurnSpeed));
        sequence.AppendCallback(() => _isMoving = false);
    }

    private void OnInteract()
    {
        if (_isHittingFront)
            if (_frontHit.transform.GetComponent<IInteractable>() != null)
            {
                _frontHit.transform.GetComponent<IInteractable>().OnInteract();
                StartCoroutine(MoveThroughDoor());
            }

    }

    IEnumerator MoveThroughDoor()
    {
        _isMoving = true;
        yield return new WaitForSeconds(1);
        yield return transform.DOLocalMove(transform.position + _camera.transform.forward * 4, MovementSpeed * 2).WaitForCompletion(true);
        yield return new WaitForSeconds(1);
        _isMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 spherePosition = _camera.transform.position + _camera.transform.forward * 2.1f;

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + Vector3.up, Vector3.one * 1.8f);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * 2.1f);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(spherePosition, 0.25f);

    }

}
