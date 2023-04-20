using System;
using Unity.Netcode;
using UnityEngine;

public class Metahuman : NetworkBehaviour, ISaveable
{
    public NetworkVariable<Gender> gender = new();
    public NetworkVariable<Vector3> scale = new(Vector3.one);
    public NetworkVariable<Color> skinTone = new(new Color(0.9058824f, 0.8156863f, 0.7058823f));
    public NetworkVariable<Color> hairColor = new(new Color(0.4386f, 0.6519f, 1f));
    public NetworkVariable<int> outfitIndex = new(0);
    public NetworkVariable<int> hairIndex = new(0);
    public NetworkVariable<int> hatIndex = new(-1);

    public Animator animator;

    public GameObject maleMesh;
    public Avatar maleAvatar;
    public GameObject femaleMesh;
    public Avatar femaleAvatar;
    public SkinnedMeshRenderer[] boneSources;
    public Transform[] outfitContainers;
    public Transform[] hairContainers;
    public Transform[] hatContainers;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ConnectBones()
    {
        foreach (var boneSource in boneSources)
        {
            foreach (var smr in boneSource.transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.rootBone = boneSource.rootBone;
                smr.bones = boneSource.bones;
            }
        }
    }

    private void OnValidate()
    {
        OnGenderChanged(gender.Value, gender.Value);
        OnScaleChanged(scale.Value, scale.Value);
        EnableByIndex(hairContainers, hairIndex.Value);
        EnableByIndex(outfitContainers, outfitIndex.Value);
        EnableByIndex(hatContainers, hatIndex.Value);

        // Remove all Armatures from containers
        foreach (var containers in new Transform[][] { outfitContainers, hairContainers, hatContainers })
        {
            foreach (var container in containers)
            {
                foreach (Transform child in container)
                {
                    child.Find("Armature").gameObject.SetActive(false);
                }
            }
        }
    }

    public void PreviewColors()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Must be in Play Mode");
            return;
        }

        OnHairColorChanged(hairColor.Value, hairColor.Value);
        OnSkinToneChanged(skinTone.Value, skinTone.Value);
    }

    public override void OnNetworkSpawn()
    {
        gender.OnValueChanged += OnGenderChanged;
        OnGenderChanged(gender.Value, gender.Value);

        outfitIndex.OnValueChanged += OnOutfitChanged;
        OnOutfitChanged(outfitIndex.Value, outfitIndex.Value);

        hairIndex.OnValueChanged += OnHairChanged;
        OnHairChanged(hairIndex.Value, hairIndex.Value);

        hatIndex.OnValueChanged += OnHatChanged;
        OnHatChanged(hatIndex.Value, hatIndex.Value);

        scale.OnValueChanged += OnScaleChanged;
        OnScaleChanged(scale.Value, scale.Value);

        skinTone.OnValueChanged += OnSkinToneChanged;
        hairColor.OnValueChanged += OnHairColorChanged;
    }

    public override void OnNetworkDespawn()
    {
        gender.OnValueChanged -= OnGenderChanged;
        scale.OnValueChanged -= OnScaleChanged;
        skinTone.OnValueChanged -= OnSkinToneChanged;
        hairColor.OnValueChanged -= OnHairColorChanged;
        outfitIndex.OnValueChanged -= OnOutfitChanged;
        hairIndex.OnValueChanged -= OnHairChanged;
        hatIndex.OnValueChanged -= OnHatChanged;
    }

    private void OnHatChanged(int previousValue, int newValue)
    {
        EnableByIndex(hatContainers, newValue);
        ConnectBones();
    }

    private void OnHairChanged(int previousValue, int newHairIndex)
    {
        EnableByIndex(hairContainers, newHairIndex);

        OnHairColorChanged(hairColor.Value, hairColor.Value);
        ConnectBones();
    }

    private void OnOutfitChanged(int _, int newOutfitIndex)
    {
        EnableByIndex(outfitContainers, newOutfitIndex);

        OnSkinToneChanged(skinTone.Value, skinTone.Value);
        ConnectBones();
    }

    private void EnableByIndex(Transform[] containers, int index)
    {
        foreach (var container in containers)
        {
            for (int i = 0; i < container.childCount; i++)
            {
                container.GetChild(i).gameObject.SetActive(i == index);
            }
        }
    }

    private void OnGenderChanged(Gender _, Gender gender)
    {
        var isMale = gender == Gender.Male;
        maleMesh.SetActive(isMale);
        femaleMesh.SetActive(!isMale);

        animator.avatar = isMale ? maleAvatar : femaleAvatar;
    }

    private void OnScaleChanged(Vector3 _, Vector3 newScale)
    {
        if (transform.parent == null)
        {
            return;
        }

        transform.parent.localScale = newScale;
    }

    private void OnSkinToneChanged(Color _, Color newColor)
    {
        var shade = new Color(newColor.r * 0.7285f, newColor.g * 0.7285f, newColor.b * 0.7285f);

        foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (var material in renderer.materials)
            {
                if (material.name.ToLower().Contains("skin"))
                {
                    material.SetColor("_Color", newColor);
                    material.SetColor("_BaseColor", newColor);
                    material.SetColor("_1st_ShadeColor", shade);;
                }
            }
        }
    }

    private void OnHairColorChanged(Color _, Color newColor)
    {
        foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (var material in renderer.materials)
            {
                if (material.name.ToLower().Contains("hairtexture"))
                {
                    material.SetColor("_Color", newColor);
                    material.SetColor("_BaseColor", newColor);
                }
            }
        }
    }

    [Serializable]
    public struct SaveData
    {
        public Gender gender;
        public Vector3 scale;
        public Color skinTone;
        public Color hairColor;
        public int outfitIndex;
        public int hairIndex;
    }

    public object Save()
    {
        return JsonUtility.ToJson(new SaveData()
        {
            gender = gender.Value,
            scale = scale.Value,
            skinTone = skinTone.Value,
            hairColor = hairColor.Value,
            outfitIndex = outfitIndex.Value,
            hairIndex = hairIndex.Value
        });
    }

    public void Load(object saveData)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(saveData.ToString());

        gender.Value = data.gender;
        scale.Value = data.scale;
        skinTone.Value = data.skinTone;
        hairColor.Value = data.hairColor;
        outfitIndex.Value = data.outfitIndex;
        hairIndex.Value = data.hairIndex;
    }

    public enum Gender
    {
        Male,
        Female
    }
}
