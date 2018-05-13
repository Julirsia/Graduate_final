using UnityEngine;
using System.Collections;


//Editor 폴더 -> 실행되면 제일 먼저 불러오는폴더. 에디터를 건드리는 스크립트도 같이 호출되서 변경된다
public class MapTool : MonoBehaviour
{
    /*
    [MenuItem("MyMenu/SayHello")]
    static void SayHello()
    {
        Debug.Log("Hello");

        PlayerSettings.virtualRealitySupported = false;//에디터를 직접 건든다
    }*/

    public int tileX;
    public int tileY;
    public bool ShowGrid = true;

    public GameObject floorPrefab;
    public GameObject selectedTilePrefab;

    public GameObject[] Tiles;
}
