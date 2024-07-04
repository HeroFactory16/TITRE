using System;
using Unity.Netcode;
using UnityEngine;

public class NetCollectible : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(Vector3.zero);
    public NetworkVariable<bool> isVisible = new NetworkVariable<bool>(true);
    public SpriteRenderer TreasureSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isVisible.OnValueChanged += ChangeVisibility;
    }

    public void ChangeVisibility(bool previousValue, bool newValue)
    {
        isVisible.Value = newValue;
        gameObject.SetActive(isVisible.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(isVisible.Value);
    }
}
