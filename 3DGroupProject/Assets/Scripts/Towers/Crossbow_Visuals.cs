using UnityEngine;
using System.Collections;

public class Crossbow_Visuals : MonoBehaviour
{
    private Tower_Crossbow crossbowTower;
    private Enemy myEnemy;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;

    private void Update()
    {
        UpdateAttackVisualsIfNeeded();
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && myEnemy != null)
        {
            //update the end point of the attack visual to follow the enemy if it's moving
            attackVisuals.SetPosition(1, myEnemy.CenterPoint());
        }
    }

    private void Awake()
    {
        crossbowTower = GetComponent<Tower_Crossbow>();
    }

    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint, newEnemy));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        //crossbowTower.EnableRotation(false); //disable rotation while attack visual is playing
        myEnemy = newEnemy; //store current enemy to track during visual

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        attackVisuals.enabled = false;

        //crossbowTower.EnableRotation(true); //re-enable rotation after attack visual is done
    }
}
