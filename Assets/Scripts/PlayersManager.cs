using System.Collections.Generic;
using Unity.Netcode;

public class PlayersManager : NetworkBehaviour
{
    public static PlayersManager Instance;

    public List<PlayerObject> Players = new();

    public delegate void OnPlayerJoinedEvent(PlayerObject player);
    public event OnPlayerJoinedEvent OnPlayerJoined;

    public delegate void OnPlayerLeftEvent(PlayerObject player);
    public event OnPlayerLeftEvent OnPlayerLeft;

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
    }

    public void AddPlayer(PlayerObject player)
    {
        Players.Add(player);
        player.SetIndexRpc(NetworkManager.Singleton.ConnectedClients.Count - 1);
        OnPlayerJoined?.Invoke(player);
    }

    public void RemovePlayer(PlayerObject player)
    {  
        Players.Remove(player);
        OnPlayerLeft?.Invoke(player);
    }
}
