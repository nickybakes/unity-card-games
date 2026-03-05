using TMPro;
using UnityEngine;

/// <summary>
/// Scriptable Object for setting how cards visually look.
/// </summary>
[CreateAssetMenu(fileName = "CardVisualProfile", menuName = "Scriptable Objects/Card Visual Profile")]
public class CardVisualProfile : ScriptableObject
{

    /// <summary>
    /// The sprites to show for each suit.
    /// </summary>
    #region Suit Sprites
    [field: SerializeField] public Sprite SpadeSprite { get; private set; }
    [field: SerializeField] public Sprite HeartSprite { get; private set; }
    [field: SerializeField] public Sprite ClubSprite { get; private set; }
    [field: SerializeField] public Sprite DiamondSprite { get; private set; }

    public Sprite[] SuitSprites { get => new Sprite[] { SpadeSprite, HeartSprite, ClubSprite, DiamondSprite }; }

    #endregion


    /// <summary>
    /// The colors to tint the suit sprites for each suit.
    /// </summary>
    #region Suit Sprite Colors

    [field: SerializeField] public Color SpadeSpriteColor { get; private set; }
    [field: SerializeField] public Color HeartSpriteColor { get; private set; }
    [field: SerializeField] public Color ClubSpriteColor { get; private set; }
    [field: SerializeField] public Color DiamondSpriteColor { get; private set; }

    public Color[] SuitSpriteColors { get => new Color[] { SpadeSpriteColor, HeartSpriteColor, ClubSpriteColor, DiamondSpriteColor }; }

    #endregion

    /// <summary>
    /// The color gradients to put on the card value texts for each suit.
    /// </summary>
    #region  Suit Text Gradients
    [field: SerializeField] public TMP_ColorGradient SpadeTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient HeartTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient ClubTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient DiamondTextColor { get; private set; }

    public TMP_ColorGradient[] SuitTextColors { get => new TMP_ColorGradient[] { SpadeTextColor, HeartTextColor, ClubTextColor, DiamondTextColor }; }

    #endregion

    /// <summary>
    /// The extra, custom sprites for each face card and whether they should also be tinted or not.
    /// </summary>
    #region Face Card Visuals
    [field: SerializeField] public Sprite JackCustomSprite { get; private set; }
    [field: SerializeField] public Sprite QueenCustomSprite { get; private set; }
    [field: SerializeField] public Sprite KingCustomSprite { get; private set; }
    [field: SerializeField] public bool TintCustomFaceSpriteWithSuitColor { get; private set; }
    [field: SerializeField] public bool DisableFaceCornerStyle { get; private set; }

    #endregion

}
