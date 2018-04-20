using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo {

    public static int gold, exp, currentSceneIndex;
    public static List<Squad> squads;

    public static int Gold {
        get {
            return gold;
        }
        set {
            gold = value;
        }
    }

    public static int Exp {
        get {
            return exp;
        }
        set {
            exp = value;
        }
    }

    public static int CurrentSceneIndex {
        get {
            return currentSceneIndex;
        }
        set {
            currentSceneIndex = value;
        }
    }
}
