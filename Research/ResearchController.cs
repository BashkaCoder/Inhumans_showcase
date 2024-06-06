using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchController : MonoBehaviour
{
    [SerializeField] private Aspects allAspects;
    [SerializeField] private Resources allResources;

    [SerializeField] private ResearchFieldView fieldView;

    [SerializeField] private int northAspectID;
    [SerializeField] private int northEastAspectID;
    [SerializeField] private int southEastAspectID;
    [SerializeField] private int southAspectID;
    [SerializeField] private int southWestAspectID;
    [SerializeField] private int northWestAspectID;

    public int NorthAspectID     { get => northAspectID; private set => northAspectID = value; }
    public int NorthEastAspectID { get => northEastAspectID; private set => northEastAspectID = value; }
    public int SouthEastAspectID { get => southEastAspectID; private set => southEastAspectID = value; }
    public int SouthAspectID     { get => southAspectID; private set => southAspectID = value; }
    public int SouthWestAspectID { get => southWestAspectID; private set => southWestAspectID = value; }
    public int NorthWestAspectID { get => northWestAspectID; private set => northWestAspectID = value; }

    [SerializeField] private int aspectCap;

    Dictionary<int, RectTransform> IDtoAspectsPosition;

    //
    public ResearchDetails researchDetails;
    //


    List<List<ContainedAspect>> CreateResources(int numberOfResources)
    {
        List<List<ContainedAspect>> resources = new List<List<ContainedAspect>>();

        for (int i = 0; i < numberOfResources; i++)
        {
            List<ContainedAspect> resource = CreateResource();
            resources.Add(resource);
        }

        return resources;
    }

    List<ContainedAspect> CreateResource()
    {
        List<ContainedAspect> resource = new List<ContainedAspect>();

        // Add 1 or 2 aspects to the current resource
        int numberOfAspects = 1;
        for (int j = 0; j < numberOfAspects; j++)
        {
            ContainedAspect aspect = new ContainedAspect
            {
                aspectID = UnityEngine.Random.Range(0, 6), // Random aspectID from 0 to 6
                amount = 10
            };
            resource.Add(aspect);
        }

        return resource;
    }

    private void InitializeIDtoAspectsPosition()
    {
        // Инициализация словаря перед добавлением элементов
        IDtoAspectsPosition = new Dictionary<int, RectTransform>
        {
            [northAspectID] = fieldView.NorthAspectCap,
            [northEastAspectID] = fieldView.NorthEastAspectCap,
            [southEastAspectID] = fieldView.SouthEastAspectCap,
            [southAspectID] = fieldView.SouthAspectCap,
            [southWestAspectID] = fieldView.SouthWestAspectCap,
            [northWestAspectID] = fieldView.NorthWestAspectCap
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize();
    }

    //
    public void Restart()
    {
        fieldView.ResetCursorPosition();
        fieldView.ResetHighlightedPiece();
        fieldView.HighlightRandomPieceOnLevel(2);
    }

    public void Initialize()
    {
        List<Aspect> fieldViewAspects = FormAspectsList();
        fieldView.Initialize();

        fieldView.InitializeAspectsView(fieldViewAspects);

        fieldView.HighlightRandomPieceOnLevel(2);

        InitializeIDtoAspectsPosition();
    }
    //

    /// <summary>
    /// ///
    /// </summary>
    public void MN()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[northAspectID], 0.1f));
        //foreach(var aspect in CreateResource())
        //{
        //    print($"ID: {aspect.aspectID}, Amount{aspect.amount / 100.0f}");
        //    fieldView.MoveCursor(IDtoAspectsPosition[aspect.aspectID],  aspect.amount / 100.0f);
        //}
    }
    public void MNE()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[northEastAspectID], 0.1f));
    }
    public void MSE()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[southEastAspectID], 0.1f));
    }
    public void MS()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[southAspectID], 0.1f));
    }
    public void MSW()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[southWestAspectID], 0.1f));
    }
    public void MNW()
    {
        StartCoroutine(fieldView.MoveCursor(IDtoAspectsPosition[northWestAspectID], 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        //Вынести в расщепление реусрсов на аспекты
        if (IsCursorInsideZone(fieldView.highlightedPiece.pieceImage, fieldView.cursor))
        {
            //
            researchDetails.RestartAndClose();
            //
            //Debug.Log("Cursor is inside the zone!");
        }
        else
        {
            //Debug.Log("Cursor is outside the zone.");
        }
    }

    private List<Aspect> FormAspectsList()
    {
        List<Aspect> result = new()
        {
            allAspects.aspects.Find(e => e.ID == northAspectID),
            allAspects.aspects.Find(e => e.ID == northEastAspectID),
            allAspects.aspects.Find(e => e.ID == southEastAspectID),
            allAspects.aspects.Find(e => e.ID == southAspectID),
            allAspects.aspects.Find(e => e.ID == southWestAspectID),
            allAspects.aspects.Find(e => e.ID == northWestAspectID)
        };

        return result;
    }

    bool IsCursorInsideZone(Image zone, Image cursor)
    {
        RectTransform zoneRectTransform = zone.rectTransform;
        RectTransform cursorRectTransform = cursor.rectTransform;

        // Получаем локальные координаты центра курсора в пределах зоны
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(zoneRectTransform, cursorRectTransform.position, null, out localCursor);

        // Нормализуем локальные координаты относительно размеров зоны, чтобы получить UV-координаты
        Vector2 uvCoords = new Vector2(
            Mathf.InverseLerp(0, zoneRectTransform.rect.width, localCursor.x),
            Mathf.InverseLerp(0, zoneRectTransform.rect.height, localCursor.y)
        );

        // Проверяем, содержит ли зона курсор
        return RectTransformUtility.RectangleContainsScreenPoint(zoneRectTransform, cursorRectTransform.position) 
            && zone.sprite.texture.GetPixelBilinear(uvCoords.x, uvCoords.y).a > 0;
    }

    private void DisolveResource(ResearchResource resource)
    {
        //fieldView.moveCursor(ContainedAspect.id, ContainedAspect.amount / aspectCap);
    }

}
