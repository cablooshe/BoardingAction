using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo {

    public static int gold;
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
}
