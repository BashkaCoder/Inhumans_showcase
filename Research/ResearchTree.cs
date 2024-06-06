using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTree : MonoBehaviour
{
    const float VERTICAL_ALIGN = 90.0f;
    const float HORIZONTAL_ALIGN = 0.0f;

    public ResearchNode rootNode; //  орневые узлы дерева исследований
    public GameObject nodePrefab; // ѕрефаб дл€ отображени€ узла
    public float horizontalSpacing;
    public float verticalSpacing;
    public float thickness;
    public Color connectionColor;

    [SerializeField] private Transform nodesHolder;
    [SerializeField] private Transform connectionsHolder;
    [SerializeField] private ResearchDetails researchDetailsPanel;

    private float nodeWidth;
    private float nodeHeight;

    private float offsetX;

    private Dictionary<string, ResearchNodeUI> nodesUI = new Dictionary<string, ResearchNodeUI>();
    public Dictionary<string, ResearchNode> nodes = new Dictionary<string, ResearchNode>();
    public Dictionary<string, bool> resarchedNodes = new Dictionary<string, bool>();

    private void Start()
    {
        nodeWidth = nodePrefab.GetComponent<RectTransform>().rect.width;
        nodeHeight = nodePrefab.GetComponent<RectTransform>().rect.height;

        offsetX = -(nodeWidth + horizontalSpacing);

        InstantiateNodes(rootNode);
        PositionNodes(rootNode, null, 0, 0);
        nodesUI[rootNode.name].DisableLock();

        // ѕосле того, как все узлы созданы и расположены, создайте св€зи между ними.
        CreateConnections();
    }

    private void InstantiateNodes(ResearchNode node)
    {

        if (NodeExists(node.name))
            return;

        GameObject nodeObject = Instantiate(nodePrefab, nodesHolder);
        nodeObject.name = node.name;

        //
        node.isLocked = true;

        ResearchNodeUI nodeUI = nodeObject.GetComponent<ResearchNodeUI>();
        nodeUI.Initialize(node, this);
        //
        nodeUI.SetResarchButtonInteractableState(false);

        nodes.Add(node.name, node);
        nodesUI.Add(nodeObject.name, nodeUI);
        //
        resarchedNodes.Add(node.name, false);

        if (node.children != null && node.children.Length > 0)
        {
            foreach (var childNode in node.children)
            {
                InstantiateNodes(childNode);
            }
        }
    }

    private void PositionNodes(ResearchNode node, RectTransform parentRectTransform, int depth, int siblingIndex)
    {
        Transform childTransform = nodesHolder.Find(node.name);
        RectTransform nodeRectTransform = childTransform.GetComponent<RectTransform>();

        // ¬ычисл€ем координаты узла в соответствии с HV-Drawing
        float xPos = + siblingIndex * (nodeWidth + horizontalSpacing);
        float yPos = -depth * (nodeHeight + verticalSpacing);

        if (node != rootNode)
            xPos += offsetX;

        if (node.parents.Length > 1)
            xPos += offsetX;

        // ”станавливаем координаты узла
        nodeRectTransform.anchoredPosition = new Vector2(xPos, yPos);
        node.depth = depth; // установка уровн€ узла

        if (node.children != null && node.children.Length > 0)
        {
            // ≈сли есть дети, рассчитываем координаты дл€ каждого ребенка
            foreach (var childNode in node.children)
            {
                PositionNodes(childNode, nodeRectTransform, depth + 1, siblingIndex);
                siblingIndex++;
            }
        }
    }

    private void CreateConnections()
    {
        foreach (var node in nodes.Values)
        {
            if (node.parents != null && node.parents.Length > 0)
            {
                foreach (var parentNode in node.parents)
                {
                    CreateConnection(nodes[parentNode.name], node);
                }
            }
        }
    }

    private void CreateConnection(ResearchNode parent, ResearchNode child)
    {
        RectTransform parentRectTransform = nodesHolder.Find(parent.name).GetComponent<RectTransform>();
        RectTransform childRectTransform = nodesHolder.Find(child.name).GetComponent<RectTransform>();

        Vector2 startPos = parentRectTransform.anchoredPosition;
        Vector2 endPos = childRectTransform.anchoredPosition;

        float halfHeight = Mathf.Abs((endPos - startPos).y) / 2;

        if (Mathf.Abs((endPos - startPos).x) < 0.001f)
        {
            CreateConnectionSegment(startPos, endPos, VERTICAL_ALIGN);
        }
        else 
        {
            Vector2 MiddleSegmentLeftPoint = new Vector2(startPos.x, startPos.y - halfHeight);
            Vector2 MiddleSegmentRightPoint = new Vector2(endPos.x, startPos.y - halfHeight);

            CreateConnectionSegment(startPos, MiddleSegmentLeftPoint, VERTICAL_ALIGN);
            CreateConnectionSegment(MiddleSegmentLeftPoint, MiddleSegmentRightPoint, HORIZONTAL_ALIGN);
            CreateConnectionSegment(MiddleSegmentRightPoint, endPos, VERTICAL_ALIGN);
        }
    }

    private void CreateConnectionSegment(Vector2 startPos, Vector2 endPos, float angle)
    {
        GameObject connection = new GameObject("Connection", typeof(Image));
        connection.transform.SetParent(connectionsHolder, false);
        Image connectionImage = connection.GetComponent<Image>();

        float length = Vector2.Distance(startPos, endPos);

        connectionImage.color = connectionColor;
        connectionImage.rectTransform.sizeDelta = new Vector2(length + thickness, thickness);
        Vector2 position = startPos + (endPos - startPos) / 2f;
        connectionImage.rectTransform.anchoredPosition = position;
        connectionImage.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private bool NodeExists(string nodeName) => nodes.ContainsKey(nodeName) || nodesUI.ContainsKey(nodeName);

    public void ShowDetais(string nodeName)
    {
        researchDetailsPanel.RefreshData(nodes[nodeName]);
        researchDetailsPanel.Open();
    }

    public void ResearchNode(ResearchNode node)
    {
        node.isLocked = false;
        resarchedNodes[node.name] = true;

        foreach (var child in node.children)
        {
            UnlockChild(child);
        }
        UpdateResearchButtonOfAllNodes();
    }

    private void UnlockChild(ResearchNode child)
    {
        foreach (var parent in child.parents)
        {
            print(parent.isLocked);
            if (parent.isLocked)
            {
                return;
            }
        }

        child.isLocked = false;
        nodesUI[child.name].DisableLock();
    }

    private void UpdateResearchButtonOfAllNodes()
    {
        foreach (var node in nodes)
        {
            if (resarchedNodes[node.Key])
            {
                nodesUI[node.Key].SetResarchButtonInteractableState(false);
            }
        }
    }

}