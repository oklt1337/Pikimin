using UnityEngine;
using UnityEngine.UI;
using static Pikimins.Pikimin;
using Random = UnityEngine.Random;

namespace Pikimins
{
    public sealed class Moves : MonoBehaviour
    {
        [SerializeField] private string moveName;
        [SerializeField] private PikiminType moveType;
        [SerializeField] private byte currentPp;
        [SerializeField] private byte maxPp;
        [SerializeField] private int power;
        [SerializeField] private float accuracy;
        [SerializeField] private Sprite sprite;
        [SerializeField] private AudioClip clip;
        
        [Header("Player Animtaion")]
        [SerializeField] private RuntimeAnimatorController animatorControllerPlayer;
        [SerializeField] private AnimationClip animationClipPlayer;
        
        [Header("Enemy Animation")]
        [SerializeField] private RuntimeAnimatorController animatorControllerEnemy;
        [SerializeField] private AnimationClip animationClipEnemy;

        public string MoveName => moveName;

        public PikiminType MoveType => moveType;

        public byte CurrentPp
        {
            get => currentPp;
            set => currentPp = value;
        }

        public byte MAXPp => maxPp;

        public int Power => power;

        public float Accuracy => accuracy;

        public RuntimeAnimatorController AnimatorControllerPlayer => animatorControllerPlayer;
        
        public RuntimeAnimatorController AnimatorControllerEnemy => animatorControllerEnemy;

        public AnimationClip AnimationClipPlayer => animationClipPlayer;
        
        public AnimationClip AnimationClipEnemy => animationClipEnemy;

        public Sprite Sprite => sprite;

        public AudioClip Clip => clip;
    }
}
