using UnityEngine;
using System.Collections;

public class Crossbow_Visuals : MonoBehaviour
{
    private Tower_Crossbow crossbowTower;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;

    private void Awake()
    {
        crossbowTower = GetComponent<Tower_Crossbow>();
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        crossbowTower.EnableRotation(false); //disable rotation while attack visual is playing

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisuals.enabled = false;

        crossbowTower.EnableRotation(true); //re-enable rotation after attack visual is done
    }
}
