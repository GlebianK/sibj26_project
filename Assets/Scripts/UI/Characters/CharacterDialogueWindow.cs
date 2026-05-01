using Az7.UI;
using System.Threading;
using TMPro;
using UnityEngine;

public class CharacterDialogueWindow : UI_ViewBase
{
    public override UI_ViewKey ViewKey => UI_ViewKey.None;

    [field: SerializeField] public CharacterKey CharacterKey { get; private set; }
    [SerializeField] private TMP_Text _text;

    public void SetText(string text)
    {
        _text.text = text;
    }
}
