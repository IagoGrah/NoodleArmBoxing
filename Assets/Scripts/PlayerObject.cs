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

    public delegate void OnPlayerMoveEvent(Vector2 inputValue);
    public event OnPlayerMoveEvent OnPlayerMove;

    public delegate void OnPlayerRotateEvent(float inputValue);
    public event OnPlayerRotateEvent OnPlayerRotate;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        PlayerInput = GetComponent<PlayerInput>();
        PlayerIndex = PlayerInput.playerIndex;
        Name = "Player " + (PlayerIndex + 1);
        ColorIndex = PlayerIndex;
        HeadIndex = PlayerIndex;
    }

    private void OnNavigate(InputValue inputValue)
    {
        var v2 = inputValue.Get<Vector2>();
        OnUINavigate?.Invoke(v2);
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
