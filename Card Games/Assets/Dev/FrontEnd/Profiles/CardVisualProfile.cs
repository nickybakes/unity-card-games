using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CardVisualProfile", menuName = "Scriptable Objects/Card Visual Profile")]
public class CardVisualProfile : ScriptableObject
{
    [field: SerializeField] public Sprite SpadeSprite { get; private set; }
    [field: SerializeField] public Sprite HeartSprite { get; private set; }
    [field: SerializeField] public Sprite ClubSprite { get; private set; }
    [field: SerializeField] public Sprite DiamondSprite { get; private set; }

    public Sprite[] SuitSprites { get => new Sprite[] { SpadeSprite, HeartSprite, ClubSprite, DiamondSprite }; }

    [field: SerializeField] public Color SpadeSpriteColor { get; private set; }
    [field: SerializeField] public Color HeartSpriteColor { get; private set; }
    [field: SerializeField] public Color ClubSpriteColor { get; private set; }
    [field: SerializeField] public Color DiamondSpriteColor { get; private set; }

    public Color[] SuitSpriteColors { get => new Color[] { SpadeSpriteColor, HeartSpriteColor, ClubSpriteColor, DiamondSpriteColor }; }

    [field: SerializeField] public TMP_ColorGradient SpadeTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient HeartTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient ClubTextColor { get; private set; }
    [field: SerializeField] public TMP_ColorGradient DiamondTextColor { get; private set; }

    public TMP_ColorGradient[] SuitTextColors { get => new TMP_ColorGradient[] { SpadeTextColor, HeartTextColor, ClubTextColor, DiamondTextColor }; }

    [field: SerializeField] public Sprite JackCustomSprite { get; private set; }
    [field: SerializeField] public Sprite QueenCustomSprite { get; private set; }
    [field: SerializeField] public Sprite KingCustomSprite { get; private set; }
    [field: SerializeField] public bool TintCustomFaceSpriteWithSuitColor { get; private set; }
    [field: SerializeField] public bool DisableFaceCornerStyle { get; private set; }


}
