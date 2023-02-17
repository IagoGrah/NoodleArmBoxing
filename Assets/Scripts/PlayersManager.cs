using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    [HideInInspector] public PlayerInputManager InputManager;
    public List<PlayerObject> Players = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        InputManager = GetComponent<PlayerInputManager>();
        InputManager.onPlayerJoined += OnPlayerJoined;
        InputManager.onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        var playerObject = player.GetComponent<PlayerObject>();
        if (playerObject != null)
        {
            playerObject.PlayerIndex = player.playerIndex;
            Players.Add(playerObject);
        }
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        var playerObject = player.GetComponent<PlayerObject>();
        if (playerObject != null)
        {
            Players.Remove(playerObject);
        }
    }

    private void OnDestroy()
    {
        InputManager.onPlayerJoined -= OnPlayerJoined;
        InputManager.onPlayerLeft -= OnPlayerLeft;
    }
}
