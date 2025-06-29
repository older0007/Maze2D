using UnityEngine;

namespace GamePlaySetup
{
    [CreateAssetMenu(menuName = "Maze/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float speed;

        public float Speed => speed;
    }
}