using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] TMP_Text winText;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] HealthBar[] healthBars;
    [SerializeField] PlayerConfig[] playerConfigs;

    PlayerInputManager inputManager;

    List<Player> players = new List<Player>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        inputManager = GetComponent<PlayerInputManager>();
    }

    void OnPlayerJoined(PlayerInput player)
    {
        var playerComponent = player.GetComponent<Player>();
        playerComponent.Init(playerConfigs[player.playerIndex], healthBars[player.playerIndex]);

        var spawnPoint = spawnPoints[player.playerIndex];
        player.transform.SetPositionAndRotation(spawnPoint.position, transform.rotation);

        //player.DeactivateInput();
        players.Add(playerComponent);
    }

    void OnPlayerLeft(PlayerInput player)
    {
        var playerComponent = player.GetComponent<Player>();
        players.Remove(playerComponent);
    }

    public void OnPlayerDeath(Player deadPlayer)
    {
        players.Remove(deadPlayer);
        if (players.Count < 2)
        {
            ShowWinScreen(players[0].Config);
        }
    }

    private void ShowWinScreen(PlayerConfig playerConfig)
    {
        winScreen.SetActive(true);
        winText.text = playerConfig.Name + " wins!";
        winText.color = playerConfig.Color;
    }

    public void StartGame()
    {
        inputManager.DisableJoining();
        foreach (var player in players)
        {
            player.GetComponent<PlayerInput>().ActivateInput();
        }
        startScreen.SetActive(false);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
