using Unity.Netcode;
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
        foreach (var player in PlayersManager.Instance.Players)
        {
            OnPlayerJoined(player);
        }

        UpdatePlayerCountUI();

        PlayersManager.Instance.OnPlayerJoined += OnPlayerJoined;
        PlayersManager.Instance.OnPlayerLeft += OnPlayerLeft;
    }

    public void OnPlayerJoined(PlayerObject player)
    {
        var playerInput = player.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap(UIControlMap);

        var playerPanel = Instantiate(PanelPrefab, PlayerSlots);
        playerPanel.transform.SetSiblingIndex(playerInput.playerIndex);
        playerPanel.PlayerObj = player;

        UpdatePlayerCountUI();
    }

    private void OnPlayerLeft(PlayerObject player)
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
        PlayersManager.Instance.OnPlayerJoined -= OnPlayerJoined;
        PlayersManager.Instance.OnPlayerLeft -= OnPlayerLeft;

        AudioManager.Instance.PlayFightBGM();
        SceneManager.LoadScene(GameScene);
    }
}
