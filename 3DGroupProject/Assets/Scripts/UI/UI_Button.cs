using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_Animator uiAnim;
    private RectTransform myRect;

    [SerializeField] private float showcaseScale = 1.1f;
    [SerializeField] private float scaleUpDuration = 0.2f;

    private Coroutine scaleCoroutine;
    
    private void Awake()
    {
        uiAnim = GetComponent<UI_Animator>();
        myRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
    
        scaleCoroutine = StartCoroutine(uiAnim.ChangeScaleCo(myRect, showcaseScale, scaleUpDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        
        scaleCoroutine = StartCoroutine(uiAnim.ChangeScaleCo(myRect, 1, scaleUpDuration));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myRect.localScale = new Vector3(1, 1, 1);
    }
}