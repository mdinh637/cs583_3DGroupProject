using UnityEngine;
using System.Collections;

public class Crossbow_Visuals : MonoBehaviour
{
    [Header("Attack Vfx")]
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;
    private Vector3 hitPoint;

    private void Update()
    {
        UpdateAttackVisualsIfNeeded();
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && hitPoint != Vector3.zero)
        {
            //update the end point of the attack visual to follow the enemy if it's moving
            attackVisuals.SetPosition(1, hitPoint);
        }
    }

    private void Awake()
    {
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        hitPoint = endPoint; //store hit point to update during visual
        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisuals.enabled = false;
    }
}
