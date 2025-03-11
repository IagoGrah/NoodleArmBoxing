using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    [SerializeField] Player playerPrefab;
    [SerializeField] GameObject winScreen;
    [SerializeField] TMP_Text winText;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] HealthBar[] healthBars;

    public string GameControlMap;

    List<Player> players = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (NetworkManager.IsHost)
        {
            foreach (var player in PlayersManager.Instance.Players)
            {
                SpawnPlayer(player);
            }
        }
        PlayersManager.Instance.OnPlayerJoined += SpawnPlayer;
    }

    private void SpawnPlayer(PlayerObject player)
    {
        if (NetworkManager.IsHost)
        {
            var spawnPoint = spawnPoints[player.PlayerIndex];

            var playerComponent = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerComponent.Init(player, healthBars[player.PlayerIndex]);

            playerComponent.GetComponent<NetworkObject>().Spawn();

            players.Add(playerComponent);
        }
    }

    public void OnPlayerDeath(Player deadPlayer)
    {
        players.Remove(deadPlayer);
        if (players.Count < 2)
        {
            AudioManager.Instance.PlayBellSound();
            ShowWinScreen(players[0].PlayerObject);
        }
    }

    private void ShowWinScreen(PlayerObject playerObject)
    {
        winScreen.SetActive(true);
        winText.text = playerObject.Name + " wins!";
        winText.color = playerObject.Color;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlayMenuBGM();
        SceneManager.LoadScene(0);
    }
}
