using UnityEngine;

#region Soldier
public class SoldierFeatures : MonoBehaviour
{
    public enum Soldiers { Soldier1, Soldier2, Soldier3 };
    public Soldiers soldiers;
    public int health { get; private set; }
    public int damage { get; private set; }

    public bool canAttack { get; private set; }

    private void Awake()
    {
        SoldierSelector();
    }

    void SoldierSelector()
    {
        switch (soldiers)
        {
            case Soldiers.Soldier1: health = 10; damage = 10; break;
            case Soldiers.Soldier2: health = 10; damage = 5; break;
            case Soldiers.Soldier3: health = 10; damage = 2; break;
        }
    }
}
#endregion

#region Building
public class BuildingFeatures : MonoBehaviour
{
    public enum Buildings { Barrack, Power_Plant, Soldier_Units}
    public Buildings buildings;

    public int health;

    private void Awake()
    {
        BuildingSelector();
    }

    public void BuildingSelector()
    {
        switch (buildings) 
        {
            case Buildings.Barrack: health = 100; break;
            case Buildings.Power_Plant: health = 50; break;
            case Buildings.Soldier_Units: health = 20; break;
        }
    }
}
#endregion
