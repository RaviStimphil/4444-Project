using UnityEngine;

public enum Alignment{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount){
        currentHP -= amount;
    }
}
