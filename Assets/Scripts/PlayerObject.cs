using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObject : NetworkBehaviour
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

    private Sprite[] heads;
    private Color[] colors;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        heads = Resources.LoadAll<Sprite>("Heads");
        colors = Resources.Load<ColorList>("ColorList").Colors;
        PlayerInput = GetComponent<PlayerInput>();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetIndexRpc(int index)
    {
        PlayerIndex = index;
        Name = GetNameFromIndex(PlayerIndex);
        ColorIndex = PlayerIndex;
        HeadIndex = PlayerIndex;
        HeadSprite = heads[HeadIndex];
        Color = colors[ColorIndex];
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
        MoveRpc(value.Get<Vector2>());
    }
    [Rpc(SendTo.Server)]
    private void MoveRpc(Vector2 value)
    {
        OnPlayerMove?.Invoke(value);
    }

    private void OnRotate(InputValue value)
    {
        RotateRpc(value.Get<float>());
    }
    [Rpc(SendTo.Server)]
    private void RotateRpc(float value)
    {
        OnPlayerRotate?.Invoke(value);
    }

    public override void OnNetworkSpawn()
    {
        PlayerInput.enabled = IsOwner;
        PlayersManager.Instance.AddPlayer(this);
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        PlayerInput.enabled = false;
        PlayersManager.Instance.RemovePlayer(this);
        base.OnNetworkDespawn();
    }
}
