using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchDetails : MonoBehaviour
{
    [SerializeField] private Button costButton;
    [SerializeField] private Image potionImage;
    [SerializeField] private TextMeshProUGUI potionName;
    [SerializeField] private TextMeshProUGUI potionDescription;

    [SerializeField] private ResearchTree researchTree;

    //
    [SerializeField] private GameObject researchPanel;
    [SerializeField] private ResearchController researchController;
    public void StartResearch()
    {

        researchPanel.SetActive(true);
        researchController.Restart();
    }
    
    //
    //

    public void OpenResearchPanel()
    {
        researchPanel.SetActive(true);
    }

    public void CloseResearchPanel()
    {
        researchPanel.SetActive(false);
    }

    private void Start()
    {
        researchController.Initialize();
        costButton.onClick.AddListener(() => StartResearch());
    }

    public void RestartAndClose()
    {
        print(1);
        researchController.Restart();
        CloseResearchPanel();
    }

    //
    private void Update()
    {
    }
    //

    public void Open()
    {
        gameObject.SetActive(true);
    } 

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void RefreshData(ResearchNode node)
    {
        costButton.onClick.RemoveAllListeners();
        costButton.GetComponentInChildren<TextMeshProUGUI>().text = "Изучить: " + node.cost.ToString();
        potionImage.sprite = node.image;
        potionName.text = node.researchName;
        potionDescription.text = node.description;

        costButton.onClick.AddListener(() => researchTree.ResearchNode(node));
    }
}
