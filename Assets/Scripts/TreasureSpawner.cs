using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class TreasureSpawner : NetworkBehaviour
{
    public NetCollectible collectible;
    float elapsedTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!IsServer)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 2)
        {
            elapsedTime = 0;
            SpawnTreasureRpc();
        }
    }

    void SpawnTreasureRpc()
    {
        var treasure = Instantiate(collectible);
        var pos = treasure.transform.position;

        pos += Vector3.up * Random.Range(-3, 4.5f);
        pos += Vector3.right * Random.Range(-7.5f, 7.5f);
        treasure.transform.position = pos;

        var netTreasure = treasure.GetComponent<NetworkObject>();
        netTreasure.Spawn();
    }
}
