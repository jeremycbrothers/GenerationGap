using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public event EventHandler Die;

    public int startingHealth = 5;
    public int currentHealth;
    public Slider SDHealth;


    public void TakeDamage(int amount){
        currentHealth -= amount;
        SDHealth.value = currentHealth;


    }


    public int health{
        get{
            return _health;
        }

        set{
            if (value <= 0)
                OnDie(new EventArgs());

            _health = value;
            Debug.Log(name + "'s health is now " + _health);
        }
    }

    private int _health;

    void OnStart(){
        currentHealth = startingHealth;
    }

    private void OnDie(EventArgs e){
        EventHandler handler = Die;
        if (handler != null)
        {
            handler(this, e);
        }
    }
}
