using UnityEngine;
using UnityEngine.UI;

public class UI_BuildButtons : MonoBehaviour
{
    private UI_Animator uiAnimator;
    [SerializeField] private float yPositionOffset;
    [SerializeField] private float openAnimationDuration = 0.1f;
    public bool isBuildMenuActive;
    private UI_BuildButtonOnHoverEffect[] buildButtons;

    private void Awake()
    {
        uiAnimator = GetComponent<UI_Animator>();
        buildButtons = GetComponentsInChildren<UI_BuildButtonOnHoverEffect>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            ShowBuildButtons();
        }
    }

    public void ShowBuildButtons()
    {
        isBuildMenuActive = !isBuildMenuActive;

        float yOffset = isBuildMenuActive ? yPositionOffset : -yPositionOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0;

        uiAnimator.ChangePosition(transform, new Vector3(0, yOffset), openAnimationDuration);
        Invoke(nameof(ToggleButtonMovement), methodDelay);
    }

    private void ToggleButtonMovement()
    {
        foreach(var button in buildButtons)
        {
            button.ToggleMovement(isBuildMenuActive);
        }
    }
}
