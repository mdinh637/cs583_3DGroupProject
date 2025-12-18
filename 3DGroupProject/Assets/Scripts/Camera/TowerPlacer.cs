using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private GameObject[] towerPrefabs; //array of tower prefabs to place
    private int selectedTowerIndex = -1; //-1 means no tower selected

    [Header("Preview Colors")]
    [SerializeField] private Color validColor = new Color(0f, 1f, 0f, 0.5f); //green for valid placement
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.5f); //red for invalid placement

    [Header("Placement Cooldowns")]
    [SerializeField] private float[] towerCooldowns; //seconds per tower
    private float[] towerCooldownTimers; //timers for each tower

    [SerializeField] private LayerMask groundLayer; //layer for ground where towers can be placed
    [SerializeField] private LayerMask towerLayer; //layer for towers to check distance
    [SerializeField] private float minTowerDistance = 2f; //minimum distance between towers

    private GameObject previewTower; //ghost tower following cursor

    private void Awake()
    {
        towerCooldownTimers = new float[towerCooldowns.Length]; //initialize cooldown timers array
    }

    void Update()
    {
        //do not place or delete towers when clicking UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        //update cooldown timers
        for (int i = 0; i < towerCooldownTimers.Length; i++) //for each tower type
        {
            //if timer > 0, reduce it
            if (towerCooldownTimers[i] > 0)
                towerCooldownTimers[i] -= Time.deltaTime; //reduce timer
        }

        //right click deletes tower
        if (Input.GetMouseButtonDown(1))
        {
            TryDeleteTower();
        }

        //if a tower is selected, move preview with mouse
        if (previewTower != null)
        {
            UpdatePreviewPosition();

            //left click places tower
            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceTower();
            }
        }
    }

    //called by UI buttons to select/deselect tower
    public void SelectTower(int index)
    {
        //clicking the same button again deselects
        if (selectedTowerIndex == index)
        {
            CancelPlacement();
            return;
        }

        selectedTowerIndex = index;

        //remove old preview if it exists
        if (previewTower != null)
        {
            Destroy(previewTower);
        }

        //create new preview tower
        previewTower = Instantiate(towerPrefabs[selectedTowerIndex]);
        SetPreviewMode(previewTower);
    }

    //cancel current placement
    private void CancelPlacement()
    {
        selectedTowerIndex = -1;

        if (previewTower != null)
        {
            Destroy(previewTower);
            previewTower = null;
        }
    }

    //update preview tower position based on mouse
    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ray from camera through mouse position

        //raycast against everything
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            //if we hit path first, make placement invalid
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Path"))
            {
                SetPreviewColor(invalidColor);
                return;
            }

            //only allow placement if we hit ground
            if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
            {
                Vector3 pos = hit.point;
                pos.y += GetPlacementYOffset();
                previewTower.transform.position = pos;

                bool canPlace = CanPlaceHere(pos);

                //safety check for cooldown array
                if (selectedTowerIndex < 0 || selectedTowerIndex >= towerCooldownTimers.Length)
                {
                    SetPreviewColor(invalidColor);
                    return;
                }

                //if tower is on cooldown, force invalid
                if (towerCooldownTimers[selectedTowerIndex] > 0f)
                {
                    SetPreviewColor(invalidColor);
                }
                else
                {
                    SetPreviewColor(canPlace ? validColor : invalidColor);
                }

            }
            else
            {
                SetPreviewColor(invalidColor); //not ground, invalid placement
            }
        }
    }

    //set preview tower color
    private void SetPreviewColor(Color color)
    {
        Renderer[] renderers = previewTower.GetComponentsInChildren<Renderer>(); //get all renderers in preview tower

        //for each renderer, set color of all materials
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                m.color = color;
            }
        }
    }

    private float GetPlacementYOffset()
    {
        return 1.5f; //adjust as needed based on tower model height
    }

    //try to place tower at preview position
    private void TryPlaceTower()
    {
        //safety check
        if (selectedTowerIndex < 0 || selectedTowerIndex >= towerPrefabs.Length)
            return;

        Vector3 placementPos = previewTower.transform.position;

        //block placement if this tower is on cooldown
        if (towerCooldownTimers[selectedTowerIndex] > 0f)
            return;

        //if valid placement, instantiate tower
        if (CanPlaceHere(placementPos))
        {
            Instantiate(towerPrefabs[selectedTowerIndex], placementPos, Quaternion.identity);

            //start cooldown for this tower type
            towerCooldownTimers[selectedTowerIndex] = towerCooldowns[selectedTowerIndex];

            //after placing, cancel placement
            CancelPlacement();
        }
    }

    //check if tower can be placed at given position
    private bool CanPlaceHere(Vector3 position)
    {
        Collider[] nearbyTowers = Physics.OverlapSphere(position, minTowerDistance, towerLayer);
        return nearbyTowers.Length == 0;
    }

    //delete tower on right click
    private void TryDeleteTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    //make preview tower non-interactive
    private void SetPreviewMode(GameObject tower)
    {
        //disable tower scripts so preview doesn't attack before placement
        Tower towerScript = tower.GetComponent<Tower>();
        if (towerScript != null)
        {
            towerScript.enabled = false;
        }

        //disable all colliders so it doesn't block placement
        Collider[] colliders = tower.GetComponentsInChildren<Collider>();

        //disable each collider
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        //start preview as invalid (red)
        Renderer[] renderers = tower.GetComponentsInChildren<Renderer>();

        //set all materials to invalid color
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                m.color = invalidColor;
            }
        }
    }
}
