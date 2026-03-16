using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;
    public GameObject source;
    void Start()
    {
        StartCoroutine(DeathTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<BattlerAgent>(out BattlerAgent agent) && other.gameObject != source)
        {
            agent.SetReward(-1.0f);
            source.GetComponent<BattlerAgent>().RewardSet(+1f);
            Destroy(gameObject);
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            source.GetComponent<BattlerAgent>().RewardSet(-0.2f);
            Destroy(gameObject);
        }
    }
    private void DestroySelf(){

    }
    private IEnumerator DeathTime(){
        yield return new WaitForSeconds(3f);
        source.GetComponent<BattlerAgent>().RewardSet(-0.2f);
        Destroy(gameObject);
    }
}
