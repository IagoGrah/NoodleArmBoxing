using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObject : MonoBehaviour
{
    public Player PlayerPrefab;
    [HideInInspector] public int PlayerIndex;
    [HideInInspector] public PlayerInput PlayerInput;

    public string Name;
    public int ColorIndex;
    public int HeadIndex;

    public Color Color;
    public Sprite HeadSprite;

    public delegate void OnUINavigateEvent(Vector2 inputValue);
    public event OnUINavigateEvent OnUINavigate;

    public delegate void EmptyDelegate();
    public event EmptyDelegate OnUISubmit;
    public event EmptyDelegate OnUICancel;

    public delegate void OnPlayerMoveEvent(Vector2 inputValue);
    public event OnPlayerMoveEvent OnPlayerMove;

    public delegate void OnPlayerRotateEvent(float inputValue);
    public event OnPlayerRotateEvent OnPlayerRotate;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        PlayerInput = GetComponent<PlayerInput>();
        PlayerIndex = PlayerInput.playerIndex;
        Name = GetNameFromIndex(PlayerIndex);
        ColorIndex = PlayerIndex;
        HeadIndex = PlayerIndex;
    }

    private string GetNameFromIndex(int playerIndex)
    {
        var number = string.Empty;
        switch (playerIndex)
        {
            case 0:
                number = "One";
                break;

            case 1: 
                number = "Two";
                break;

            case 2:
                number = "Three";
                break;

            case 3:
                number = "Four";
                break;
        }
        return ("Player " + number).Trim();
    }

    private void OnNavigate(InputValue inputValue)
    {
        var v2 = inputValue.Get<Vector2>();
        OnUINavigate?.Invoke(v2);
    }

    private void OnSubmit()
    {
        OnUISubmit?.Invoke();
    }

    private void OnCancel()
    {
        OnUICancel?.Invoke();
    }

    private void OnMove(InputValue value)
    {
        OnPlayerMove?.Invoke(value.Get<Vector2>());
    }

    private void OnRotate(InputValue value)
    {
        OnPlayerRotate?.Invoke(value.Get<float>());
    }
}
