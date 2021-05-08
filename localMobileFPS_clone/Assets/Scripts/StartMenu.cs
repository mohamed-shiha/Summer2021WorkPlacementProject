using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private void Start()
    {
        // start with start screen 
        ShowScreen(3);
    }
    public Transform StartScreen;
    public Transform SettingsScreen;
    public Transform PlayerRecords;
    public Transform PlayModeScreen;
    public void GoToSettings()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(1);
        Debug.Log("Button Start Settings");
    }

    public void GoToPlayMode()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(0);
        Debug.Log("Button Start Game Mode");
    }

    public void SaveAndExit()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        Application.Quit();
        Debug.Log("Button Save and exit");
    }

    public void ShowPlayerRecords()
    {
        // start showing animation 
        // Set start menu panel to not active
        // set play mode screen to active 
        ShowScreen(2);
        Debug.Log("Button Show Player Records");
    }

    public void BackToMainMenu()
    {
        ShowScreen(3);
        Debug.Log("Button Show BackToMain");
    }

    private void ShowScreen(int index)
    {
        StartScreen.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);
        PlayModeScreen.gameObject.SetActive(false);
        PlayerRecords.gameObject.SetActive(false);

        switch (index)
        {
            case 0:
                PlayModeScreen.gameObject.SetActive(true);
                break;
            case 1:
                SettingsScreen.gameObject.SetActive(true);
                break; 
            case 2:
                PlayerRecords.gameObject.SetActive(true);   
                break;
            case 3:
                StartScreen.gameObject.SetActive(true);
                break;
        }
    }
}
