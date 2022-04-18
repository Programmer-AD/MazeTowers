using System;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [NonSerialized] public TowerManager towerManager;
    [NonSerialized] public int Level;
    [NonSerialized] public Tower tower;

    private void OnMouseUpAsButton()
    {
        towerManager.UpdateSlot(this);
    }
}
