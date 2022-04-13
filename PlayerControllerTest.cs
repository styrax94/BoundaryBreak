using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public GridMakerTest grid;
    bool action = false;
    public bool isPlayerOne;
    public Vector2Int currentPos;
    float groundPos;
    public int facing = 1;
    public GameObject moveblock;
    public bool grabbingPlayer;
    public GameObject otherPlayer;
    PlayerAnimationController playerAnimationController;
    public UIManager uiHandler;

    public void MovePlayer(Vector2Int dir, int face, PManagerTest manager)
    {
        if (action) return;

        ChangeOrientation(face);
        playerAnimationController.moving = true;
        StartCoroutine(MovePlayerCoro(dir, face, manager));
    }
    public void ChangeFace(int face)
    {
        facing = face;
        ChangeOrientation(face);

    }

    void ChangeOrientation(int dir)
    {

        switch (dir)
        {

            case 0:

                transform.eulerAngles = new Vector3(0, -90f, 0);
                break;
            case 1:

                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 2:

                transform.eulerAngles = new Vector3(0, 90f, 0);
                break;
            case 3:

                transform.eulerAngles = new Vector3(0, 180f, 0);
                break;


        }

    }
    public bool GrabObject(bool playerOne)
    {

        if (grid.grid[currentPos.x, currentPos.y].GetComponent<TileTest>().StateIndex() != 0) return false;
        Vector2Int target;
        grabbingPlayer = false;
        switch (facing)
        {
            case 0:
                target = currentPos + Vector2Int.up;


                if (grid.CheckInsideGrid(target))
                {
                    TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                    if (tile.StateIndex() == 1 || tile.StateIndex() == 2)
                    {
                        Block block = tile.obOn.GetComponent<Block>();
                        if (block.isMoveable)
                        {
                            moveblock = tile.obOn;
                            if (playerOne)
                            {
                                uiHandler.ChangeToGrabUI(true, true, true);
                            }
                            else
                            {
                                uiHandler.ChangeToGrabUI(true, false, false);
                            }
                            return true;
                        }
                    }
                    else if (tile.hasPlayer)
                    {

                        grabbingPlayer = true;
                        moveblock = otherPlayer;
                        if (playerOne)
                        {
                            uiHandler.ChangeToGrabUI(true, true, true);
                        }
                        else
                        {
                            uiHandler.ChangeToGrabUI(true, false, false);
                        }
                        return true;

                    }
                }
                break;
            case 1:
                target = currentPos + Vector2Int.right;


                if (grid.CheckInsideGrid(target))
                {
                    TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                    if (tile.StateIndex() == 1 || tile.StateIndex() == 2)
                    {
                        Block block = tile.obOn.GetComponent<Block>();
                        if (block.isMoveable)
                        {
                            moveblock = tile.obOn;
                            if (playerOne)
                            {
                                uiHandler.ChangeToGrabUI(true, false, true);
                            }
                            else
                            {
                                uiHandler.ChangeToGrabUI(true, true, false);
                            }
                            return true;
                        }

                    }
                    else if (tile.hasPlayer)
                    {

                        grabbingPlayer = true;
                        moveblock = otherPlayer;
                        if (playerOne)
                        {
                            uiHandler.ChangeToGrabUI(true, false, true);
                        }
                        else
                        {
                            uiHandler.ChangeToGrabUI(true, true, false);
                        }
                        return true;
                    }
                }
                break;
            case 2:
                target = currentPos + Vector2Int.down;


                if (grid.CheckInsideGrid(target))
                {
                    TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                    if (tile.StateIndex() == 1 || tile.StateIndex() == 2)
                    {
                        Block block = tile.obOn.GetComponent<Block>();
                        if (block.isMoveable)
                        {
                            moveblock = tile.obOn;
                            if (playerOne)
                            {
                                uiHandler.ChangeToGrabUI(true, true, true);
                            }
                            else
                            {
                                uiHandler.ChangeToGrabUI(true, false, false);
                            }
                            return true;
                        }

                    }
                    else if (tile.hasPlayer)
                    {

                        grabbingPlayer = true;
                        moveblock = otherPlayer;
                        if (playerOne)
                        {
                            uiHandler.ChangeToGrabUI(true, true, true);
                        }
                        else
                        {
                            uiHandler.ChangeToGrabUI(true, false, false);
                        }
                        return true;

                    }
                }
                break;
            case 3:
                target = currentPos + Vector2Int.left;


                if (grid.CheckInsideGrid(target))
                {
                   TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                    if (tile.StateIndex() == 1 || tile.StateIndex() == 2)
                    {
                        Block block = tile.obOn.GetComponent<Block>();
                        if (block.isMoveable)
                        {
                            moveblock = tile.obOn;
                            if (playerOne)
                            {
                                uiHandler.ChangeToGrabUI(true, false, true);
                            }
                            else
                            {
                                uiHandler.ChangeToGrabUI(true, true, false);
                            }
                            return true;
                        }

                    }
                    else if (tile.hasPlayer)
                    {

                        grabbingPlayer = true;
                        moveblock = otherPlayer;
                        if (playerOne)
                        {
                            uiHandler.ChangeToGrabUI(true, false, true);
                        }
                        else
                        {
                            uiHandler.ChangeToGrabUI(true, true, false);
                        }
                        return true;

                    }
                }
                break;

        }
        return false;
    }
    public void ReleaseObject()
    {
        moveblock = null;
    }
    public void MoveWithBlock(Vector2Int dir, bool push, PManagerTest manager)
    {
        if (push)
        {

            playerAnimationController.pushing = true;

        }
        else
        {

            playerAnimationController.pulling = true;

        }
        StartCoroutine(MoveWithBlockCoro(dir, push, manager));

    }

    IEnumerator MoveWithBlockCoro(Vector2Int dir, bool push, PManagerTest manager)
    {
        float journey = 0;
        Vector3 pStartPos = transform.position;
        Vector3 bStartPos = moveblock.transform.position;
        Vector3 pEndPos, bEndPos;
        TileTest pCurrentTile = grid.grid[currentPos.x, currentPos.y].GetComponent<TileTest>();
        TileTest bCurrentTile, endTile;

        if (push)
        {
            bCurrentTile = grid.grid[(currentPos + dir).x, (currentPos + dir).y].GetComponent<TileTest>();
            endTile = grid.grid[(currentPos + (dir * 2)).x, (currentPos + (dir * 2)).y].GetComponent<TileTest>();
            pEndPos = new Vector3(bCurrentTile.transform.position.x, transform.position.y, bCurrentTile.transform.position.z);
            bEndPos = new Vector3(endTile.transform.position.x, moveblock.transform.position.y, endTile.transform.position.z);
        }
        else
        {
            bCurrentTile = grid.grid[(currentPos - dir).x, (currentPos - dir).y].GetComponent<TileTest>();
            endTile = grid.grid[(currentPos + dir).x, (currentPos + dir).y].GetComponent<TileTest>();
            pEndPos = new Vector3(endTile.transform.position.x, transform.position.y, endTile.transform.position.z);
            bEndPos = new Vector3(pCurrentTile.transform.position.x, moveblock.transform.position.y, pCurrentTile.transform.position.z);
        }

        while (journey < 1)
        {

            journey += Time.deltaTime * 2f;
            transform.position = Vector3.Lerp(pStartPos, pEndPos, journey);
            moveblock.transform.position = Vector3.Lerp(bStartPos, bEndPos, journey);
            yield return null;
        }

        if (push)
        {

            if (grabbingPlayer)
            {
                pCurrentTile.SetPlayerPresence(false);
                // pCurrentTile.hasPlayer = false;
                bCurrentTile.SetPlayerPresence(true);
                // bCurrentTile.hasPlayer = true;
                // endTile.hasPlayer = true;
                endTile.SetPlayerPresence(true);
                otherPlayer.GetComponent<PlayerControllerTest>().currentPos = currentPos + (dir * 2);
            }
            else
            {
                if (moveblock.GetComponent<Block>().hasPlayer)
                {
                    otherPlayer.GetComponent<PlayerControllerTest>().currentPos = currentPos + (dir * 2);
                    endTile.SetPlayerPresence(true);
                }
                pCurrentTile.SetPlayerPresence(false);
                //pCurrentTile.hasPlayer = false;
                bCurrentTile.SetState(0, moveblock);
                endTile.SetState(moveblock.GetComponent<Block>().blockType, moveblock);
                endTile.SetPlayerPresence(false);
            }


        }
        else
        {

            if (grabbingPlayer)
            {
                //pCurrentTile.hasPlayer = true;
                pCurrentTile.SetPlayerPresence(true);
                endTile.SetPlayerPresence(true);
                //endTile.hasPlayer = true;
                bCurrentTile.SetPlayerPresence(false);
                //bCurrentTile.hasPlayer = false;
                otherPlayer.GetComponent<PlayerControllerTest>().currentPos = currentPos;
            }
            else
            {
                if (moveblock.GetComponent<Block>().hasPlayer)
                {
                    otherPlayer.GetComponent<PlayerControllerTest>().currentPos = currentPos;
                    pCurrentTile.SetPlayerPresence(true);
                }
                pCurrentTile.SetState(moveblock.GetComponent<Block>().blockType, moveblock);
                bCurrentTile.SetState(0, moveblock);
                //bCurrentTile.hasPlayer = false;
                bCurrentTile.SetPlayerPresence(false);
                endTile.SetPlayerPresence(true);
                // endTile.hasPlayer = true;
            }


        }
        currentPos += dir;
        playerAnimationController.pushing = false;
        playerAnimationController.pulling = false;
        manager.inAction = false;
    }

    public void Jump(GameObject targetOB, bool jumpOn, Vector2Int dir, int face, PManagerTest manager)
    {
        playerAnimationController.jumping = true;
        StartCoroutine(JumpCoro(targetOB, jumpOn, dir, face, manager));

    }

    IEnumerator JumpCoro(GameObject targetOB, bool jumpOn, Vector2Int dir, int face, PManagerTest manager)
    {
        ChangeFace(face);
        float journey = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos;
        TileTest currentTile = grid.grid[currentPos.x, currentPos.y].GetComponent<TileTest>();
        TileTest targetTile = targetOB.GetComponent<TileTest>();

        if (jumpOn)
        {
            endPos = new Vector3(targetOB.transform.position.x,
               groundPos + targetTile.obOn.transform.localScale.y,
                targetOB.transform.position.z
                );
        }
        else
        {
            endPos = new Vector3(targetOB.transform.position.x, groundPos, targetOB.transform.position.z);
        }

        Vector3 midPos = (startPos + endPos) / 2;
        midPos.y = transform.position.y + endPos.y;

        while (journey < 1)
        {
            journey += Time.deltaTime * 2;
            Vector3 l1 = Vector3.Lerp(startPos, midPos, journey);
            Vector3 l2 = Vector3.Lerp(midPos, endPos, journey);
            transform.position = Vector3.Lerp(l1, l2, journey);
            yield return null;
        }
        if (jumpOn)
        {
            transform.SetParent(targetTile.obOn.transform);
            targetTile.obOn.GetComponent<Block>().hasPlayer = true;
        }
        else
        {
            transform.SetParent(null);
        }
        if (currentTile.StateIndex() == 1 || currentTile.StateIndex() == 2)
        {
            currentTile.obOn.GetComponent<Block>().hasPlayer = false;
        }
        currentPos += dir;
        currentTile.SetPlayerPresence(false);
        targetTile.SetPlayerPresence(true);
        manager.inAction = false;
        playerAnimationController.jumping = false;

    }
    IEnumerator MovePlayerCoro(Vector2Int dir, int face, PManagerTest manager)
    {
        ChangeFace(face);
        float journey = 0;

        Vector3 startPos = transform.position;
        GameObject targetOB = grid.grid[currentPos.x + dir.x, currentPos.y + dir.y];
        TileTest currentTile = grid.grid[currentPos.x, currentPos.y].GetComponent<TileTest>();
        TileTest targetTile = targetOB.GetComponent<TileTest>();

        Vector3 endPos = new Vector3(targetOB.transform.position.x, transform.position.y, targetOB.transform.position.z);

        while (journey < 1)
        {
            journey += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPos, endPos, journey);
            yield return null;
        }
        if (currentTile.StateIndex() == 1 || currentTile.StateIndex() == 2)
        {
            currentTile.obOn.GetComponent<Block>().hasPlayer = false;
        }
        if (targetTile.StateIndex() == 1 || targetTile.StateIndex() == 2)
        {
            targetTile.obOn.GetComponent<Block>().hasPlayer = true;
            transform.SetParent(targetTile.obOn.transform);
        }
        else
        {
            transform.SetParent(null);
        }
        playerAnimationController.moving = false;
        currentPos += dir;
        currentTile.SetPlayerPresence(false);
        targetTile.SetPlayerPresence(true);
        action = false;
        manager.inAction = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        groundPos = transform.position.y;
        playerAnimationController = GetComponent<PlayerAnimationController>();
        uiHandler = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
