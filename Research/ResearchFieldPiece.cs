using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchFieldPiece : MonoBehaviour
{
    [HideInInspector]
    public Image pieceImage;

    public bool isHighlighted;

    public void Initialize()
    {
        pieceImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHighlight(bool state)
    {
        pieceImage.color = state ? Color.red : Color.white;
    }

}
