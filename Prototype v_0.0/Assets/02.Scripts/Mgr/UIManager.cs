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

    /* 설명 : HP바, HP텍스트를 배열형태로 받아와서 액터 코드에 맞게 본인의 HP를 표시.
     * 이게 어려울 경우, Actor의 hp를 전부 UI매니저 쪽에서 서버에서 받아서
     * 일일히 표시하는 방법이 있을듯
     */
    public void myHpChange(int fullHp, int currHp, int actorCode)
    {
        //HpText[0].text = currHp.ToString() + "/" + fullHp.ToString();
        //HpBar[0].fillAmount = (float)currHp / (float)fullHp;
    }

    
}
