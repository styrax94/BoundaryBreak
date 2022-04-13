using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    public enum Contain { Empty, Small, Large , Flag}
    [SerializeField]
    public Contain tileContains = Contain.Empty;
    public GameObject obOn;
    public bool hasPlayer;
    public bool hasFlag = false;
    public GameObject blockPrefab;
    public GameObject flagPrefab;
    public bool spawnUnmovableBlock;

    private void Start()
    {
        Create();
    }
    public int StateIndex()
    {
        return (int)tileContains;
    }
    public void SetPlayerPresence(bool playerPref)
    {
        if (tileContains == Contain.Flag) return;
        hasPlayer = playerPref;
    }
    public void SetState(int i, GameObject block)
    {
        switch (i)
        {
            case 0:
                tileContains = Contain.Empty;
                obOn = null;
                hasPlayer = true;
                break;
            case 1:
                tileContains = Contain.Small;
                obOn = block;
                hasPlayer = false;
                break;
            case 2:
                tileContains = Contain.Large;
                obOn = block;
                hasPlayer = false;
                break;
            

        }
    }
    public void Create()
    {
        Vector3 pos;
        Vector3 size;
        switch (tileContains)
        {
            case Contain.Large:
                pos = new Vector3(transform.position.x, transform.localScale.y / 2 + (blockPrefab.transform.localScale.y / 2) * 2, transform.position.z);
                size = blockPrefab.transform.localScale;
                size.y *= 2;
                obOn = GameObject.Instantiate(blockPrefab, pos, blockPrefab.transform.rotation);
                obOn.transform.localScale = size;
                obOn.GetComponent<Block>().BlockInit(!spawnUnmovableBlock, 2);
                break;
            case Contain.Small:
                pos = new Vector3(transform.position.x, transform.localScale.y / 2 + blockPrefab.transform.localScale.y / 2, transform.position.z);
                obOn = GameObject.Instantiate(blockPrefab, pos, blockPrefab.transform.rotation);
                obOn.GetComponent<Block>().BlockInit(!spawnUnmovableBlock, 1);
                break;
            case Contain.Flag:
                pos = new Vector3(transform.position.x, transform.localScale.y / 2 + blockPrefab.transform.localScale.y / 2, transform.position.z);
                obOn = GameObject.Instantiate(flagPrefab, pos, flagPrefab.transform.rotation);
                break;
        }
      
    }
    public void OnDrawGizmos()
    {
        if (spawnUnmovableBlock) Gizmos.color = Color.green;
        else if (tileContains == Contain.Flag) Gizmos.color = Color.black;
        else Gizmos.color = Color.blue;
        Vector3 pos;
        Vector3 size;
        switch (tileContains)
        {
            
            case Contain.Large:
                pos= new Vector3(transform.position.x,transform.localScale.y/2+ (blockPrefab.transform.localScale.y/2)*2, transform.position.z);
                size = blockPrefab.transform.localScale;
                size.y *= 2;
                Gizmos.DrawWireCube(pos, size);
                break;
            case Contain.Small:
                pos = new Vector3(transform.position.x, transform.localScale.y / 2 + blockPrefab.transform.localScale.y/2, transform.position.z);
                Gizmos.DrawWireCube(pos, blockPrefab.transform.localScale);
                break;
            case Contain.Flag:
                pos = new Vector3(transform.position.x, transform.localScale.y / 2 + blockPrefab.transform.localScale.y / 2, transform.position.z);
                Gizmos.DrawWireSphere(pos, 0.5f);
                break;
        }
    }
}
