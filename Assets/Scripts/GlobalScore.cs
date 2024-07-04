using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GlobalScore : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;
    public NetworkVariable<int> score = new(0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Global score : " + score.Value;
    }

    [Rpc(SendTo.Server)]
    public void IncrementScoreRpc()
    {
        score.Value++;
    }
}
