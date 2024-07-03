using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetWorkAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ConnectToServer()
    {
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.OnClientConnectedCallback += obj => Debug.Log("Connected: " + obj);
    }

    public void LoadGameScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
