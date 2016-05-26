using System;
using UnityEngine;
using System.Collections;

public class UnitHealth : MonoBehaviour {

    public delegate void HealthDecresed(int current, int max);
    public event HealthDecresed OnHealthDecresed;


    void Start () {
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

        if (OnHealthDecresed != null) {
            OnHealthDecresed(Health, MaxHealth);
        }

        return true;
    }

    void Update() {
        TakeDamage(1);
    }
}
