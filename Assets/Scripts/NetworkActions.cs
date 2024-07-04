using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetWorkAction : MonoBehaviour
{
    public UIContainer UIContainer;
    // Start is called before the first frame update
    void Start()
    {
        UIContainer = FindFirstObjectByType<UIContainer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartHost();
        UIContainer.gameObject.SetActive(false);
    }

    public void ConnectToServer()
    {
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.OnClientConnectedCallback += obj => Debug.Log("Connected: " + obj);
        UIContainer.gameObject.SetActive(false);
    }

    public void LoadGameScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
