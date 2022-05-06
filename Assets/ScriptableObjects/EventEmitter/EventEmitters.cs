using UnityEngine;

namespace UnityTools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EventEmitter/GameObject")]
    public class GameObjectEventEmitter : EventEmitter<GameObject> { }

    [CreateAssetMenu(menuName = "ScriptableObjects/EventEmitter/Float")]
    public class FloatEventEmitter : EventEmitter<float> { }

    [CreateAssetMenu(menuName = "ScriptableObjects/EventEmitter/String")]
    public class StringEventEmitter : EventEmitter<string> { }

    [CreateAssetMenu(menuName = "ScriptableObjects/EventEmitter/Int")]
    public class IntEventEmitter : EventEmitter<int> { }
}
