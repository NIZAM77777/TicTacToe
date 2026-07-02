using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text cellText;

    private int cellIndex;

    private bool isFilled = false;

    public void Initialize(int index)
    {
        cellIndex = index;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCellClicked);

        ClearCell();
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
    }

    public void ClearCell()
    {
        isFilled = false;
        cellText.text = "";
    }
}