using UnityEngine;

/*
Basic archer tower
Things to consider: atk range, atk spd, dmg

Get back to this after implementing enemies cuz rn just a shell
*/
public class ArcherAttackLogic : MonoBehaviour
{
    //variables for archer tower stats
    public int projectileDmg = 1; //dmg per arrow
    public float atkRange = 10f; //atk range of turret
    public float atkSpd = 1f; //atk spd of turret per sec
    private float atkCd = 0f; //cooldown timer after each atk

    void Update()
    {
        atkCd -= Time.deltaTime; //cooldown timer decrement
    }

    void OnDrawGizmosSelected()
    {
        //logic taken from my 2d game, gives visual of what would be atk range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
