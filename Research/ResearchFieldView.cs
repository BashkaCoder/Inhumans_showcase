using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchFieldView : MonoBehaviour
{
    [Header("Aspect Images")]
    [SerializeField] private Image northAspectImage;
    [SerializeField] private Image northEastAspectImage;
    [SerializeField] private Image southEastAspectImage;
    [SerializeField] private Image southAspectImage;
    [SerializeField] private Image southWestAspectImage;
    [SerializeField] private Image northWestAspectImage;

    [field: SerializeField] public RectTransform NorthAspectCap { get; private set; }
    [field: SerializeField] public RectTransform NorthEastAspectCap { get; private set; }
    [field: SerializeField] public RectTransform SouthEastAspectCap { get; private set; }
    [field: SerializeField] public RectTransform SouthAspectCap { get; private set; }
    [field: SerializeField] public RectTransform SouthWestAspectCap { get; private set; }
    [field: SerializeField] public RectTransform NorthWestAspectCap { get; private set; }

    [Header("Sides")]
    [SerializeField] private GameObject northEastSide;
    [SerializeField] private GameObject eastSide;
    [SerializeField] private GameObject southEastSide;
    [SerializeField] private GameObject northWestSide;
    [SerializeField] private GameObject WestSide;
    [SerializeField] private GameObject southWestSide;

    [Header("Cursor")]
    [SerializeField] public Image cursor;
    private RectTransform cursorContainer;
    private Vector3 researchCenter;

    public ResearchFieldPiece highlightedPiece = null;

    public void InitializeAspectsView(List<Aspect> aspects) 
    {
        northAspectImage.sprite     = aspects[0].imageIcon;
        northEastAspectImage.sprite = aspects[1].imageIcon;
        southEastAspectImage.sprite = aspects[2].imageIcon;
        southAspectImage.sprite     = aspects[3].imageIcon;
        southWestAspectImage.sprite = aspects[4].imageIcon;
        northWestAspectImage.sprite = aspects[5].imageIcon;
    }

    public void InitializeAspectsPosition(List<Aspect> aspects)
    {

    }

    //
    public void ResetCursorPosition()
    {
        cursorContainer.localPosition = researchCenter;
    }

    public void ResetHighlightedPiece()
    {
        highlightedPiece.SetHighlight(false);
    }

    //

    void Start()
    {
        Initialize();
        //
        //ResetCursorPosition();
        //
    }

    public void Initialize()
    {
        cursorContainer = cursor.transform.parent.GetComponent<RectTransform>();
        researchCenter = cursorContainer.localPosition;
    }

    public void HighlightRandomPieceOnLevel(int level)
    {
        if (level < 0 || level >= 3)
            throw new ArgumentException("Level is out of bounds:[0; 2]");


        List<GameObject> sides = new()
        {
            northEastSide,
            eastSide,
            southEastSide,
            northWestSide,
            WestSide,
            southWestSide
        };

        //
        foreach (GameObject side in sides)
        {
            for (int i = 0; i < side.transform.childCount; i++) 
            {
                side.transform.GetChild(i).GetComponent<ResearchFieldPiece>().Initialize();
            }
        }
        //

        GameObject randomSide = sides[UnityEngine.Random.Range(0, 6)];
        highlightedPiece = randomSide.transform.GetChild(level).GetComponent<ResearchFieldPiece>();
        highlightedPiece.SetHighlight(true);
    }

    public IEnumerator MoveCursor(RectTransform aspectCapPosition, float amount)
    {
        if (amount < 0 || amount > 1)
            throw new ArgumentException("Ты долбаеб что ли. T должно быть нормализованным - [0; 1]!!!");

        Vector3 direction = (aspectCapPosition.localPosition - researchCenter);
        Vector3 newPotentialPosition = cursorContainer.localPosition + direction * amount;

        // Проверяем, не выходит ли курсор за пределы головоломки
        if (IsPointInsideHexagon(newPotentialPosition))
        {
            float elapsedTime = 0;
            Vector3 startingPosition = cursorContainer.localPosition;

            while (elapsedTime < 0.3f)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / 0.3f);
                cursorContainer.localPosition = Vector3.Lerp(startingPosition, newPotentialPosition, t);
                yield return null;
            }

            cursorContainer.localPosition = newPotentialPosition;
        }
    }


    private bool IsPointInsideHexagon(Vector2 point)
    {
        // Предполагается, что hexagonVertices содержит координаты вершин шестиугольника
        List<Vector2> hexagonVertices = new List<Vector2>
    {
        NorthAspectCap.localPosition,
        NorthEastAspectCap.localPosition,
        SouthEastAspectCap.localPosition,
        SouthAspectCap.localPosition,
        SouthWestAspectCap.localPosition,
        NorthWestAspectCap.localPosition
    };

        bool isInside = false;
        int j = hexagonVertices.Count - 1;

        for (int i = 0; i < hexagonVertices.Count; i++)
        {
            if ((hexagonVertices[i].y < point.y && hexagonVertices[j].y >= point.y
                || hexagonVertices[j].y < point.y && hexagonVertices[i].y >= point.y)
                && (hexagonVertices[i].x <= point.x || hexagonVertices[j].x <= point.x))
            {
                isInside ^= (hexagonVertices[i].x + (point.y - hexagonVertices[i].y) / (hexagonVertices[j].y - hexagonVertices[i].y) * (hexagonVertices[j].x - hexagonVertices[i].x) < point.x);
            }

            j = i;
        }

        return isInside;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
