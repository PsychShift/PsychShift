using Guns;
using UnityEngine;

[System.Serializable]

public class SaveObject
{
  public int Scene;
  public Vector3 Savepoint;
  public int Health;
  public AIAgression AIAgression;
  public EBrainType EnemyType;
  public GunType GunType;
  public EEnemyModifier[] Modifiers;

  public SaveObject(int scene, Vector3 savePoint, int health, AIAgression aIAgression, EBrainType enemyType,  
                    GunType gunType, EEnemyModifier[] modifiers)
  {
    Scene = scene;
    Savepoint = savePoint;
    Health = health;
    AIAgression = aIAgression;
    EnemyType = enemyType;
    GunType = gunType;
    Modifiers = modifiers;
  }
  public SaveObject()
  {

  }

    public override string ToString()
    {
        return $"Scene: {Scene}, \nSavePoint: {Savepoint}, \nHealth: {Health}";
    }
}
