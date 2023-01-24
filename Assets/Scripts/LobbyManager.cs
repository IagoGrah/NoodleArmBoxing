using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Transform PlayerSlots;
    public string GameScene;
    public string UIControlMap;

    public PlayerPanel PanelPrefab;

    private void Awake()
    {
        PlayersManager.Instance.InputManager.onPlayerJoined += OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft += OnPlayerLeft;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        var playerObject = player.GetComponent<PlayerObject>();
        player.SwitchCurrentActionMap(UIControlMap);

        var playerPanel = Instantiate(PanelPrefab, PlayerSlots);
        playerPanel.PlayerObj = playerObject;
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        //var playerPanel = player.GetComponent<PlayerPanel>();
        //if (playerPanel != null)
        //{
        //    PlayerConfigs.Remove(playerPanel.config);
        //}
    }

    public void StartGame()
    {
        PlayersManager.Instance.InputManager.onPlayerJoined -= OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft -= OnPlayerLeft;

        PlayersManager.Instance.InputManager.DisableJoining();
        SceneManager.LoadScene(GameScene);
    }
}
