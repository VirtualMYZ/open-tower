﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    [SerializeField]
    private int life;

    [SerializeField]
    private int power;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int experience;

    private int steps;

    public int Life {
        get {
            return life;
        }
    }

    public int Power {
        get {
            return power;
        }
    }

    public int Defense {
        get {
            return defense;
        }
    }

    public int Experience {
        get {
            return experience;
        }
    }

    public int StepCount {
        get {
            return steps;
        }
    }

    public void AddToLife(int amount) {
        this.life += amount;
    }

    public void AddToPower(int amount) {
        this.power += amount;
    }

    public void AddToDefense(int amount) {
        this.defense += amount;
    }

    public void AddToExperience(int amount) {
        this.experience += amount;
    }

    public void IncrementSteps() {
        this.steps++;
    }

    private void Start() {
        Util.Assert(Life > 0, "Life must be positive number.");
        Util.Assert(Power > 0, "Power must be positive number.");
        Util.Assert(Defense > 0, "Defense must be positive number.");
        Util.Assert(Experience >= 0, "Experience must be nonnegative.");
    }
}