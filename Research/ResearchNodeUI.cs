using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costLabel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image lockImage;

    [SerializeField] private Button detailsButton;

    private ResearchTree researchTree;

    public void Initialize(ResearchNode node, ResearchTree tree)
    {
        researchTree = tree;

        itemImage.sprite = node.image;
        costLabel.text = "Изучить";

        detailsButton.onClick.AddListener(() => researchTree.ShowDetais(gameObject.name));
    }

    public void SetResarchButtonInteractableState(bool state)
    {
        detailsButton.interactable = state;
    }

    public void EnableLock()
    {
        lockImage.gameObject.SetActive(true);
        SetResarchButtonInteractableState(false);
    }

    public void DisableLock()
    {
        lockImage.gameObject.SetActive(false);
        SetResarchButtonInteractableState(true);
    }
}