using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Vector3 defaultPosition;
    private BuildManager buildManager;
    private bool tileCanBeMoved = true;
    // create a reference for TileAnimator
    private TileAnimator tileAnim;
    private void Awake()
    {
        tileAnim = FindFirstObjectByType<TileAnimator>();
        buildManager = FindFirstObjectByType<BuildManager>();
        defaultPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // log when a tile is clicked
        Debug.Log("Tile was Selected");

        buildManager.SelectBuildSlot(this);
        MoveTileUp();
        // stop tile from moving when selected
        tileCanBeMoved = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tileCanBeMoved == false)
        {
            return;
        }
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tileCanBeMoved == false)
        {
            return;
        }
        MoveToDefaultPosition();
    }

    public void UnselectTile()
    {
        MoveToDefaultPosition();
        tileCanBeMoved = true;
    }

    // move tile up when pointer on it
    private void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, tileAnim.GetBuildOffset(), 0);
        tileAnim.MoveTile(transform, targetPosition);
    }

    private void MoveToDefaultPosition()
    {
        tileAnim.MoveTile(transform, defaultPosition);
    }
}
