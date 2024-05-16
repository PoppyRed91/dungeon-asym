using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;

[Serializable]
public class PlayerSettings
{
    public float MovementSpeed;
    public float TurningSpeed;
}

[Serializable]
public class PlayerInput
{
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode StepLeft;
    public KeyCode StepRight;
    public KeyCode TurnAround;
    public KeyCode Interact;
    public KeyCode Sprint;
}

[Serializable]
public class PlayerReferences
{
    public Camera Camera;
    public List<AudioClip> Footsteps;
}

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private PlayerReferences _references;
    [SerializeField] private PlayerInput _input;

    public Keyboard keyboard;

    private bool _isMoving;
    private bool _isRotating;
    public bool HasInput;
    private Vector3 _originalPosition, _targetPosition;

    private void Update()
    {
        Debug.DrawRay(_references.Camera.transform.position, _references.Camera.transform.forward, Color.red, 0);
        Debug.DrawRay(_references.Camera.transform.position, -_references.Camera.transform.forward, Color.red, 0);
        HandleInput();
        HandleDebugInput();
    }

    private void Interact()
    {
        if (Physics.Raycast(_references.Camera.transform.position, _references.Camera.transform.forward, out RaycastHit hit, 2))
            hit.transform.GetComponent<IInteractable>()?.OnInteract(this);
    }

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NET_UpdatePosition();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            NetworkManager.Instance.Socket.EmitAsync("map", DungeonManager.Instance.DungeonCode);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            NetworkManager.Instance.Socket.EmitAsync("compass", "Nothing to see here");
        }
    }

    private void HandleInput()
    {
        if (!HasInput) return;
        if (Input.GetKeyDown(_input.Interact)) Interact();
        if (_isMoving || _isRotating) return;
        if (Input.GetKey(_input.Forward))
        {
            if (Physics.Raycast(_references.Camera.transform.position, _references.Camera.transform.forward, 1.5f)) return;
            StartCoroutine(MovePlayer(_references.Camera.transform.forward));

        }
        if (Input.GetKey(_input.Backward))
        {
            if (Physics.Raycast(_references.Camera.transform.position, -_references.Camera.transform.forward, 1.5f)) return;
            StartCoroutine(MovePlayer(-_references.Camera.transform.forward));
        }
        if (Input.GetKey(_input.StepLeft))
        {
            if (Physics.Raycast(_references.Camera.transform.position, -_references.Camera.transform.right, 1.5f)) return;
            StartCoroutine(MovePlayer(-_references.Camera.transform.right));
        }
        if (Input.GetKey(_input.StepRight))
        {
            if (Physics.Raycast(_references.Camera.transform.position, _references.Camera.transform.right, 1.5f)) return;
            StartCoroutine(MovePlayer(_references.Camera.transform.right));
        }

        if (_isMoving) return;
        if (Input.GetKey(_input.TurnLeft)) StartCoroutine(TurnPlayer(new Vector3(0, -90, 0)));
        if (Input.GetKey(_input.TurnRight)) StartCoroutine(TurnPlayer(new Vector3(0, 90, 0)));
        if (Input.GetKey(_input.TurnAround)) StartCoroutine(TurnPlayer(new Vector3(0, 180, 0)));
    }


    IEnumerator MovePlayer(Vector3 direction)
    {
        _isMoving = true;
        float elapsedTime = 0;
        _originalPosition = transform.position;
        _targetPosition = _originalPosition + direction;
        _references.Camera.transform.DOLocalJump(Vector3.up, 0.05f, 1, _settings.MovementSpeed);
        while (elapsedTime < _settings.MovementSpeed)
        {
            transform.position = Vector3.Lerp(_originalPosition, _targetPosition, elapsedTime / _settings.MovementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _targetPosition;
        //_audioSource.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Count)]);
        AudioSource.PlayClipAtPoint(_references.Footsteps[UnityEngine.Random.Range(0, _references.Footsteps.Count)], transform.position);
        _isMoving = false;
    }
    IEnumerator TurnPlayer(Vector3 rotation)
    {
        _isRotating = true;
        float elapsedTime = 0;
        _originalPosition = transform.eulerAngles;
        _targetPosition = _originalPosition + rotation;
        _references.Camera.transform.DOLocalJump(Vector3.up, 0.05f, 1, _settings.TurningSpeed);
        while (elapsedTime < _settings.TurningSpeed)
        {
            transform.eulerAngles = Vector3.Lerp(_originalPosition, _targetPosition, elapsedTime / _settings.TurningSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = _targetPosition;
        //_audioSource.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Count)]);
        AudioSource.PlayClipAtPoint(_references.Footsteps[UnityEngine.Random.Range(0, _references.Footsteps.Count)], transform.position);
        _isRotating = false;
        NET_UpdatePosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomIdentifier"))
        {
            NET_UpdatePosition();
        }
    }

    public async void NET_UpdatePosition()
    {
        await NetworkManager.Instance.Socket.EmitAsync("player", $"X{Math.Floor(transform.position.x)}.Y{Math.Floor(transform.position.z)}.R{Math.Round(transform.eulerAngles.y)}");
    }

}
