using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor;


namespace InhumansUtility
{
    [System.Serializable]
    public class Product
    {
        public int ID;
        public string Name;
        public string Quality;
        public List<int> Components = new();
        public int Cost;
        public string Sprite;
        public string Description;
        public string Type;
        public float CraftingTime;
        public int CraftingAmount;
    }

    [System.Serializable]
    public class Requirements
    {
        public int ID;
        public int amount;
    }

    [System.Serializable]
    public class ProductList : IEnumerable<Product>
    {
        public List<Product> ProductsList = new();

        public IEnumerator<Product> GetEnumerator()
        {
            return ProductsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ProductJSONLoader : MonoBehaviour
    {
        const string RESOURCE_DATA_PERSISTENT_PATH = "Assets/Resources/ProductsData/ScriptableObjects";

        [SerializeField] private TextAsset ProductsJSONData;
        public ProductList ProductList = new();

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            ProductList = JSONLoader.LoadFromJson<ProductList>(ProductsJSONData);
        }

        public void CreateProductAssets()
        {
            foreach (Product product in ProductList)
            {
                ProductData newInstance = CreateProductInstance();
                InitializeProductInstance(newInstance, product);
#if UNITY_EDITOR
                CreateProductAsset(newInstance);
#endif
            }
        }

        private ProductData CreateProductInstance()
        {
            return ScriptableObject.CreateInstance<ProductData>();
        }

        private ProductData InitializeProductInstance(ProductData instance, Product product)
        {
            instance.ID = product.ID;
            instance.Name = product.Name;
            instance.Quality = GetQuality(product.Quality);
            instance.Type = GetType(product.Type);
            instance.Requirements = GetRequirements(product.Components);
            instance.Cost = product.Cost;
            instance.Icon = UnityEngine.Resources.Load<Sprite>(product.Sprite);
            instance.Description = product.Description;
            instance.CraftingTime = product.CraftingTime;
            instance.CraftingAmount = product.CraftingAmount;

            return instance;
        }
#if UNITY_EDITOR
        private void CreateProductAsset(ProductData instance)
        {
            UnityEditor.AssetDatabase.CreateAsset(
                instance,
                $"{RESOURCE_DATA_PERSISTENT_PATH}/{instance.Name}Data.asset"
            );
        }
#endif

        private Specialization GetType(string specialization)
        {
            return specialization switch
            {
                "Alchemy" => Specialization.Alchemy,
                "Blacksmithing" => Specialization.Blacksmithing,
                "Joinery" => Specialization.Joinery,
                _ => throw new ArgumentException("Invalid product specialization type in json"),
            };
        }

        private Quality GetQuality(string quality)
        {
            return quality switch
            {
                "Bad" => Quality.Bad,
                "Normal" => Quality.Normal,
                "High" => Quality.High,
                _ => throw new ArgumentException("Invalid product quality in json"),
            };
        }

        private List<ItemRequirement> GetRequirements(List<int> requirements)
        {
            List<ItemRequirement> itemRequirements = new();
            for (int i = 0; i < requirements.Count; i += 2)
            {
                ItemRequirement newRequirement = new() { ResourceID = requirements[i], Amount = requirements[i + 1] };
                itemRequirements.Add(newRequirement);
            }
            return itemRequirements;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ProductJSONLoader))]
    public class ProductAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create Product Assets"))
                target.GetComponent<ProductJSONLoader>().CreateProductAssets();
            base.OnInspectorGUI();
        }
    }
#endif
}