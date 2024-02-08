using Guns;
using UnityEngine;
using Guns.Demo;
using Guns.Health;
using Player;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
//using CharacterInfo = Player.CharacterInfo;
//using UnityEditor.SearchService;


[System.Serializable]

public struct SaveObject
{
  public Transform Savepoint;
  public int Health;
  public AIAgression AIAgression;
  public EBrainType EnemyType;
  public Guns.GunScriptableObject GunType;
  public EEnemyModifier[] Modifiers;

  public SaveObject(Transform savePoint, int health, AIAgression aIAgression, EBrainType enemyType,  
                    Guns.GunScriptableObject gunType, EEnemyModifier[] modifiers)
  {
    Savepoint = savePoint;
    Health = health;
    AIAgression = aIAgression;
    EnemyType = enemyType;
    GunType = gunType;
    Modifiers = modifiers;
  }
}
