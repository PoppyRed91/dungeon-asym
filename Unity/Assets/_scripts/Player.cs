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
    private bool _isHittingSolid;
    private RaycastHit _hit;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red, 0);
        Debug.DrawRay(_camera.transform.position, -_camera.transform.forward, Color.red, 0);
    }
    private void OnMove(InputValue input)
    {
        Debug.Log("Pressed Move");
        var direction = input.Get<float>();
        Debug.Log("Direction: " + direction);
        if (_isMoving || CheckCollision(direction)) return;
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _isMoving = true);
        sequence.Append(transform.DOMove(transform.position + 2 * direction * _camera.transform.forward, MovementSpeed));
        sequence.AppendCallback(() => _isMoving = false);
    }

    private void OnTurn(InputValue input)
    {
        Debug.Log("Pressed Turn");
        int direction = (int)input.Get<float>();
        if (_isMoving || direction == 0) return;
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _isMoving = true);
        sequence.Append(transform.DORotate(transform.eulerAngles + (direction * new Vector3(0, 90, 0)), TurnSpeed));
        sequence.AppendCallback(() => _isMoving = false);
    }

    private void OnInteract()
    {
        if (CheckCollision(1))
            if (_hit.transform.GetComponent<IInteractable>() != null)
            {
                _hit.transform.GetComponent<IInteractable>().OnInteract();
                StartCoroutine(MoveThroughDoor());
            }
    }

    private bool CheckCollision(float direction)
    {
        if (Physics.Raycast(_camera.transform.position, direction * _camera.transform.forward, out _hit, 2f))
        {
            Debug.Log("Collision with " + _hit.transform.name);
            return true;
        }
        else return false;

    }

    IEnumerator MoveThroughDoor()
    {
        _isMoving = true;
        yield return new WaitForSeconds(1);
        yield return transform.DOMove(transform.position + _camera.transform.forward * 4, MovementSpeed * 2).WaitForCompletion(true);
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
