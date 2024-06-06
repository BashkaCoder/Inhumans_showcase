using DialogueEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WhitehallControllerUIHandler))]
/// <summary> Класс управления диалогами чиновников.</summary>
public class WhitehallController : MonoBehaviour
{
    /// <summary> Количество полученных одобрений от чиновников.</summary>
    public int score => (int)AlchemyPlaceman.TrustNormalized + (int)JoineryPlaceman.TrustNormalized + (int)SmitheryPlaceman.TrustNormalized;

    /// <summary> Переменные на проверку первого обращения к чиновнику.</summary>
    private bool firstAlchemy = true;
    private bool firstJoinery = true;
    private bool firstSmithery = true;

    /// <summary> Переменные на проверку полного выполнения заданий чиновника.</summary>
    private bool fullAlchemy = false;
    private bool fullJoinery = false;
    private bool fullSmithery = false;

    /// <summary> Переменная-погрешность для части выполненных контрактов.</summary>
    private float delta = 0.00001f;

    /// <summary> Ссылка на ContractLogger.</summary>
    [SerializeField] private ContractLogger contractLogger;
    /// <summary> Ссылка на WhitehallControllerUIHandler.</summary>
    private WhitehallControllerUIHandler whitehallControllerUIHandler;

    /// <summary> Ссылки на все виды диалогов.</summary>
    [SerializeField] public NPCConversation Alchemy_dialogue_first;
    [SerializeField] public NPCConversation Joinery_dialogue_first;
    [SerializeField] public NPCConversation Smithery_dialogue_first;
    [SerializeField] public NPCConversation Alchemy_dialogue_up_to_20;
    [SerializeField] public NPCConversation Joinery_dialogue_up_to_20;
    [SerializeField] public NPCConversation Smithery_dialogue_up_to_20;
    [SerializeField] public NPCConversation Alchemy_dialogue_up_to_60;
    [SerializeField] public NPCConversation Joinery_dialogue_up_to_60;
    [SerializeField] public NPCConversation Smithery_dialogue_up_to_60;
    [SerializeField] public NPCConversation Alchemy_dialogue_up_to_90;
    [SerializeField] public NPCConversation Joinery_dialogue_up_to_90;
    [SerializeField] public NPCConversation Smithery_dialogue_up_to_90;
    [SerializeField] public NPCConversation Alchemy_dialogue_up_to_100;
    [SerializeField] public NPCConversation Joinery_dialogue_up_to_100;
    [SerializeField] public NPCConversation Smithery_dialogue_up_to_100;
    [SerializeField] public NPCConversation Alchemy_dialogue_full_first;
    [SerializeField] public NPCConversation Joinery_dialogue_full_first;
    [SerializeField] public NPCConversation Smithery_dialogue_full_first;
    [SerializeField] public NPCConversation Alchemy_dialogue_full_repeat;
    [SerializeField] public NPCConversation Joinery_dialogue_full_repeat;
    [SerializeField] public NPCConversation Smithery_dialogue_full_repeat;

    /// <summary> Метод выыбора диалога в зависимости от чиновника и количества его выполненных контрактов.</summary>
    public void DialogueDependsOnTrust(Placeman placeman)
    {
        if (placeman.name == "Placeman_Alchemy")
        {
            if (firstAlchemy == true)
            {
                StartTheDialogue(Alchemy_dialogue_first);
                firstAlchemy = false;
            }
            else
            {
                if (placeman.TrustNormalized <= 0.2 + delta)
                {
                    StartTheDialogue(Alchemy_dialogue_up_to_20);
                }

                else if (placeman.TrustNormalized <= 0.6 + delta && placeman.TrustNormalized > 0.2)
                {
                    StartTheDialogue(Alchemy_dialogue_up_to_60);
                }

                else if (placeman.TrustNormalized <= 0.9 + delta && placeman.TrustNormalized > 0.6)
                {
                    StartTheDialogue(Alchemy_dialogue_up_to_90);
                }

                else if (placeman.TrustNormalized < 1 && placeman.TrustNormalized > 0.9)
                {
                    StartTheDialogue(Alchemy_dialogue_up_to_100);
                }

                else if (placeman.TrustNormalized == 1)
                {
                    if (fullAlchemy == false)
                    {
                        StartTheDialogue(Alchemy_dialogue_full_first);
                        fullAlchemy = true;
                    }

                    else
                    {
                        StartTheDialogue(Alchemy_dialogue_full_repeat);
                    }
                }
            }
        }

        else if (placeman.name == "Placeman_Joinery")
        {
            if (firstJoinery == true)
            {
                StartTheDialogue(Joinery_dialogue_first);
                firstJoinery = false;
            }

            else
            {
                if (placeman.TrustNormalized <= 0.2 + delta)
                {
                    StartTheDialogue(Joinery_dialogue_up_to_20);
                }

                else if (placeman.TrustNormalized > 0.2 && placeman.TrustNormalized <= 0.6 + delta)
                {
                    StartTheDialogue(Joinery_dialogue_up_to_60);
                }

                else if (placeman.TrustNormalized <= 0.9 + delta && placeman.TrustNormalized > 0.6)
                {
                    StartTheDialogue(Joinery_dialogue_up_to_90);
                }

                else if (placeman.TrustNormalized < 1 && placeman.TrustNormalized > 0.9)
                {
                    StartTheDialogue(Joinery_dialogue_up_to_100);
                }

                else if (placeman.TrustNormalized == 1)
                {
                    if (fullJoinery == false)
                    {
                        StartTheDialogue(Joinery_dialogue_full_first);
                        fullJoinery = true;
                    }

                    else
                    {
                        StartTheDialogue(Joinery_dialogue_full_repeat);
                    }
                }
            }
        }

        else if (placeman.name == "Placeman_Smithery")
        {
            if (firstSmithery == true)
            {
                StartTheDialogue(Smithery_dialogue_first);
                firstSmithery = false;
            }

            else
            {
                if (placeman.TrustNormalized <= 0.2 + delta)
                {
                    StartTheDialogue(Smithery_dialogue_up_to_20);
                }

                else if (placeman.TrustNormalized <= 0.6 + delta && placeman.TrustNormalized > 0.2)
                {
                    StartTheDialogue(Smithery_dialogue_up_to_60);
                }

                else if (placeman.TrustNormalized <= 0.9 + delta && placeman.TrustNormalized > 0.6)
                {
                    StartTheDialogue(Smithery_dialogue_up_to_90);
                }

                else if (placeman.TrustNormalized < 1 && placeman.TrustNormalized > 0.9)
                {
                    StartTheDialogue(Smithery_dialogue_up_to_100);
                }

                else if (placeman.TrustNormalized == 1)
                {
                    if (fullSmithery == false)
                    {
                        StartTheDialogue(Smithery_dialogue_full_first);
                        fullSmithery = true;
                    }

                    else
                    {
                        StartTheDialogue(Smithery_dialogue_full_repeat);
                    }
                }
            }
        }
    }

