using UnityEngine;
namespace Guns.Stats
{
    [CreateAssetMenu(fileName = "CharacterStatsScriptableObject", menuName = "CharacterStatsScriptableObject", order = 0)]
    public class CharacterStatsScriptableObject : ScriptableObject, System.ICloneable
    {
        [Header("General Stats")]
        public float Health = 100;
        [Header("Ground Stats")]
        public float WalkMoveSpeed = 45;
        public float JumpForce = 1.5f;

        [Header("Wall Stats")]
        public float WallMoveSpeed = 55;
        public float WallJumpForce = 55;

        public object Clone()
        {
            CharacterStatsScriptableObject config = CreateInstance<CharacterStatsScriptableObject>();

            config.Health = Health;
            config.WalkMoveSpeed = WalkMoveSpeed;
            config.JumpForce = JumpForce;
            config.WallMoveSpeed = WallMoveSpeed;
            config.WallJumpForce = WallJumpForce;
            return config;
        }
    }
}

