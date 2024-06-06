using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using System.IO;


namespace InhumansUtility
{
    [System.Serializable]
    public class Resource
    {
        public int ID;
        public string Name;
        public int Cost;
        public string Sprite;
        public string Description;
        public Specialization Type;
    }

    [System.Serializable]
    public class Resources : IEnumerable<Resource>
    {
        public List<Resource> ResourceList = new();

        public IEnumerator<Resource> GetEnumerator()
        {
            return ResourceList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ResourcesJSONLoader : MonoBehaviour
    {
        const string RESOURCE_DATA_PERSISTENT_PATH = "Assets/Resources/ResourcesData/ScriptableObjects";

        [SerializeField] private TextAsset ResourceJSONData;
        public Resources ResourceList = new();

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            ResourceList = JSONLoader.LoadFromJson<Resources>(ResourceJSONData);
        }

        public void CreateResourceAssets()
        {
            foreach (Resource resource in ResourceList)
            {
                ResourceData newInstance = CreateResourceInstance();
                InitializeResourceInstance(newInstance, resource);
#if UNITY_EDITOR
                CreateResourceAsset(newInstance);
#endif
            }
        }

        private ResourceData CreateResourceInstance()
        {
            return ScriptableObject.CreateInstance<ResourceData>();
        }

        private ResourceData InitializeResourceInstance(ResourceData instance, Resource resource)
        {
            instance.ID = resource.ID;
            instance.Name = resource.Name;
            instance.Cost = resource.Cost;
            instance.Icon = UnityEngine.Resources.Load<Sprite>(resource.Sprite);
            instance.Description = resource.Description;
            instance.Type = resource.Type;

            return instance;
        }
#if UNITY_EDITOR
        private void CreateResourceAsset(ResourceData instance)
        {
            UnityEditor.AssetDatabase.CreateAsset(
                instance,
                $"{RESOURCE_DATA_PERSISTENT_PATH}/{instance.Name}Data.asset"
            );
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ResourcesJSONLoader))]
    public class ResourceAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Resource Assets"))
                target.GetComponent<ResourcesJSONLoader>().CreateResourceAssets();
            base.OnInspectorGUI();
        }
    }
#endif
}