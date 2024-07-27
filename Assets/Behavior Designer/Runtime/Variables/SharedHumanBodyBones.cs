namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedHumanBodyBones : SharedVariable<UnityEngine.HumanBodyBones>
    {
        public static implicit operator SharedHumanBodyBones(UnityEngine.HumanBodyBones value) { return new SharedHumanBodyBones { Value = value }; }
    }
}