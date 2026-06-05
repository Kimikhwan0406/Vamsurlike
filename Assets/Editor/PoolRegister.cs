using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolPrefabRegister : EditorWindow
{
    // 네 PoolManager 실제 필드명에 맞게 수정해야 함
    private const string PoolListFieldName = "pools";
    private const string PoolIdFieldName = "poolId";
    private const string SizeFieldName = "size";
    private const string PrefabFieldName = "prefab";

    private const int DefaultSize = 60;

    [SerializeField] private PoolManager poolManager;
    [SerializeField] private List<GameObject> prefabs = new();

    private SerializedObject windowSerializedObject;
    private SerializedProperty poolManagerProperty;
    private SerializedProperty prefabsProperty;

    private Vector2 scroll;

    [MenuItem("Tools/Pool Register Editor")]
    private static void Open()
    {
        GetWindow<PoolPrefabRegister>("Pool Register");
    }

    private void OnEnable()
    {
        windowSerializedObject = new SerializedObject(this);
        poolManagerProperty = windowSerializedObject.FindProperty(nameof(poolManager));
        prefabsProperty = windowSerializedObject.FindProperty(nameof(prefabs));
    }

    private void OnGUI()
    {
        windowSerializedObject.Update();

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("Pool Manager 설정", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(poolManagerProperty, new GUIContent("Pool Manager"));

        EditorGUILayout.Space(8);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("현재 선택한 Prefab 가져오기"))
            {
                AddSelectedPrefabs();
            }

            if (GUILayout.Button("목록 비우기"))
            {
                prefabs.Clear();
            }
        }

        EditorGUILayout.Space(8);

        DrawDragAndDropArea();

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("등록할 Prefab 목록", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MinHeight(180));
        EditorGUILayout.PropertyField(prefabsProperty, true);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(100);

        using (new EditorGUI.DisabledScope(poolManagerProperty.objectReferenceValue == null))
        {
            if (GUILayout.Button("PoolManager에 등록", GUILayout.Height(32)))
            {
                RegisterPrefabs();
            }
        }

        windowSerializedObject.ApplyModifiedProperties();
    }

    private void AddSelectedPrefabs()
    {
        foreach (Object selectedObject in Selection.objects)
        {
            GameObject prefab = GetPrefabAsset(selectedObject);

            if (prefab == null)
            {
                Debug.LogError($"Prefab을 찾을 수 없습니다: {selectedObject.name}");
                continue;
            }

            if (!prefabs.Contains(prefab))
            {
                prefabs.Add(prefab);
            }
        }
    }

    private void DrawDragAndDropArea()
    {
        Rect dropArea = GUILayoutUtility.GetRect(0, 55, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Project 창의 Prefab 여러 개를 여기에 드래그 & 드롭");

        Event currentEvent = Event.current;

        if (!dropArea.Contains(currentEvent.mousePosition))
            return;

        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                currentEvent.Use();
                break;

            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    GameObject prefab = GetPrefabAsset(draggedObject);

                    if (prefab == null)
                    {
                        Debug.LogError($"Prefab을 찾을 수 없습니다: {draggedObject.name}");
                        continue;
                    }

                    if (!prefabs.Contains(prefab))
                    {
                        prefabs.Add(prefab);
                    }
                }

                currentEvent.Use();
                break;
        }
    }

    private void RegisterPrefabs()
    {
        PoolManager targetPoolManager = poolManagerProperty.objectReferenceValue as PoolManager;

        if (targetPoolManager == null)
        {
            Debug.LogError("PoolManager가 선택되지 않았습니다.");
            return;
        }

        SerializedObject poolManagerSerializedObject = new SerializedObject(targetPoolManager);
        SerializedProperty poolListProperty = poolManagerSerializedObject.FindProperty(PoolListFieldName);

        if (poolListProperty == null || !poolListProperty.isArray)
        {
            Debug.LogError($"PoolManager에서 리스트 필드를 찾을 수 없습니다: {PoolListFieldName}");
            return;
        }

        Undo.RecordObject(targetPoolManager, "Register Pool Prefabs");

        int addedCount = 0;
        int skippedCount = 0;
        int failedCount = 0;

        foreach (GameObject prefabObject in prefabs)
        {
            GameObject prefab = GetPrefabAsset(prefabObject);

            if (prefab == null)
            {
                Debug.LogError($"Prefab을 찾을 수 없습니다: {(prefabObject == null ? "null" : prefabObject.name)}");
                failedCount++;
                continue;
            }

            string poolId = prefab.name;

            if (ContainsPoolId(poolListProperty, poolId))
            {
                Debug.Log($"이미 PoolManager에 등록되어 있습니다: {poolId}");
                skippedCount++;
                continue;
            }

            int newIndex = poolListProperty.arraySize;
            poolListProperty.InsertArrayElementAtIndex(newIndex);

            SerializedProperty newElement = poolListProperty.GetArrayElementAtIndex(newIndex);

            SerializedProperty poolIdProperty = newElement.FindPropertyRelative(PoolIdFieldName);
            SerializedProperty sizeProperty = newElement.FindPropertyRelative(SizeFieldName);
            SerializedProperty prefabProperty = newElement.FindPropertyRelative(PrefabFieldName);

            if (poolIdProperty == null || sizeProperty == null || prefabProperty == null)
            {
                poolListProperty.DeleteArrayElementAtIndex(newIndex);

                Debug.LogError(
                    $"Pool 데이터 필드를 찾을 수 없습니다. " +
                    $"필드명을 확인하세요. " +
                    $"PoolId: {PoolIdFieldName}, Size: {SizeFieldName}, Prefab: {PrefabFieldName}"
                );

                failedCount++;
                continue;
            }

            poolIdProperty.stringValue = poolId;
            sizeProperty.intValue = DefaultSize;
            prefabProperty.objectReferenceValue = prefab;

            addedCount++;

            Debug.Log($"Pool 등록 완료: {poolId}, Size: {DefaultSize}");
        }

        poolManagerSerializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(targetPoolManager);

        Debug.Log($"Pool 등록 작업 완료 / 추가: {addedCount}, 중복 제외: {skippedCount}, 실패: {failedCount}");
    }

    private bool ContainsPoolId(SerializedProperty poolListProperty, string poolId)
    {
        for (int i = 0; i < poolListProperty.arraySize; i++)
        {
            SerializedProperty element = poolListProperty.GetArrayElementAtIndex(i);
            SerializedProperty poolIdProperty = element.FindPropertyRelative(PoolIdFieldName);

            if (poolIdProperty == null)
                continue;

            if (poolIdProperty.stringValue == poolId)
                return true;
        }

        return false;
    }

    private GameObject GetPrefabAsset(Object targetObject)
    {
        if (targetObject == null)
            return null;

        if (targetObject is not GameObject gameObject)
            return null;

        // Project 창에서 직접 선택한 Prefab Asset인 경우
        if (AssetDatabase.Contains(gameObject))
        {
            PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(gameObject);

            if (prefabAssetType != PrefabAssetType.NotAPrefab)
            {
                return gameObject;
            }
        }

        // Scene에 있는 Prefab Instance를 선택한 경우 원본 Prefab을 찾음
        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);

        if (sourcePrefab != null && AssetDatabase.Contains(sourcePrefab))
        {
            return sourcePrefab;
        }

        return null;
    }
}