using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;
    public int friendlyFire = 1; //1 is enemy only, 0 is enemy/ally, -1 is ally only. 
    public int damageAmount;
    public int pierceAmount;
    public GameObject source;
    public AbilityData abilityEffect;
    public bool hitTarget;
    void OnEnable(){
        SharedEvents.startRound += DestroySelf;
    }
    void OnDisable(){
        SharedEvents.startRound -= DestroySelf;
    }
    void Start()
    {
        StartCoroutine(DeathTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<BattlerAgent>(out BattlerAgent agent) && other.gameObject != source)
        {
            //DamagePackage damage = new DamagePackage(source, other.gameObject, damageAmount);
            foreach(var effect in abilityEffect.effects){
                effect.Execute(source, other.gameObject);
            }
            if(other.GetComponent<UnitStats>().align != source.GetComponent<UnitStats>().align){
                hitTarget = true;
            }
            pierceAmount--;
            if(pierceAmount <= 0){
                DestroySelf();
            }
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            //source.GetComponent<BattlerAgent>().RewardAdd(-0.02f);
            if(pierceAmount < 100){
                DestroySelf();
            }
            
        }
    }
    public void UpdateAbilityEffect(AbilityData data){
        abilityEffect = data;
    }
    public void AssignDamage(int amount){
        damageAmount = amount;
    }
    private void DestroySelf(){
        if(hitTarget == true){
            source.GetComponent<BattlerAgent>().RewardAdd(0.1f);
            SharedEvents.DidAbilityHit(abilityEffect.label, false);
        }else{
            source.GetComponent<BattlerAgent>().RewardAdd(-0.03f);
            SharedEvents.DidAbilityHit(abilityEffect.label, true);
        }
        Destroy(this.gameObject);
    }
    private IEnumerator DeathTime(){
        yield return new WaitForSeconds(lifetime);
        DestroySelf();
    }
    public Alignment TellAlignment(){
        return source.GetComponent<UnitStats>().align;
    }
}
