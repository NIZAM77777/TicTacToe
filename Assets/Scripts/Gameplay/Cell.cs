using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text cellText;
    [SerializeField] private Shadow glowEffect;

    private int cellIndex;

    private bool isFilled = false;

    public void Initialize(int index)
    {
        cellIndex = index;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCellClicked);

        ClearCell();
    }

    public void ShowGlow()
    {
        glowEffect.enabled = true;
    }

    public void HideGlow()
    {
        glowEffect.enabled = false;
    }

    private void OnCellClicked()
    {
        if (isFilled)
            return;

        GameManager.Instance.SelectCell(cellIndex);
    }

    public void SetSymbol(string symbol)
    {
        isFilled = true;

        cellText.text = symbol;

        button.interactable = false;
    }

    public void ClearCell()
    {
        isFilled = false;
        cellText.text = "";
        button.interactable = true;

        HideGlow();
    }
    
}