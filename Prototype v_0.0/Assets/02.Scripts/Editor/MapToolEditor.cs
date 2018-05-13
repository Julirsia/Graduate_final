using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapTool))]
public class MapToolEditor : Editor
{
    MapTool mt; //mapTool 스크립트 안의 꺼를 변경시키면 다 변함. 여기서 변경시킴
    Vector3 center = Vector3.zero;

    float currTime = 0f;
    public float createTime = 0.2f;
    public static int selected = 0;

    void OnEnable()
    {
        mt = (MapTool)target; //mapTool 스크립트 안의 꺼를 변경시키면 다 변함. 여기서 변경시킴
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        mt.ShowGrid = EditorGUILayout.Toggle("그리드 표시?",mt.ShowGrid);
        mt.tileX = EditorGUILayout.IntField("Tile x 값 입력", mt.tileX);
        mt.tileY = EditorGUILayout.IntField("Tile y 값 입력", mt.tileY);
        if (mt.tileX <= 0)
            mt.tileX = 1;
        if (mt.tileY <= 0)
            mt.tileY = 1;
        mt.floorPrefab = (GameObject)EditorGUILayout.ObjectField("Floor Tile",mt.floorPrefab, typeof(GameObject));
        mt.selectedTilePrefab = (GameObject)EditorGUILayout.ObjectField("Selected Tile", mt.selectedTilePrefab, typeof(GameObject));

        serializedObject.Update();
        Show(serializedObject.FindProperty("Tiles"), EditorListOptions.ListSize|EditorListOptions.ListLabel |EditorListOptions.Buttons); 
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("CreateMap")) 
        {
            //Debug.Log("CreateMap Clicked");
            //클릭하면 타일을 촵 깔아버린다
            CheckFloorExsist();
            CreateMap();
        }
        createTime = EditorGUILayout.FloatField("Tile생성시간 간격", createTime);
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void OnSceneGUI() //Scene에서 그려주는 역할을 한다
    {
        if(mt.ShowGrid)
            DrawGrid();
        int id = GUIUtility.GetControlID(FocusType.Passive);//다른 애들이 포커스를 가지고 가지 못하도록 바꿔야 맵툴만 선택되어 있는다
        HandleUtility.AddDefaultControl(id);
        DrawTiles();
        DrawSelectedTilePreview();


    }

    void DrawSelectedTilePreview()
    {
        //prefab에 프리뷰 이미지가 있다면 넘겨준다
        Texture2D preview = AssetPreview.GetAssetPreview(mt.selectedTilePrefab);
        if (preview == null)
            preview = AssetPreview.GetMiniThumbnail(mt.selectedTilePrefab);

        if (preview)
        {
            Handles.BeginGUI();
            GUI.Button(new Rect(10, 10, 100, 100), preview);
            Handles.EndGUI();
        }
    }

    public void DrawGrid()
    {
        //어디에서부터 그릴까
       
        //Scene뷰에 그리는 작업 - Handles
        Handles.color = Color.green;
        for (int i = 0; i <= mt.tileY; i++)
        {
            for (int j = 0; j <= mt.tileX; j++)
            {
                Handles.DrawLine(center + Vector3.right * j, center + Vector3.right * j + Vector3.forward * mt.tileY);
            }
            Handles.DrawLine(center + Vector3.forward * i, center + Vector3.forward * i + Vector3.right * mt.tileX);
        }
        
    }

    bool isMouseDownState = false;
    //타일을 바닥 타일 위에 그림
    //selectedTilePrefab을 그린다.
    //여러개의 타일을 등록하여 쓸 수 있도록 
    //타일 미리보기 기능?
    // 마우스를 클릭하면 그린다
    void DrawTiles()
    {
        Event e = Event.current; // 현재 발생한 이벤트를 가져올 수 있음
        // if mouse button Down, 그리기 모드
        // mouse up - 그리기 종료
        if (e.alt)
            return;
        if (e.type == EventType.MouseDown)
            isMouseDownState = true;
        if (e.type == EventType.MouseUp)
            isMouseDownState = false;

        if (isMouseDownState)
        {
            currTime += Time.maximumDeltaTime;
            if (currTime < createTime)
                return;
            currTime = 0f;
            //어느 지점에 마우스를 클릭했는지 찾는다
            // 마우스 클릭된 지점으로 Ray를 쏜다
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.tag == "Tile")
                {
                    RaycastHit hit;
                    while (true)
                    {
                        ray = new Ray(hitInfo.transform.position, Vector3.up);
                        if (Physics.Raycast(ray, out hit))
                            hitInfo = hit;
                        else
                            break;
                    }

                    //Shift를 누르면 제거시킨다
                    if (e.shift)
                    {
                        DestroyImmediate(hitInfo.transform.gameObject);
                    }
                    else
                    {
                        return;
                        /*
                        GameObject tile = PrefabUtility.InstantiatePrefab(mt.selectedTilePrefab) as GameObject;
                        tile.transform.parent = GameObject.Find("Tiles").transform;
                        tile.transform.position = hitInfo.transform.position + Vector3.up;*/
                    }


                }
                else if (hitInfo.transform.tag == "Floor")
                {
                    if (e.shift)
                        return;
                        //ray와 부딛힌게 floor일때
                        //찾아낸 지점에 Tile 생성 ->selectedTilePrefab
                        //index찾기 -> 소숫점은 떼버림
                    GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(mt.selectedTilePrefab) as GameObject;
                    tile.transform.parent = GameObject.Find("Tiles").transform;
                    int x = Mathf.FloorToInt(hitInfo.point.x % mt.tileX); //소숫점 뒷자리 떼뿌림
                    int z = Mathf.FloorToInt(hitInfo.point.z % mt.tileY); //소숫점 뒷자리 떼뿌림

                    tile.transform.position = new Vector3(x+0.5f, 0.5f, z+0.5f);
                }

                
                
            }

        }
    }
    
    //기존 타일이 있으면 날려버림
    void CheckFloorExsist()
    {
        if (GameObject.FindGameObjectWithTag("FloorParent"))
            DestroyImmediate(GameObject.FindGameObjectWithTag("FloorParent"));
        if (GameObject.FindGameObjectWithTag("TileParent"))
            DestroyImmediate(GameObject.FindGameObjectWithTag("TileParent"));

        GameObject Tiles = new GameObject("Tiles");
        Tiles.tag = "TileParent";
    }


    //바닥타일 깔기
    //1x1짜리 타일을 tilex, tile y만큼 키워서 쓴다
    void CreateMap()
    {
        GameObject floor = PrefabUtility.InstantiatePrefab(mt.floorPrefab) as GameObject;
        //타일 크기를 x, y만큼 키우기
        floor.transform.localScale = new Vector3(mt.tileX, 1, mt.tileY);
        floor.transform.position = center;

    }



    private static GUIContent
        selectButtonContent = new GUIContent("Select", "Select");

    public void Show(SerializedProperty list, EditorListOptions options = EditorListOptions.Default)
    {
        bool
            showListLabel = (options & EditorListOptions.ListLabel) != 0,
            showListSize = (options & EditorListOptions.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            ShowElements(list, options);
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }
    private void ShowElements(SerializedProperty list, EditorListOptions options)
    {
        bool
            showElementLabels = (options & EditorListOptions.ElementLabels) != 0,
            showButtons = (options & EditorListOptions.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(selectButtonContent, GUILayout.Width(50f)))
        {
           mt.selectedTilePrefab = mt.Tiles[index];
        }
    }
}
