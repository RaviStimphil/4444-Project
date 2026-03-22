using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject source;
    public float projectileSpeed = 10f;
    public float attackCooldown = 1f;
    public bool canAttack = true;

    void Start()
    {
        source = this.gameObject;
    }
    public void Shoot(Vector2 direction, Transform point)
    {
        if(canAttack){
            GameObject proj = Instantiate(projectilePrefab, point.position, Quaternion.identity);
            proj.GetComponent<Projectile>().source = source;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction.normalized * projectileSpeed;
            //source.GetComponent<BattlerAgent>().RewardSet(+0.1f);  
            StartCoroutine(AttackWait());  
        }else{
            //source.GetComponent<BattlerAgent>().RewardSet(-0.1f);
        }
       
    }
    private IEnumerator AttackWait(){
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    } 
}
