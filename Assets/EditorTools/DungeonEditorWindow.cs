using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class DungeonEditorWindow : EditorWindow
{
    public int width = 20, height = 10;
    private RadioButtonGroup tileSelect;
    private Slider EnemySpawnPercentage;
    private Slider ItemSpawnPercentage;
    private Slider KeySpawnPercentage;
    private List<List<Button>> buttons;
    private TileType[][] map;

    public enum TileType
    {
        Normal, Lava, Box, Pit
    }

    [MenuItem("Window/UI Toolkit/Dungeon Editor")]
    public static void ShowExample()
    {
        DungeonEditorWindow wnd = GetWindow<DungeonEditorWindow>();
        wnd.titleContent = new GUIContent("Dungeon Editor");
    }

    public void CreateGUI()
    {
        buttons = new List<List<Button>>();

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorTools/DungeonEditorWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorTools/DungeonEditorWindow.uss");

        tileSelect = rootVisualElement.Q<RadioButtonGroup>("tileSelect");
        EnemySpawnPercentage = rootVisualElement.Q<Slider>("RoomEnemiesPercent");
        ItemSpawnPercentage = rootVisualElement.Q<Slider>("RoomItemPercent");
        KeySpawnPercentage = rootVisualElement.Q<Slider>("RoomKeyPercent");

        EnemySpawnPercentage.value = 0;
        ItemSpawnPercentage.value = 0;
        KeySpawnPercentage.value = 0;

        GenerateGrid();

        root.Q<Button>("save").clicked += () =>
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogError("Select a Dungeon Room.");
                return;
            }
            DungenRoom room = Selection.activeGameObject.GetComponent<DungenRoom>();
            if (room == null)
                Debug.LogError(Selection.activeGameObject.name + " does not have a Dungeon Room component.");
            else
            {
                room.GenerateMislanious(map, (int)KeySpawnPercentage.value, (int)EnemySpawnPercentage.value, (int)ItemSpawnPercentage.value);
                room.GenerateRoom(map);
            }
        };
        root.Q<Button>("load").clicked += () =>
        {
        };

        root.Q<DropdownField>("roomSize").RegisterValueChangedCallback(e =>
        {
            switch (e.newValue.ToLower())
            {
                case "normal":
                    width = 20;
                    height = 10;
                    break;
                case "big":
                    width = 20*2;
                    height = 10*2;
                    break;
                default:
                    width = 1;
                    height = 1;
                    break;
            }
            GenerateGrid();
        });
        /*DungeonEditorManipulator keymanip1 = new DungeonEditorManipulator(rootVisualElement.Q<VisualElement>("key1"), rootVisualElement.Q<VisualElement>("key1Slot"));
        DungeonEditorManipulator keymanip2 = new DungeonEditorManipulator(rootVisualElement.Q<VisualElement>("key2"), rootVisualElement.Q<VisualElement>("key2Slot"));
        DungeonEditorManipulator keymanip3 = new DungeonEditorManipulator(rootVisualElement.Q<VisualElement>("key3"), rootVisualElement.Q<VisualElement>("key3Slot"));
        */
    }

    public void GenerateGrid()
    {
        buttons.Clear();

        map = new TileType[height][];
        for (int y = 0; y < height; y++)
        {
            map[y] = new TileType[width];
            for (int x = 0; x < width; x++)
                map[y][x] = TileType.Normal;
        }

        VisualElement grid = rootVisualElement.Q<VisualElement>("slots");

        if (grid.childCount > 0)
            grid.Clear();

        for (int h = 0; h < height; h++)
        {
            buttons.Add(new List<Button>());
            VisualElement row = new VisualElement();
            row.name = "slot_row" + (h + 1);
            row.AddToClassList("slot_row");
            grid.Add(row);
            for (int w = 0; w < width; w++)
            {
                int tempH = h;
                int tempW = w;
                Button slot = new Button();
                slot.name = "slot" + (w + 1);
                slot.AddToClassList("slot");
                row.Add(slot);
                buttons[h].Add(slot);
                //buttons[h].Add(row.Q<Button>("slot" + (w + 1)));
                buttons[h][w].clicked += () => { SetTile(tempW, tempH); };
            }
        }
    }

    public void SetTile(int x, int y)
    {
        switch (tileSelect.value)
        {
            case 0:
                map[y][x] = TileType.Normal;
                buttons[y][x].style.backgroundColor = Color.white;
                break;
            case 1:
                map[y][x] = TileType.Lava;
                buttons[y][x].style.backgroundColor = Color.red;
                break;
            case 2:
                map[y][x] = TileType.Box;
                buttons[y][x].style.backgroundColor = Color.gray;
                break;
            case 3:
                map[y][x] = TileType.Pit;
                buttons[y][x].style.backgroundColor = Color.black;
                break;
            default:
                break;
        }
    }
}