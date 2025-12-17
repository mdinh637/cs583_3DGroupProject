using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]
public class TileSlotEditor : Editor
{
    public GUIStyle centeredStyle;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 14
        };

        float oneButtonWidth = (EditorGUIUtility.currentViewWidth - 25);
        float twoButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
        float threeButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 3;

        GUILayout.Label("Position and Rotation", centeredStyle);

        // Buttons for rotation
        GUILayout.BeginHorizontal();

        // Rotation buttons
        if (GUILayout.Button("Rotate Left", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).RotateTile(-1);
            }
        }

        if (GUILayout.Button("Rotate Right", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).RotateTile(1);
            }
        }

        GUILayout.EndHorizontal();
        
        // Buttons for positioning
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("-0.1f on the Y", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).AdjustY(-1);
            }
        }

        if (GUILayout.Button("+0.1f on the Y", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).AdjustY(1);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Basic Tile Options", centeredStyle);
        // Buttons for Field and Road tiles
        GUILayout.BeginHorizontal();

        // Field tile button
        if (GUILayout.Button("Field", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileField;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Road tile button
        if (GUILayout.Button("Road", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileRoad;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        // Button for Sideway tile
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Sideway", GUILayout.Width(oneButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileSideway;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Corner Options", centeredStyle);

        // Buttons for Corner tiles
        GUILayout.BeginHorizontal();

        // Button for Inner Corner tile
        if (GUILayout.Button("Inner Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileInnerCorner;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for Outer Corner tile
        if (GUILayout.Button("Outer Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileOuterCorner;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        // Buttons for Small Corner tiles
        GUILayout.BeginHorizontal();

        // Button for Small Inner Corner tile
        if (GUILayout.Button("Small Inner Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileInnerCornerSmall;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for Small Outer Corner tile
        if (GUILayout.Button("Small Outer Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileOuterCornerSmall;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Bridges and Hills", centeredStyle);

        // Buttons for Bridges and Hills
        GUILayout.BeginHorizontal();

        // Button for Hill tile 1
        if (GUILayout.Button("Hill 1", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_1;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for Hill tile 2
        if (GUILayout.Button("Hill 2", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_2;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for Hill tile 3
        if (GUILayout.Button("Hill 3", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileHill_3;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        // Buttons for bridge tiles
        GUILayout.BeginHorizontal();

        // Button for field bridge tile
        if (GUILayout.Button("Bridge with road", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeField;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for road bridge tile 
        if (GUILayout.Button("Bridge with field", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeRoad;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        // Button for sideway bridge tile
        if (GUILayout.Button("Bridge with sideway", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().tileBridgeSideway;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();
    }
}
