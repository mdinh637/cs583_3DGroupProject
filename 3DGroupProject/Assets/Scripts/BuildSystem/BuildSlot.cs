using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UI ui;
    private Vector3 defaultPosition;
    private BuildManager buildManager;
    private bool tileCanBeMoved = true;
    // create a reference for TileAnimator
    private TileAnimator tileAnim;

    private Coroutine currentMovementUpCo;
    private Coroutine moveToDefaultCo;
    private bool buildSlotAvailable = true;

    
    private void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnim = FindFirstObjectByType<TileAnimator>();
        buildManager = FindFirstObjectByType<BuildManager>();
        defaultPosition = transform.position;
    }

    private void Start()
    {
        if (buildSlotAvailable == false)
        transform.position += new Vector3(0, .1f);
    
    }

    public void SetSlotAvailableTo(bool value) => buildSlotAvailable = value;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(buildSlotAvailable)
        {
            return;
        }
        // limit select only to lmb 
        if(eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if(buildManager.GetSelectedSlot() == this) // disables selecting same build slot multiple times 
        {
            return;
        }

        // log when a tile is clicked
        Debug.Log("Tile was Selected");
        
        buildManager.EnableBuildMenu(); // needs ui 
        buildManager.SelectBuildSlot(this);
        MoveTileUp();
        // stop tile from moving when selected
        tileCanBeMoved = false;

        ui.buildButtonsUI.GetLastSelectedButton()?.SelectButton(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(buildSlotAvailable == false)
        {
            return;
        }
        if(tileCanBeMoved == false)
        {
            return;
        }
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(buildSlotAvailable == false)
        {
            return;
        }
        if(tileCanBeMoved == false)
        {
            return;
        }

        if(currentMovementUpCo != null)
        {
            Invoke(nameof(MoveToDefaultPosition), tileAnim.GetTravelDuration());
        }
        else
        {
            MoveToDefaultPosition();
        }
        
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
        currentMovementUpCo = StartCoroutine(tileAnim.MoveTileCo(transform, targetPosition));
    }

    private void MoveToDefaultPosition()
    {
        moveToDefaultCo = StartCoroutine(tileAnim.MoveTileCo(transform, defaultPosition));
    }

    public void SnapToDefaultPositionImmediately()
    {
        if(moveToDefaultCo != null)
        {
            StopCoroutine(moveToDefaultCo);
        }
        transform.position = defaultPosition;
    }

        public Vector3 GetBuildPosition(float yOffset) => defaultPosition + new Vector3(0, yOffset);
}
