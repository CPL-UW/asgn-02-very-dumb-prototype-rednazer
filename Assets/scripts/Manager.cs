using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Manager : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject heldItemPrefab;
    public Tilemap inventory;
    public List<Sprite> itemSprites;

    GameObject[] items;
    GameObject heldItem;
    GameObject cursorFollowedItem;
    int heldItemLoc;

    // Start is called before the first frame update
    void Start() {
        items = new GameObject[28];
        heldItem = null;
        heldItemLoc = -1;
        addItems();
    }

    void addItems() {
        int location = 0;
        foreach (Sprite pref in itemSprites) {
            // Sets the location of the next gameObject
            Vector3 nextLoc = new Vector3(0, 0, 0);
            if(location / 7 == 0) {
                nextLoc.x = -9 + (2 * location);
                nextLoc.y = 3;
            } else if(location / 7 == 1) {
                nextLoc.x = -9 + (2 * (location % 7));
                nextLoc.y = 3;
            }

            GameObject newObj = Instantiate(itemPrefab, nextLoc, itemPrefab.transform.rotation);
            newObj.GetComponent<SpriteRenderer>().sprite = pref;
            items[location] = newObj;

            location++;
        }
    //    for(int i = location; i < items.Length; i++) {
  //          items[i] = null;
//        }
    }

    // Update is called once per frame
    void Update() {
        // Attemps to pick up item
        if (Input.GetMouseButtonDown(0)) {
            Vector3Int mousePos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // gameObject.GetComponent<Tilemap>().GetTile(mousePos); // I don't know if we need the tile at mouse positon
            Vector3Int tilePos = mousePos;

            // The tile scale is 2, so to get the clicked tile I divide by 2 (and floor the result)
            if (tilePos.x < 0 && tilePos.x % 2 == -1) {
                tilePos.x = (tilePos.x / 2) - 1;
            } else {
                tilePos.x = tilePos.x / 2;
            }
            if (tilePos.y < 0 && tilePos.y % 2 == -1) {
                tilePos.y = (tilePos.y / 2) - 1;
            } else {
                tilePos.y = tilePos.y / 2;
            }

            // If nothing is held, then picks up item. Otherwise swaps two items
            if(heldItem == null) {
                // Only sets as held item if item is not null
                if (items[coordToIndex(tilePos)] != null) {
                    heldItemLoc = coordToIndex(tilePos);
                    heldItem = items[coordToIndex(tilePos)];
                    heldItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
                    cursorFollowedItem = Instantiate(heldItemPrefab, heldItem.transform.position, heldItemPrefab.transform.rotation);
                    cursorFollowedItem.GetComponent<SpriteRenderer>().sprite = heldItem.GetComponent<SpriteRenderer>().sprite;
                    Debug.Log("Setting item to move: " + heldItem);
                }
            } else if (coordToIndex(tilePos) != -1 && coordToIndex(tilePos) != heldItemLoc) {
                if (items[coordToIndex(tilePos)] != null) {
                    swapItems(coordToIndex(tilePos), items[coordToIndex(tilePos)]);
                } else {
                    moveItem(coordToIndex(tilePos));
                }
                heldItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                Destroy(cursorFollowedItem);
                heldItem = null;
                heldItemLoc = -1;
                Debug.Log("Swapping items" + items[0] + ", " + items[1] + ", " + items[2] + ", " + items[3]);
            }


            //Debug.Log("Clicked at: " + mousePos + ", Tile Pos: " + tilePos);
            Debug.Log("Clicked at: " + tilePos + ", item Pos: " + coordToIndex(tilePos));
        }



    }
    
    void swapItems(int index, GameObject obj) {
        // Swaps obj to heldObj positions
        Vector3 tempLoc = obj.transform.position;
        obj.transform.position = heldItem.transform.position;
        heldItem.transform.position = tempLoc;

        // Then swaps heldObj to obj item index
        items[heldItemLoc] = obj;
        items[index] = heldItem;
    }

    void moveItem(int index) {
        // Moves held item to location and sets position in array
        heldItem.transform.position = indexToCoord(index);
        items[index] = heldItem;

        // Removes held item from previous location
        items[heldItemLoc] = null;
    }

    int coordToIndex(Vector3Int pos) {
        // If not in grid
        if(pos.x < -5 || pos.x > 1) {
            return -1;
        } else if(pos.y < -2 || pos.y > 1) {
            return -1;
        }

        int valX = pos.x + 5;
        int valY = (pos.y - 1) * -7;
        return valX + valY;
    }

    Vector3 indexToCoord(int index) {
        Vector3 coords = new Vector3(0, 0, 0);
        if (index / 7 == 0) {
            coords.x = -9 + (2 * index);
            coords.y = 3;
        } else if (index / 7 == 1) {
            coords.x = -9 + (2 * (index % 7));
            coords.y = 1;
        } else if(index / 7 == 2) {
            coords.x = -9 + (2 * (index % 7));
            coords.y = -1;
        } else if(index / 7 == 3) {
            coords.x = -9 + (2 * (index % 7));
            coords.y = -3;
        }
        return coords;
    }

    Tile spriteToTile(Sprite s) {
        Tile t = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
        t.sprite = s;
        return t;
    }
}
