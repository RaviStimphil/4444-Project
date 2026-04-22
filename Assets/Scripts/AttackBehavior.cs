using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AttackBehavior", menuName = "ScriptableObjects/AttackBehavior")]
public class AttackBehavior : ScriptableObject
{
    public GameObject ammoInfo;
    public GameObject source;

    public bool isDash;
    public float attackDuration;
    public float attackSpeed;
    public int pierceAmount = 1;

    public float maxRewardRange;
    public float minRewardRange;

    [SerializeField] AbilityData abilityEffect;

    public float castTime;
    public float endTime;
    public float cooldown;

    // --- Runtime State ---
    private float timer = 0f;

    private enum AttackState
    {
        Idle,
        Casting,
        EndLag,
        Cooldown
    }

    private AttackState currentState = AttackState.Idle;

    private BattlerAgent battler;

    // Call this once when assigning
    public void Initialize(GameObject sourceObj)
    {
        source = sourceObj;
        battler = source.GetComponent<BattlerAgent>();
    }

    // Call this every frame (from Agent or MonoBehaviour)
    public void Tick(float deltaTime)
    {
        if (currentState == AttackState.Idle) return;

        timer -= deltaTime;

        switch (currentState)
        {
            case AttackState.Casting:
                if (timer <= 0f)
                {
                    FireProjectile();
                    currentState = AttackState.EndLag;
                    timer = endTime;
                }
                break;

            case AttackState.EndLag:
                if (timer <= 0f)
                {
                    battler.ToggleActing(true);
                    currentState = AttackState.Cooldown;
                    timer = cooldown;
                }
                break;

            case AttackState.Cooldown:
                if (timer <= 0f)
                {
                    currentState = AttackState.Idle;
                }
                break;
        }
    }

    public void DoAction()
    {
        if (currentState != AttackState.Idle) return;

        battler.ToggleActing(false);
        currentState = AttackState.Casting;
        timer = castTime;
    }

    private void FireProjectile()
    {
        GameObject attack = Instantiate(
            ammoInfo,
            battler.firePoint.position,
            source.transform.rotation
        );

        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        rb.AddForce(attack.transform.up * attackSpeed, ForceMode2D.Impulse);

        Projectile ammo = attack.GetComponent<Projectile>();
        ammo.lifetime = attackDuration;
        ammo.source = source;
        ammo.pierceAmount = pierceAmount;
        ammo.UpdateAbilityEffect(abilityEffect);
    }

    public bool IsReady()
    {
        return currentState == AttackState.Idle;
    }
}