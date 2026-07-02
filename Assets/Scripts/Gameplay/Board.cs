using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] cells;

    public Cell[] Cells => cells;

    private void Awake()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Initialize(i);
        }
    }

    public void ResetBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ClearCell();
        }
    }
}