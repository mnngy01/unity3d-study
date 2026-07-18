using UnityEngine;

public class RailPuzzleController : MonoBehaviour
{
    [SerializeField] private PuzzleSymbol[] symbols;

    private bool isSolved = false;

    // 퍼즐 검사
    public void CheckPuzzle()
    {
        if (isSolved)
            return;
        
        foreach (PuzzleSymbol symbol in symbols)
        {
            if (!symbol.IsMatched())
                return;
        }

        isSolved = true;

        Debug.Log("Puzzle Clear");

        // todo
        // Door.Open(); 
    }
}
