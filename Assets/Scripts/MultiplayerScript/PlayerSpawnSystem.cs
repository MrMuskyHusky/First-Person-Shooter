using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Spawn players in the world scene
/// </summary>
public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    NetworkManagerLobby networkManagerLobby;

    /// <summary>
    /// Add spawnpoints where they are set
    /// </summary>
    /// <param name="spawnTransform"></param>
    public static void AddSpawnPoint(Transform spawnTransform)
    {
        spawnPoints.Add(spawnTransform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform spawnTransform) => spawnPoints.Remove(spawnTransform);

    public override void OnStartServer()
    {
        NetworkManagerLobby.onServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        NetworkManagerLobby.onServerReadied -= SpawnPlayer;
    }
    /// <summary>
    /// Spawn the player in
    /// </summary>
    /// <param name="conn"></param>
    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if(spawnPoint == null)
        {
            Debug.LogError("Missing spawn point for player" + nextIndex);
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }
}
