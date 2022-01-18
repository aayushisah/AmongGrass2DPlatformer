using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Damagable : MonoBehaviour
{
    public UnityEvent deathEvent;
   

    public void OnDeath()
    {
        Debug.Log("ded");
        deathEvent.Invoke();

    }

    
}