    /// <summary> Метод для старта диалога.</summary>
    public void StartTheDialogue(NPCConversation thisConversation)
    {
        ConversationManager.Instance.StartConversation(thisConversation);
    }

    /// <summary> Словарь. Ключ - [гильдия], значение - "чиновник гильдии".</summary>
    Dictionary<Specialization, Placeman> specToPlacemanDictionary;

    /// <summary> Список чиновников.</summary>
    [Header("Placemen")]
    [SerializeField] private Placeman AlchemyPlaceman;
    [SerializeField] private Placeman JoineryPlaceman;
    [SerializeField] private Placeman SmitheryPlaceman;

    private void Start()
    {
        Initialize();
    }

    /// <summary> Инициализация словаря.</summary>
    private void Initialize()
    {
        specToPlacemanDictionary = new()
        {
            {Specialization.Alchemy,  AlchemyPlaceman},
            {Specialization.Joinery,  JoineryPlaceman},
            {Specialization.Blacksmithing, SmitheryPlaceman}
        };

        contractLogger = GetComponent<ContractLogger>();
        contractLogger.Initialize();

        whitehallControllerUIHandler = GetComponent<WhitehallControllerUIHandler>();
        AssignPlacemanButtons();
        AssignPlacemanButtonsListeners();
    }

    /// <summary> Назначение кнопок чиновников.</summary>
    private void AssignPlacemanButtons()
    {
        whitehallControllerUIHandler.AlchemyPlacemanButton = AlchemyPlaceman.GetComponent<Button>();
        whitehallControllerUIHandler.JoineryPlacemanButton = JoineryPlaceman.GetComponent<Button>();
        whitehallControllerUIHandler.SmitheryPlacemanButton = SmitheryPlaceman.GetComponent<Button>();
    }

    /// <summary> Назначение listener-ов на кнопки чиновников с показом UI визуала и активацией диалогов.</summary>
    private void AssignPlacemanButtonsListeners()
    {
        whitehallControllerUIHandler.AlchemyPlacemanButton.onClick.AddListener(()
            => ShowSpecializationContractsDetails(AlchemyPlaceman));
        whitehallControllerUIHandler.AlchemyPlacemanButton.onClick.AddListener(() => DialogueDependsOnTrust(AlchemyPlaceman));

        whitehallControllerUIHandler.JoineryPlacemanButton.onClick.AddListener(()
            => ShowSpecializationContractsDetails(JoineryPlaceman));
        whitehallControllerUIHandler.JoineryPlacemanButton.onClick.AddListener(() => DialogueDependsOnTrust(JoineryPlaceman));

        whitehallControllerUIHandler.SmitheryPlacemanButton.onClick.AddListener(()
            => ShowSpecializationContractsDetails(SmitheryPlaceman));
        whitehallControllerUIHandler.SmitheryPlacemanButton.onClick.AddListener(() => DialogueDependsOnTrust(SmitheryPlaceman));
    }

    /// <summary> Метод пересчает части выполненных контрактов и показа UI визуала.</summary>
    private void ShowSpecializationContractsDetails(Placeman placeman)
    {
        RecalculatePlacemenTrust();
        whitehallControllerUIHandler.ShowSpecializationContractsDetails(placeman.TrustNormalized, placeman.ClanCoatSprite);
    }

    /// <summary> Пересчет части выполненных контрактов для каждой пары "ключ-значение" в словаре чиновников.</summary>
    private void RecalculatePlacemenTrust()
    {
        foreach (var pair in specToPlacemanDictionary)
        {
            pair.Value.TrustNormalized = contractLogger.PlottedContractsCompleted[pair.Key] / contractLogger.MaxPlottedContractsAmount[pair.Key];
        }
    }
}