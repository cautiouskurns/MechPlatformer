using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private DamageCalculator damageCalculator;
    
    private void Awake()
    {
        // Create damage calculator if not assigned
        if (damageCalculator == null)
        {
            damageCalculator = new DamageCalculator();
        }
    }
    
    public int ApplyDamage(IAttacker attacker, IDamageable target)
    {
        // Calculate damage using the calculator
        int rawDamage = attacker.GetAttackDamage();
        int finalDamage = damageCalculator.CalculateDamage(rawDamage);
        
        // Apply damage to target
        target.TakeDamage(finalDamage);
        
        return finalDamage;
    }
}

[System.Serializable]
public class DamageCalculator
{
    [SerializeField] private float damageMultiplier = 1.0f;
    
    public int CalculateDamage(int rawDamage)
    {
        // Apply simple multiplier for now
        // Later this could include defense values, critical hits, etc.
        return Mathf.RoundToInt(rawDamage * damageMultiplier);
    }
}
