using UnityEngine;

public class EventAgent : MonoBehaviour
{
    public BattlerAgent agent;
    void Awake()
    {
        agent = GetComponent<BattlerAgent>();
    }
    void OnEnable(){
        SharedEvents.damageTaken += RewardForDamage;
        SharedEvents.unitDeath += RewardForDeath;
    }
    void OnDisable(){
        SharedEvents.damageTaken -= RewardForDamage;
        SharedEvents.unitDeath -= RewardForDeath;
    }

    public void RewardForDeath(GameObject unit){
        Alignment deadAlign = unit.gameObject.GetComponent<UnitStats>().align;
        Alignment agentAlign = this.gameObject.GetComponent<UnitStats>().align;
        if(unit == this.gameObject){
            agent.PendingRewardAdd(-2f);
        }
        else if(deadAlign == agentAlign){
            agent.PendingRewardAdd(-1f);
        }
        else{
            agent.PendingRewardAdd(1f);
        }
    }
    public void RewardForDamage(DamagePackage pack){
        //In case I want to make deployables... I really shouldn't.
        float rewardMultiplier = 1f; 
        //Take care of friendly fire or ally healing first.
        //Damage comes in negative.
        if(pack.TargetAlignment() == pack.SourceAlignment()){
            if(pack.TargetAlignment() == this.gameObject.GetComponent<UnitStats>().align){
                if(pack.source == this.gameObject){
                    agent.PendingRewardAdd(pack.damagePercent * 3f); //If the agent does the action, it gets more reward.
                    //If the agent did 30% of an allied agent HP, it is sent as -0.3
                    //-0.3 * 3 = -0.9 reward. Agent should learn not to damage ally.
                }else{
                    agent.PendingRewardAdd(pack.damagePercent); //If the agent didn't do the action, it gets some, but not as much.
                }
            }else{
                agent.PendingRewardAdd(-pack.damagePercent); //If the agent is not involved, it gets its reward. 
                
            }
        }
        //If the agent or allies is the target, negative reward if taking damage, positive vince vesa.
        else if(pack.TargetAlignment() == this.gameObject.GetComponent<UnitStats>().align){
            if(pack.target == this.gameObject){
                agent.PendingRewardAdd(pack.damagePercent * 3f);
                //Agent should learn to avoid damage. 
            }else{
                agent.PendingRewardAdd(pack.damagePercent);
            }
        }
        //Allied agents and self dealing damage is good. 
        else if(pack.SourceAlignment() == this.gameObject.GetComponent<UnitStats>().align){
            if(pack.source == this.gameObject){
                agent.PendingRewardAdd(-pack.damagePercent * 3f);
            }else{
                agent.PendingRewardAdd(-pack.damagePercent);
            }
        }
        //If there's a third faction for some reason... 
        //Enemies attacking each other is good. 
        else{
            agent.PendingRewardAdd(-pack.damagePercent);
        }
    }
}
