using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PersonalScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Personal score : " + score;
    }
    public void SetScore(int newScore)
    {
        score = newScore;
        Debug.Log("Score = " + score);
    }
}
