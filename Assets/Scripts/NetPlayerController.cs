using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using Unity.Cinemachine;

public class NetPlayerController : NetworkBehaviour
{
    public PlayerInputActions PIA;
    public Vector2 direction;
    public Vector2 inputs;
    public Vector2 lastDirection;
    public float speed = 10f;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(Vector3.zero);
    public Animator PlayerAnimator;
    public bool facingLeft = true;
    public SpriteRenderer PlayerSpriteRenderer;
    public List<NetCollectible> inventory;
    public Rigidbody2D playerRigidBody;
    public GlobalScore globalScore;
    public PersonalScore personalScore;
    public int personalScoreValue;

    //public GameObject personalUI;

    // Start is called before the first frame update
    void Start()
    {
        personalScore = FindFirstObjectByType<PersonalScore>();
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
            //Debug.Log("DirX = " + direction.x + " DirY = " + direction.y + " facingLeft = " + facingLeft);
            if ((direction.x < 0 && !facingLeft) || (direction.x > 0 && facingLeft))
            {
                FlipPlayerRpc();
            }
        }

        if (IsServer)
        {
            playerRigidBody.MovePosition(Position.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            Debug.Log("Collision with " +  collision.name);
            if (collision.GetComponent<NetCollectible>() != null)
            {
                AddToInventoryRpc(collision.GetComponent<NetCollectible>());
            }
        }
    }

    [Rpc(SendTo.Server)]
    void AddToInventoryRpc(NetworkBehaviourReference collectible)
    {
        var c = (NetCollectible) collectible;
        if (c.isVisible.Value)
        {
            inventory.Add(c);
            c.ChangeVisibility(c.isVisible.Value, false);
            globalScore.IncrementScoreRpc();
            IncrementScoreRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void SetPositionServerRpc(Vector2 actualInput)
    {
        float moveX = actualInput.x;
        float moveY = actualInput.y;

        if ((moveX == 0 && moveY == 0) && (inputs.x != 0 || inputs.y != 0))
        {
            //Debug.Log("lastDir.x = " + lastDirection.x + " y = " + lastDirection.y);
            lastDirection = inputs;
        }

        inputs = actualInput;

        var position = playerRigidBody.position;
        position.x += actualInput.x * speed * NetworkManager.ServerTime.FixedDeltaTime;
        position.y += actualInput.y * speed * NetworkManager.ServerTime.FixedDeltaTime;
        Position.Value = position;

        SendAnimationRpc(actualInput, lastDirection);
    }

    [Rpc(SendTo.Everyone)]
    public void SendAnimationRpc(Vector2 actualInput, Vector2 lastDir)
    {
        PlayerAnimator.SetFloat("MoveX", actualInput.x);
        PlayerAnimator.SetFloat("MoveY", actualInput.y);
        PlayerAnimator.SetFloat("LastMoveX", lastDir.x);
        PlayerAnimator.SetFloat("LastMoveY", lastDir.y);
        PlayerAnimator.SetFloat("MoveMagnitude", actualInput.magnitude);
    }

    [Rpc(SendTo.Server)]
    public void FlipPlayerRpc()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        UpdateFacingLeftRpc();
    }

    [Rpc(SendTo.Owner)]
    public void IncrementScoreRpc()
    {
        personalScoreValue++;
        personalScore.SetScore(personalScoreValue);
        Debug.Log("Score = " + personalScoreValue);
    }

    [Rpc(SendTo.Everyone)]
    void UpdateFacingLeftRpc()
    {
        facingLeft = !facingLeft;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerRigidBody.isKinematic = false;
        globalScore = FindFirstObjectByType<GlobalScore>();


        if (NetworkObject.IsLocalPlayer)
        {
            CinemachineCamera playerCamera = FindFirstObjectByType<CinemachineCamera>();

            if (playerCamera != null)
            {
                playerCamera.LookAt = gameObject.transform;
                playerCamera.Follow = gameObject.transform;
            }

            Debug.Log("Name " + gameObject.name);
            PIA = new PlayerInputActions();
            PIA.Enable();
            PIA.Player.Move.started += context => direction = context.ReadValue<Vector2>();
            PIA.Player.Move.canceled += context => direction = context.ReadValue<Vector2>();
            PIA.Player.Move.performed += context => direction = context.ReadValue<Vector2>();
        }
    }
}
