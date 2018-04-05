using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo {

    public static int gold, exp;
    public static GameObject[] soldiers;

    public static int Gold {
        get {
            return gold;
        }
        set {
            gold = value;
        }
    }

    public static GameObject[] Soldiers {
        get {
            return soldiers;
        }
        set {
            soldiers = value;
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
}
