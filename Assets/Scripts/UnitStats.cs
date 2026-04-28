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
    
    public BattlerAgent agent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<BattlerAgent>();
        ResetStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(DamagePackage pack){
        int storeHP = currentHP;
        currentHP -= pack.amount;
        if(currentHP > maxHP){
            currentHP = maxHP;
        }
        DamagePackage damageTaken = new DamagePackage(pack.source, pack.target, pack.amount + currentHP);
        damageTaken.damagePercent = (float) (currentHP - storeHP)/maxHP;
        SharedEvents.AnnounceDamage(damageTaken);
        CheckDeath();
        if(isDead){
            pack.source.GetComponent<BattlerAgent>().AddReward(50f);
        }
    }
    public void CheckDeath(){
        if(currentHP <= 0){
            isDead = true;
            agent.ToggleDeath(isDead);
            agent.AddReward(-50f);
        }
    }

    public void ResetStats(){
        ResetHP();
        isDead = false; 
        agent.ToggleDeath(isDead);
    }
    public void ResetHP(){
        currentHP = maxHP;
    }
}
