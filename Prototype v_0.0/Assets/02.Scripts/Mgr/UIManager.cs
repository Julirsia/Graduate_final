using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using clientData;

public class UIManager : MonoBehaviour
{
    public static UIManager instance= null;
    public int gameMode;
    public GameObject[] modeUI;

    public Image[] HpBar;
    public Text[] HpText;

    public Image[] BossHpList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /* 설명 : HP바, HP텍스트를 배열형태로 받아와서 액터 코드에 맞게 본인의 HP를 표시.
     * 이게 어려울 경우, Actor의 hp를 전부 UI매니저 쪽에서 서버에서 받아서
     * 일일히 표시하는 방법이 있을듯
     */
    public void hpChange(int fullHp, int currHp, int actorCode, ActorType type)
    {
        if (type == ActorType.boss)
        {
            int count = currHp / 200;
            if (fullHp > 200)
                HpText[actorCode / 1000].text = count.ToString();

            HpBar[actorCode / 1000].fillAmount = (float)(currHp - 200 * count) / (float)fullHp;
        }
        else
        {
            HpText[actorCode / 1000].text = currHp.ToString() + "/" + fullHp.ToString();
            HpBar[actorCode / 1000].fillAmount = (float)currHp / (float)fullHp;
        }
    }
}
