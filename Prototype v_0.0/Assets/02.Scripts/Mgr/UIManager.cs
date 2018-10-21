using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance= null;
    public int gameMode;
    public GameObject[] modeUI;

    public Image[] HpBar;
    public Text[] HpText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void myHpChange(int fullHp, int currHp, int actorCode)
    {
        HpText[actorCode].text = currHp.ToString() + "/" + fullHp.ToString();
        HpBar[actorCode].fillAmount = (float)currHp / (float)fullHp;
    }

    
}
