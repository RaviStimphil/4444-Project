using UnityEngine;

public enum Alignment{
    Zearo,
    Firast,
    Seacond,
    Thiard
}
public class UnitStats : MonoBehaviour
{
    public Alignment align;
    public int maxHP;
    public int currentHP;
    public bool isDead;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //poison
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(DamagePackage pack){
        int storeHP = currentHP;
        currentHP -= pack.amount;
        DamagePackage damageTaken = new DamagePackage(pack.source, pack.target, pack.amount + currentHP);
        damageTaken.damagePercent = (float) (currentHP - storeHP)/maxHP;
        SharedEvents.AnnounceDamage(damageTaken);
        CheckDeath();
    }
    public void CheckDeath(){
        if(currentHP <= 0){
            isDead = true;

        }
    }
}
