using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetPlayerController : NetworkBehaviour
{
    public PlayerInputActions PIA;
    public Vector2 direction;
    public float speed = 10f;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(Vector3.zero);
    public Animator PlayerAnimator;
    //public GameObject personalUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            SetPositionServerRpc(direction);
        }

        if (IsServer)
        {
            transform.position = Position.Value;
        }
    }

    [Rpc(SendTo.Server)]
    public void SetPositionServerRpc(Vector2 actualInput)
    {
        var position = transform.position;
        position.x += actualInput.x * speed * NetworkManager.ServerTime.FixedDeltaTime;
        position.y += actualInput.y * speed * NetworkManager.ServerTime.FixedDeltaTime;
        Position.Value = position;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (NetworkObject.IsLocalPlayer)
        {
            Debug.Log("Name " + gameObject.name);
            PIA = new PlayerInputActions();
            PIA.Enable();
            PIA.Player.Move.started += context => direction = context.ReadValue<Vector2>();
            PIA.Player.Move.canceled += context => direction = context.ReadValue<Vector2>();
            PIA.Player.Move.performed += context => direction = context.ReadValue<Vector2>();
        }
    }
}
