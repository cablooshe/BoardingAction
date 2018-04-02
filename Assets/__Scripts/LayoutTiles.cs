using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class EnemyDef {
    public string str;
    public GameObject go;
}
public class LayoutTiles : MonoBehaviour {
    static private int maxRoomHeight = 100;
    static private int maxRoomWidth = 100; //PRESET VARIABLES TO REPLACE MAXIMUM ROOM SIZE, see BuildRoom(PT_Hashtable)

    public EnemyDef[] enemyDefinitions;

    public List<Texture2D> floorTexes;
    public Texture2D wallTex;

    static public LayoutTiles S;

    public TextAsset roomsText; //the rooms .xml file
    public string roomNumber = "0"; //current room # as string
    //doing this as string allows for encoding in XML and rooms 0-f
    public GameObject tilePrefab; //prefab for all tiles
    public GameObject playerPrefab;

    public bool ________________________________;

    private bool firstRoom = true;

    public PT_XMLReader roomsXMLR;
    public PT_XMLHashList roomsXML;
    public Tile[,] tiles;
    public Transform tileAnchor;

	// Use this for initialization
	void Start () {
        S = this;

        //make new object to be tileAnchor, keeps tiles tidy in hierarchy pane
        GameObject tAnc = new GameObject("TileAnchor");
        tileAnchor = tAnc.transform;

        //read the XML
        roomsXMLR = new PT_XMLReader(); //create a PT_XMLReader
        roomsXMLR.Parse(roomsText.text);//parse the xml file
        roomsXML = roomsXMLR.xml["xml"][0]["room"];//pull all the rooms

        //build the 0th room
        BuildRoom(roomNumber);

	}

    //Build a room based on room number
    public void BuildRoom(string rNumStr) {
        PT_XMLHashtable roomHT = null;
        for (int i = 0; i < roomsXML.Count; i++)
        {
            PT_XMLHashtable ht = roomsXML[i];
            if(ht.att("num") == rNumStr) {
                roomHT = ht;
                break;
            }
        }
        if(roomHT == null) {
            Utils.tr("ERROR", "LayoutTiles.BuildRoom()", "Room not found: " + rNumStr);
            return;
        }
        BuildRoom(roomHT);
    }        

    //build a room from the XML entry
    public void BuildRoom(PT_XMLHashtable room) {
        //Destroy any old tiles
        foreach(Transform t in tileAnchor) {
            Destroy(t.gameObject);
        }

        //move the mage out of the way
        //Mage.S.pos = Vector3.left * 100;

        //Mage.S.ClearInput();

        string rNumStr = room.att("num");



        //get texture names for the floors and walls from room attributes
        string floorTexStr = room.att("floor");
        string wallTexStr = room.att("wall");

        //split the room into rows of tiles based on carriage returns in the rooms.xml file
        string[] roomRows = room.text.Split('\n');

        //trim tabs from beginnings of lines.  we'll leave spaces and underscores to allow for non-rectangular rooms
        for(int i = 0; i<roomRows.Length;i++) {
            roomRows[i] = roomRows[i].Trim('\t');
        }
        //clear the tiles array
        tiles = new Tile[maxRoomWidth, maxRoomHeight]; //arbitrary max room size is 100x100
        //declare a number of local fields that we'll use later
        Tile ti;
        string type, rawType, tileTexStr;
        GameObject go;
        int height;
        float maxY = roomRows.Length - 1;

        //List<Portal> portals = new List<Portal>();


        //these loops scan through each tile of each row of the room
        for (int y = 0; y<roomRows.Length;y++) {
            for (int x = 0; x < roomRows[y].Length;x++) {
                //Set Defaults
                height = 0;
                tileTexStr = floorTexStr;
                //get the character representing the tile
                type = rawType = roomRows[y][x].ToString();
                switch (rawType) {
                    case " ": //empty space
                        continue;
                    case "_": //empty space
                        //just continue
                        continue;
                    case "."://default floor
                        //keep type = "."
                        break;
                    case "|": //default wall
                        height = 1;
                        break;
                    default:
                        //anything else will be a floor
                        type = ".";
                        break;
                }
                //instantitate the tile prefab
                go = Instantiate(tilePrefab) as GameObject;
                //set the texure for floor or wall based on room attributes
                if (type == ".") {
                    go.GetComponent<Renderer>().material.mainTexture = floorTexes[Random.Range(0, floorTexes.Count - 1)];
                    go.layer = 10; //set to floor layer
                } else if (type == "|") {
                    go.GetComponent<Renderer>().material.mainTexture = wallTex;

                }


                ti = go.GetComponent<Tile>();
                //set the parent transform to tileanchor
                ti.transform.parent = tileAnchor;
                //set the position of the tile
                ti.pos = new Vector3(x, maxY - y, 0);
                tiles[x, y] = ti; //add ti to the tiles 2d array

                //set the type and height of the tile
                ti.type = type;
                ti.height = height;
                


                //If the type is still rawType, continue to the next iteration
                if (rawType == type) continue;

                //check for specific entities in the room
                switch(rawType) {
                    case "X": //starting position for the Mage
                        if (firstRoom) {
                            //ti.pos = new Vector3(x, maxY - y, -0.05f);
                            GameObject unitToSpawn = Instantiate(playerPrefab) as GameObject;
                            unitToSpawn.transform.position = ti.pos; //Use the mage singleton
                            //ti.pos = new Vector3(x, maxY - y, 0);
                            firstRoom = false;
                        }
                        break;
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                        
                        break;
                    default:
                        break;
                }

                //MORE TO COME HERE...
            }
        }

        roomNumber = rNumStr;
    }

}
