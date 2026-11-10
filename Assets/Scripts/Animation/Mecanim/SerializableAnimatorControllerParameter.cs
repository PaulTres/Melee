using UnityEngine;

namespace Animation
{
    [System.Serializable]
    public class SerializableAnimatorControllerParameter
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public AnimatorControllerParameterType Type { get; protected set; }
        [SerializeField] protected int? hash;
        public int Hash
        {
            get
            {
                hash ??= Animator.StringToHash(Name);
                return hash.Value;
            }
        }

        public SerializableAnimatorControllerParameter(string name, AnimatorControllerParameterType type)
        {
            Name = name;
            Type = type;
        }

        public SerializableAnimatorControllerParameter(AnimatorControllerParameter parameter)
        {
            Name = parameter.name;
            Type = parameter.type;
        }
    }
}