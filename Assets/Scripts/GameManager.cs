using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
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
        foreach (var player in PlayersManager.Instance.Players)
        {
            var spawnPoint = spawnPoints[player.PlayerIndex];

            var playerComponent = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerComponent.Init(player, healthBars[player.PlayerIndex]);

            player.PlayerInput.SwitchCurrentActionMap(GameControlMap);

            players.Add(playerComponent);
        }
    }

    public void OnPlayerDeath(Player deadPlayer)
    {
        players.Remove(deadPlayer);
        if (players.Count < 2)
        {
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
        SceneManager.LoadScene(0);
    }
}
