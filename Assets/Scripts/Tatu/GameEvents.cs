using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    void Awake()
    {
        current = this;
    }

    public event Action onPainKillerConsumed;
    public void PainKillerConsumed()
    {
        if(onPainKillerConsumed != null)
        {
            onPainKillerConsumed();
        }
    }

    public event Action onCoffeePackPickedUp;
    public void CoffeePackPickedUp()
    {
        if(onCoffeePackPickedUp != null)
        {
            onCoffeePackPickedUp();
        }
    }

    public event Action<Vector3, Transform> onCollisionSound;
    public void OnCollisionSound(Vector3 force, Transform impactTransform)
    {
        if(onCollisionSound != null)
        {
            onCollisionSound(force, impactTransform);
        }
    }

    public event Action<TriggerVolumeID> onTriggerVolumeExit;
    public void OnTriggerVolumeExit(TriggerVolumeID sendersID)
    {
        if(onTriggerVolumeExit != null)
        {
            onTriggerVolumeExit(sendersID);
        }
    }

    public event Action<string> onShowerEnter;
    public void OnShowerEnter(string sendersID)
    {
        if(onShowerEnter != null)
        {
            onShowerEnter(sendersID);
        }
    }
    public event Action onGrillFoodEaten;
    public void OnGrillFoodEaten()
    {
        if(onGrillFoodEaten != null)
        {
            onGrillFoodEaten();
        }
    }

}
