using UnityEngine;

public class UnitHealth : MonoBehaviour {
    public delegate void HealthDecresed(int current, int max);
    public event HealthDecresed OnHealthChanged;

    void OnEnable() {
        Health = MaxHealth;
    }

    public int Health { get; private set; }
    public int MaxHealth;

    public bool TakeDamage(int damage) {
        if (Health <= 0) {
            return false;
        }

        Health -= damage;

        if (Health < 0) {
            Health = 0;
        }

        if (OnHealthChanged != null) {
            OnHealthChanged(Health, MaxHealth);
        }

        return true;
    }
}
