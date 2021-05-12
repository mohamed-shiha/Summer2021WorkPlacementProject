using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : MonoBehaviour
{

    public Transform JoinHostPanel;
    public Transform MainPanel;
    public Button ConnectCreateButton;

    public void Join()
    {
        MainPanel.gameObject.SetActive(false);
        JoinHostPanel.gameObject.SetActive(true);
        ConnectCreateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connect";
    }

    public void Host()
    {
        MainPanel.gameObject.SetActive(false);
        JoinHostPanel.gameObject.SetActive(true);
        ConnectCreateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Create";
    }

    public void Back()
    {
        MainPanel.gameObject.SetActive(true);
        JoinHostPanel.gameObject.SetActive(false);
    }
}
