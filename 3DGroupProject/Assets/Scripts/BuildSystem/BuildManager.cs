using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private UI ui; // needs ui
    public BuildSlot selectedBuildSlot;

    private void Awake()
    {
        ui = FindFirstObjectByType<UI>(); // needs ui 
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
            // if clicked outside that is not on a build slot, cancel the build action ==> unselect tile and exit build menu
            if(Physics.RayCast(Camera.main.ScrenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                bool clickedNotOnBuildSlot = hit.collider.GetComponent<BuildSlot>() == null;

                if(clickedNotOnBuildSlot)
                {
                    CancelBuildAction();
                }
            }
        }
    }

    private void CancelBuildAction()
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
        if(selectedBuildSlot != null)
        {
            return;
        }

        ui.buildButtonsUI.ShowBuildButtons(true);
    }

    private void DisableBuildMenu()
    {
        ui.buildButtonsUI.ShowBuildButtons(false);
    }

    public BuildSlot GetSelectedSlot() => selectedBuildSlot;

}
