using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private UI ui; // needs ui
    public BuildSlot selectedBuildSlot;

    public WaveManager waveManager;
    public GridBuilder currentGrid;

    [Header("Build Materials")]
    [SerializeField] private Material attackRadiusMat;
    [SerializeField] private Material buildPreviewMat;
    
    private bool isMouseOverUI;

    private void Awake()
    {
        ui = FindFirstObjectByType<UI>(); // needs ui 
        ui.buildButtonsUI.ShowBuildButtons();
    }
    // requires ui
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildAction();
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isMouseOverUI)
            {
                return;
            }

            // if clicked outside that is not on a build slot, cancel the build action ==> unselect tile and exit build menu
            if(Physics.Raycast(Player.main.ScrenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                bool clickedNotOnBuildSlot = hit.collider.GetComponent<BuildSlot>() == null;

                if(clickedNotOnBuildSlot)
                {
                    CancelBuildAction();
                }
            }
        }
    }

    public void MouseOverUI(bool isOverUI) => isMouseOverUI = isOverUI;

// cant place in tile that will be dynamically changed to a road
    public void MakeBuildSlotNotAvailableIfNeeded(WaveManager waveManager, GridBuilder currentGrid)
    {
        foreach (var wave in waveManager.GetLevelWaves())
        {
            if (wave.nextGrid == null)
            {
                continue;
            }
                

            List<GameObject> grid = currentGrid.GetTileSetup();
            List<GameObject> nextWaveGrid = wave.nextGrid.GetTileSetup();

            //compare the current grid and next grids 
            for (int i = 0; i < grid.Count; i++)
            {
                TileSlot currentTile = grid[i].GetComponent<TileSlot>();
                TileSlot nextTile = nextWaveGrid[i].GetComponent<TileSlot>();

                bool tileNotTheSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                                    currentTile.GetMaterial() != nextTile.GetMaterial() ||
                                    currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;

                if (tileNotTheSame == false)
                {
                    continue;
                }
                    

                BuildSlot buildSlot = grid[i].GetComponent<BuildSlot>();

                if (buildSlot != null)
                {
                    buildSlot.SetSlotAvailableTo(false);
                }
                    
            }

        }
    }

    public void CancelBuildAction()
    {
        if(selectedBuildSlot == null)
        {
            return;
        }

        selectedBuildSlot.UnselectTile();
        selectedBuildSlot = null;
        DisableBuildMenu();
    }

    public void SelectBuildSlot(BuildSlot newSlot)
    {
        if(selectedBuildSlot != null)
        {
            selectedBuildSlot.UnselectTile();
        }
        
        selectedBuildSlot = newSlot;
    }

    // needs ui
    public void EnableBuildMenu()
    {
        // only bring up menu if no tile is selected
        if(selectedBuildSlot != null)
        {
            return;
        }

        ui.buildButtonsUI.ShowBuildButtons(true); // added to UI script -> buildButtonsUI = GetComponentInChildren<UI_BuildButtons> in Awake() 
    }

    private void DisableBuildMenu()
    {
        ui.buildButtonsUI.ShowBuildButtons(false);
    }

    public BuildSlot GetSelectedSlot() => selectedBuildSlot;
    public Material GetAttackRadiusMat() => attackRadiusMat;
    public Material GetBuildPreviewMat() => buildPreviewMat;

}
