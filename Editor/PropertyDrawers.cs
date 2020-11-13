using UnityEditor;
using static UnityTools.AudioClips;

[CustomPropertyDrawer(typeof(AudioClipsDictionary))] public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
