using Godot;
using Godot.Collections;
using System;

public class MainSceneControl : Control
{
    private const int IndexLength = 5;
    private readonly string[] _defaultResultText =
    {
        "(國文加權後分數)",
        "(英文加權後分數)",
        "(數學加權後分數)",
        "(專一加權後分數)",
        "(專二加權後分數)",
        "(加權後總分)"
    };

    private Array<LineEdit> _scoreLineEdits = new Array<LineEdit>();
    private Array<LineEdit> _multipleLineEdits;
    private Array<Label> _resultLabels;

    private Label _error, _total;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _error = this.GetNode<Label>("Error");
        _total = this.GetNode<Label>("Total");

        _multipleLineEdits = new Array<LineEdit>(this.GetNode("MultiplesTitle").GetChildren());
        _resultLabels = new Array<Label>(this.GetNode("Results").GetChildren());

        for (int i = 0; i < IndexLength; i++)
            _scoreLineEdits.Add(this.GetNode("Scores").GetChild(i).GetChild<LineEdit>(0));
    }

    // Signal Methods
    private void OnCalculatePressed()
    {
        float[] scores = new float[IndexLength];
        float[] multiples = new float[IndexLength];
        float totalResult = 0;

        for (int i = 0; i < IndexLength; i++)
        {
            bool isEmpty = _scoreLineEdits[i].Text.Equals(string.Empty) ||
                           _multipleLineEdits[i].Text.Equals(string.Empty);
            bool isNumeric = float.TryParse(_scoreLineEdits[i].Text, out scores[i]) &&
                             float.TryParse(_multipleLineEdits[i].Text, out multiples[i]);

            // Error Messages
            if (isEmpty)
            {
                _error.Text = "請勿讓任一欄位為空";
                return;
            }
            if(!isNumeric)
            {
                _error.Text = "請勿輸入數字以外的字元";
                return;
            }

            float result = scores[i] * multiples[i];
            _resultLabels[i].Text = result.ToString();
            totalResult += result;
        }

        _total.Text = totalResult.ToString();
        _error.Text = "計算成功";
    }

    private void OnClearPressed()
    {
        for(int i = 0; i < IndexLength; i++)
        {
            _scoreLineEdits[i].Text = string.Empty;
            _multipleLineEdits[i].Text = string.Empty;
            _resultLabels[i].Text = _defaultResultText[i];
        }
        _total.Text = _defaultResultText[5];
        _error.Text = string.Empty;
    }

    private void OnWatermarkMetaClicked(object meta) => OS.ShellOpen((string)meta);
}
