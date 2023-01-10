using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SudokuGrid : MonoBehaviour
{
    [SerializeField] private GameObject _gridSquarePrefab;

    [SerializeField] private int _columnsGrid = 0;
    [SerializeField] private int _rowsGrid = 0;
    [SerializeField] private float _square_offset = 0.0f;
    [SerializeField] private float _squaer_scale = 1.0f;
    [SerializeField] private float _square_gap = 0.1f;
    [SerializeField] private Vector2 _startPosition = new Vector2(0.0f, 0.0f);

    [SerializeField] private Color _lineHighlight = Color.red;
    [SerializeField] private int _hintCount = 9;
    [SerializeField] private Text _hintText;

    private List<GameObject> _initializedSquares = new List<GameObject>();
    private int _selected_grid_data = -1;

    private void Start()
    {
        CreateGrid();

        if (!PlayerPrefs.HasKey(PrefsKey.difficulity))
            PlayerPrefs.SetString(PrefsKey.difficulity, "Easy");

        if (!PlayerPrefs.HasKey(PrefsKey.hints))
            PlayerPrefs.SetInt(PrefsKey.hints, 9);

        _hintCount = PlayerPrefs.GetInt(PrefsKey.hints);
        _hintText.text = $"Hint ({_hintCount})";
        SetGridNumber(PlayerPrefs.GetString(PrefsKey.difficulity));
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSqauresPosition();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;
        for(int row = 0; row < _rowsGrid; row++)
        {
            for(int column = 0; column < _columnsGrid; column++)
            {
                _initializedSquares.Add(Instantiate(_gridSquarePrefab) as GameObject);
                _initializedSquares[_initializedSquares.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                _initializedSquares[_initializedSquares.Count - 1].transform.SetParent(this.transform);
                _initializedSquares[_initializedSquares.Count - 1].transform.localScale = new Vector3(_squaer_scale, _squaer_scale, _squaer_scale);
                square_index++;
            }
        }
    }

    private void SetSqauresPosition()
    {
        var square_rect = _initializedSquares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 sqaure_gap_number = Vector2.zero;
        bool rowMoved = false;

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + _square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + _square_offset;

        int column_number = 0;
        int row_number = 0;

        foreach(GameObject square in _initializedSquares)
        {
            if(column_number + 1 > _columnsGrid)
            {
                row_number += 1;
                column_number = 0;
                sqaure_gap_number.x = 0;
                rowMoved = false;
            }

            var pos_x_offset = offset.x * column_number + (sqaure_gap_number.x * _square_gap);
            var pos_y_offset = offset.y * row_number +(sqaure_gap_number.y * _square_gap);

            if(column_number > 0 && column_number % 3 == 0)
            {
                sqaure_gap_number.x++;
                pos_x_offset += _square_gap;
            }

            if(row_number > 0 && row_number % 3 == 0 && !rowMoved)
            {
                rowMoved = true;
                sqaure_gap_number.y++;
                pos_y_offset += _square_gap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(_startPosition.x + pos_x_offset, _startPosition.y - pos_y_offset);
            column_number += 1;
        }
    }

    private void SetGridNumber(string level)
    {
        _selected_grid_data = Random.Range(0, SudokuData.instance.sudoku_game[level].Count);
        var data = SudokuData.instance.sudoku_game[level][_selected_grid_data];

        SetGridSquareData(data);
    }

    private void SetGridSquareData(SudokuData.SudokuBoardData data)
    {
        for(int index = 0; index < _initializedSquares.Count; index++)
        {
            _initializedSquares[index].GetComponent<GridSquare>().SetNubmer(data.unsolved_data[index]);
            _initializedSquares[index].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[index]);
            _initializedSquares[index].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolved_data[index] != 0 && data.unsolved_data[index] == data.solved_data[index]);
        }
    }

    public void OnSquareSelected(int square_index)
    {
        var horizontal_line = LineIndicator.instance.GetHorizontalLine(square_index);
        var vertical_line = LineIndicator.instance.GetVerticalLine(square_index);
        var square = LineIndicator.instance.GetSquare(square_index);

        SetSquaresColor(LineIndicator.instance.GetAllSquaresIndex(), Color.white);
        SetSquaresColor(square, _lineHighlight);
        SetSquaresColor(horizontal_line, _lineHighlight);
        SetSquaresColor(vertical_line, _lineHighlight);
    }

    private void SetSquaresColor(int[] data, Color col)
    {
        foreach(var index in data)
        {
            var comp = _initializedSquares[index].GetComponent<GridSquare>();
            if (!comp.isSelected && !comp.hasWrongValues)
                comp.SetSquareColor(col);
        }
    }

    private void CheckBoardCompleted(int number)
    {
        foreach(var square in _initializedSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            if (!comp.isCorrectNumberSet)
                return;
        }

        GameEvents.OnBoardCompletedMethod();
    }

    public void SolveSudoku()
    {
        foreach(var square in _initializedSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber();
        }

        CheckBoardCompleted(0);
    }

    public void HintSudoku()
    {
        if (_hintCount == 0)
            return;

        int index = Random.Range(0, _initializedSquares.Count);
        if (!_initializedSquares[index].GetComponent<GridSquare>().hasDefaultValue)
        {
            _initializedSquares[index].GetComponent<GridSquare>().SetCorrectNumber();
            _hintCount -= 1;
            _hintText.text = $"Hint ({_hintCount})";
            
        }
        else
            HintSudoku();

    }

    public void Exit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnUpdateSquareNumber += CheckBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnUpdateSquareNumber -= CheckBoardCompleted;
    }
}
