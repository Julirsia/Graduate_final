using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject Title;
    public GameObject ModeSelection;
    public GameObject ComingSoonPanel;

    public void ButtonActionQuit()
    {
        Application.Quit();    
    }

    public void ButtonActionModeChange()
    {
        Title.SetActive(false);
        ModeSelection.SetActive(true);
    }

    public void ButtonActionComingSoon()
    {
        ComingSoonPanel.SetActive(true);
        StartCoroutine(ComingSooPanelDisappear());
    }
    IEnumerator ComingSooPanelDisappear()
    {
        yield return new WaitForSeconds(4f);
        ComingSoonPanel.SetActive(false);
    }

    public void ButtonActionLoadScene_Game()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonActionLoadScene_Title()
    {
        SceneManager.LoadScene(1);
    }
}
