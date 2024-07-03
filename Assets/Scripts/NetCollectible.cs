using Unity.Netcode;
using UnityEngine;

public class NetCollectible : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(Vector3.zero);
    public SpriteRenderer TreasureSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
