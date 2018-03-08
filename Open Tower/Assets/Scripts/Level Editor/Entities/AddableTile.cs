﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AddableTile : MonoBehaviour {
    private const int MAX_VALUE_FOR_ENEMY_STATS = 99999;
    private const int MAX_VALUE_FOR_BOOSTER_STATS = 9999;

    // Static types have no customizable values
    private static readonly HashSet<TileType> STATIC_TYPES = new HashSet<TileType>() {
        TileType.WALL,
        TileType.UP_STAIRS,
        TileType.DOWN_STAIRS,
        TileType.GOLD_KEY,
        TileType.BLUE_KEY,
        TileType.RED_KEY,
        TileType.PLAYER,
        TileType.EXIT,
        TileType.GOLD_DOOR,
        TileType.BLUE_DOOR,
        TileType.RED_DOOR
    };

    private static readonly IDictionary<int, StatType> MAPPING = new Dictionary<int, StatType>() {
        { 0, StatType.LIFE },
        { 1, StatType.POWER },
        { 2, StatType.DEFENSE },
        { 3, StatType.EXPERIENCE }
    };

    private static EntitiesPanel _parentPanel;
    private static FloorPanel _floorPanel;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private TileType tile;

    [SerializeField]
    private PlaceType placementType;

    [SerializeField]
    private Button changeSprite;

    [SerializeField]
    private Button delete;

    [SerializeField]
    private InputField life;

    [SerializeField]
    private InputField power;

    [SerializeField]
    private InputField defense;

    [SerializeField]
    private InputField experience;

    [SerializeField]
    private InputField boostedAmount;

    /// <summary>
    /// The booster stat icons, MUST be parallel to StatType!
    /// </summary>
    [SerializeField]
    private Sprite[] boosterStatIcons;

    [SerializeField]
    private Image boosterStatIcon;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Element elementPrefab;

    private StatType boostedStatType;

    private int spriteID;

    public bool IsStaticTileType {
        get {
            return STATIC_TYPES.Contains(tile);
        }
    }

    public TileType TileType {
        get {
            return tile;
        }
    }

    public Sprite Sprite {
        get {
            return image.sprite;
        }
    }

    public int EnemyLife {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(life.text);
        }
    }

    public int EnemyPower {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(power.text);
        }
    }

    public int EnemyDefense {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(defense.text);
        }
    }

    public int EnemyStars {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(experience.text);
        }
    }

    public StatType BoostedStatType {
        get {
            Util.Assert(tile == TileType.BOOSTER, "Expected Booster type, was {0} instead.", this.tile);
            return boostedStatType;
        }
    }

    public int BoostedAmount {
        get {
            Util.Assert(tile == TileType.BOOSTER, "Expected Booster type, was {0} instead.", this.tile);
            return int.Parse(boostedAmount.text);
        }
    }

    public int SpriteID {
        get {
            Util.Assert(!IsStaticTileType, "Tile type is not dynamic.");
            return spriteID;
        }
    }

    private static EntitiesPanel ParentPanel {
        get {
            if (_parentPanel == null) {
                _parentPanel = EntitiesPanel.Instance;
            }
            return _parentPanel;
        }
    }

    private static FloorPanel FloorPanel {
        get {
            if (_floorPanel == null) {
                _floorPanel = FloorPanel.Instance;
            }
            return _floorPanel;
        }
    }

    private IEnumerable<Element> AllAssociatedElementsInLevel {
        get {
            return FloorPanel.FloorParent.GetComponentsInChildren<Element>(true).Where(e => e.IsSource(this));
        }
    }

    private IEnumerable<Element> AllAssociatedElementsInFloor {
        get {
            return FloorPanel.Selected.Associated.GetComponentsInChildren<Element>(true).Where(e => e.IsSource(this));
        }
    }

    // Toggle through all boostable stats
    public void IterateBoosterStat() {
        boostedStatType = boostedStatType.Next();
        boosterStatIcon.sprite = boosterStatIcons[(int)BoostedStatType];
    }

    // TODO
    public void Delete() {
        Destroy(gameObject);
        if (EntitiesPanel.Instance.LastSelected == this) {
            EntitiesPanel.Instance.LastSelected = null;
        }
        foreach (Element e in AllAssociatedElementsInLevel) {
            Destroy(e.gameObject);
        }
    }

    public void SelectThisTile() {
        EntitiesPanel.Instance.LastSelected = this;
    }

    public void OnBoosterValueChange(string value) {
        Util.Assert(tile == TileType.BOOSTER, "Expected Booster type but was {0}", tile);
        ClampStats(boostedAmount, 1);
    }

    public void OnLifeChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(life, 1);
    }

    public void OnPowerChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(power, 0);
    }

    public void OnDefenseChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(defense, 0);
    }

    public void OnExperienceChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(experience, 0);
    }

    public void CreateElement(Transform parent) {
        switch (placementType) {
            case PlaceType.NO_RESTRICTION:
                // do nothing
                break;

            case PlaceType.UNIQUE_PER_FLOOR:
                foreach (Element e in AllAssociatedElementsInFloor) {
                    Destroy(e.gameObject);
                }
                break;

            case PlaceType.UNIQUE_PER_LEVEL:
                foreach (Element e in AllAssociatedElementsInLevel) {
                    Destroy(e.gameObject);
                }
                break;
        }
        Element newElement = Instantiate(elementPrefab, parent);
        newElement.Init(this);
    }

    private void ClampStats(InputField field, int min) {
        Util.ClampField(field, min, MAX_VALUE_FOR_ENEMY_STATS);
    }

    private int GetEnemyStatCount(StatType stat) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
        InputField field = null;
        switch (stat) {
            case StatType.LIFE:
                field = life;
                break;

            case StatType.POWER:
                field = power;
                break;

            case StatType.DEFENSE:
                field = defense;
                break;

            case StatType.EXPERIENCE:
                field = experience;
                break;
        }
        int count = 0;
        Util.Assert(int.TryParse(field.text, out count), "Failed to parse {0}", field.text);
        return count;
    }

    private void Start() {
        if (this.TileType == TileType.BOOSTER) {
            this.image.sprite = SpriteList.GetBooster(spriteID);
        } else if (this.TileType == TileType.ENEMY) {
            this.image.sprite = SpriteList.GetEnemy(spriteID);
        }
        if (changeSprite != null) {
            changeSprite.onClick.AddListener(new UnityEngine.Events.UnityAction(
                () => {
                    SpritePicker.Instance.Activate(this.TileType, pickable => {
                        image.sprite = pickable.Sprite;
                        spriteID = pickable.ID;
                        foreach (Element e in AllAssociatedElementsInLevel) {
                            e.Sprite = pickable.Sprite;
                        }
                    });
                }
                ));
        }
    }

    private void DeleteAllPlacedElementsInLevel() {
        foreach (Element e in AllAssociatedElementsInLevel) {
            if (e.IsSource(this)) {
                Destroy(e.gameObject);
            }
        }
    }

    private void DeleteAllPlacedElementsInCurrentFloor() {
        foreach (Element e in AllAssociatedElementsInFloor) {
            if (e.IsSource(this)) {
                Destroy(e.gameObject);
            }
        }
    }

    private void Update() {
        outline.effectColor = (ParentPanel.LastSelected == this) ? Color.green : Color.white;
    }
}