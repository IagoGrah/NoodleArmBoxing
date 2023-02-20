using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Transform PlayerSlots;
    public string GameScene;
    public string UIControlMap;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject joinPanel;
    [SerializeField] GameObject playersRequiredText;

    public PlayerPanel PanelPrefab;

    private void Awake()
    {
        PlayersManager.Instance.InputManager.EnableJoining();
        foreach (var player in PlayersManager.Instance.Players)
        {
            OnPlayerJoined(player.PlayerInput);
        }

        UpdatePlayerCountUI();

        PlayersManager.Instance.InputManager.onPlayerJoined += OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft += OnPlayerLeft;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        var playerObject = player.GetComponent<PlayerObject>();
        player.SwitchCurrentActionMap(UIControlMap);

        var playerPanel = Instantiate(PanelPrefab, PlayerSlots);
        playerPanel.transform.SetSiblingIndex(player.playerIndex);
        playerPanel.PlayerObj = playerObject;

        UpdatePlayerCountUI();
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        UpdatePlayerCountUI();
    }

    private void UpdatePlayerCountUI()
    {
        var enoughPlayers = PlayersManager.Instance.Players.Count > 1;
        playButton.SetActive(enoughPlayers);
        playersRequiredText.SetActive(!enoughPlayers);

        var canJoinMore = PlayersManager.Instance.Players.Count < 4;
        joinPanel.SetActive(canJoinMore);
        joinPanel.transform.SetAsLastSibling();
    }

    public void StartGame()
    {
        PlayersManager.Instance.InputManager.onPlayerJoined -= OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft -= OnPlayerLeft;

        PlayersManager.Instance.InputManager.DisableJoining();
        AudioManager.Instance.PlayFightBGM();
        SceneManager.LoadScene(GameScene);
    }
}
