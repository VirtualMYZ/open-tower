﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteListLoader : MonoBehaviour {
    private static SpriteListLoader _instance;
    private static SpriteListLoader _prefab;

    [SerializeField]
    private Sprite[] enemies;

    [SerializeField]
    private Sprite[] boosters;

    [SerializeField]
    private Sprite wall;

    [SerializeField]
    private Sprite upStairs;

    [SerializeField]
    private Sprite downStairs;

    [SerializeField]
    private Sprite goldKey;

    [SerializeField]
    private Sprite blueKey;

    [SerializeField]
    private Sprite redKey;

    [SerializeField]
    private Sprite player;

    [SerializeField]
    private Sprite exit;

    [SerializeField]
    private Sprite goldDoor;

    [SerializeField]
    private Sprite blueDoor;

    [SerializeField]
    private Sprite redDoor;

    public static SpriteListLoader Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SpriteListLoader>();
            }
            if (_instance == null) {
                _instance = GameObject.Instantiate(Prefab);
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private static SpriteListLoader Prefab {
        get {
            if (_prefab == null) {
                _prefab = Resources.Load<SpriteListLoader>("Prefabs/Level Editor/Sprite List Loader");
            }
            return _prefab;
        }
    }

    public Sprite[] Enemies {
        get {
            return enemies;
        }
    }

    public Sprite[] Boosters {
        get {
            return boosters;
        }
    }

    public Sprite Wall {
        get {
            return wall;
        }
    }

    public Sprite UpStairs {
        get {
            return upStairs;
        }
    }

    public Sprite DownStairs {
        get {
            return downStairs;
        }
    }

    public Sprite GoldKey {
        get {
            return goldKey;
        }
    }

    public Sprite BlueKey {
        get {
            return blueKey;
        }
    }

    public Sprite RedKey {
        get {
            return redKey;
        }
    }

    public Sprite Player {
        get {
            return player;
        }
    }

    public Sprite Exit {
        get {
            return exit;
        }
    }

    public Sprite GoldDoor {
        get {
            return goldDoor;
        }
    }

    public Sprite BlueDoor {
        get {
            return blueDoor;
        }
    }

    public Sprite RedDoor {
        get {
            return redDoor;
        }
    }
}