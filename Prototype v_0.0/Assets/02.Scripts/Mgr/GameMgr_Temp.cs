using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr_Temp : MonoBehaviour
{
    public Actor User;
    public Actor NPC;
    public Actor Zombie;

	// Use this for initialization
	void Start ()
    {
        //Time.timeScale = 0f;
    }
    public void ButtonActionLoadScene_Title()
    {
        SceneManager.LoadScene(1);
    }
    public void ButtonActionStartGame()
    {
        UIManager.instance.GuidePanelActive();
        Time.timeScale = 1f;
    }


}
