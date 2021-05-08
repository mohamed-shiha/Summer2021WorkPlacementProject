using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinHostScreen : MonoBehaviour
{

    public void Connect()
    {
        // take the ip
        // use one port 
        // connect to the ip
        SceneManager.LoadScene("TestScene");
        StartCoroutine("StartAsClient", 1f);
    }

    private void StartAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void StartAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Host()
    {
        // take the ip
        // Set the ip as the host ip
        // start as host using the ip 
        SceneManager.LoadScene("TestScene");
        StartCoroutine("StartAsHost", 1f);
    }
}
