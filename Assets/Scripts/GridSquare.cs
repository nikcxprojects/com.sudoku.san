using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Text _numberText;
    private int _number = 0;
    private int _square_index = -1;
    [SerializeField] private int _correct_number = 0;

    private bool _isSelected = false;
    private bool _hasDefaultValue = false;
    private bool _hasWrongValue = false;

    public bool isCorrectNumberSet { get { return _number == _correct_number; } }

    public bool isSelected => _isSelected;
    public bool hasWrongValues => _hasWrongValue;
    public bool hasDefaultValue => _hasDefaultValue;
    public void SetHasDefaultValue(bool value) => _hasDefaultValue = value;
    public void SetSquareIndex(int index) => _square_index = index;
    public void SetCorrectNumber(int number)
    {
        _correct_number = number;
        _hasWrongValue = false;
    } 

    public void SetCorrectNumber()
    {
        _number = _correct_number;
        DisplayText();
    }

    private void Start()
    {
        _isSelected = false;
    }

    public void DisplayText()
    {
        if (_number <= 0)
            _numberText.text = " ";
        else
            _numberText.text = _number.ToString();
    }
    public void SetNubmer(int number)
    {
        _number = number;
        DisplayText();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isSelected = true;
        GameEvents.SquareSelectedMethod(_square_index);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
    }

    public void OnSetNumber(int number)
    {
        if (_isSelected && !_hasDefaultValue) { 
            SetNubmer(number);

            if (number != _correct_number)
            {
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;
                _hasDefaultValue = true;
                _hasWrongValue = true;
                GameEvents.OnWrongNumberMethod();
            } else
            {
                _hasDefaultValue = true;
                var colors = this.colors;
                colors.normalColor = Color.white;
                this.colors = colors;
                _hasWrongValue = false;

            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if(square_index != _square_index)
            _isSelected = false;
    }

    public void SetSquareColor(Color col)
    {
        var colors = this.colors;
        colors.normalColor = col;
        this.colors = colors;
    }
}
