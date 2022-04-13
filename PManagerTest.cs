using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PManagerTest : MonoBehaviour
{
    public PlayerControllerTest pControllerOne;
    public PlayerControllerTest pControllerTwo;
    public bool isPlayerOne = true;

    public InputMaster inputMaster;
    public CameraChangeTest camChange;
    public GridMakerTest grid;
    bool isGrabbing = false;
    public bool inAction = false;
    public ThemeManager themeManager;
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Vertical.performed += ctx => ReadPlayerInput();
        inputMaster.Player.Horizontal.performed += ctx => ReadPlayerInput();
        inputMaster.Player.Switch.performed += ctx => OnChange();
        inputMaster.Player.Grab.performed += ctx => PlayerGrab();
        OnChange();

    }
    private void OnEnable()
    {

        inputMaster.Enable();

    }
    private void OnDisable()
    {

        inputMaster.Disable();

    }
    public void PlayerGrab()
    {
        // Debug.Log("Player grab:" + inAction + isGrabbing);
        if (inAction) return;

        if (isGrabbing)
        {
            isGrabbing = false;
            pControllerOne.uiHandler.ChangeToGrabUI(false, false, true);
            // pControllerTwo.uiHandler.ChangeToGrabUI(false, false, false);
            return;
        }

        if (isPlayerOne)
        {

            if (pControllerOne.GrabObject(true))
            {
                isGrabbing = true;

            }
        }
        else
        {
            if (pControllerTwo.GrabObject(false))
            {
                isGrabbing = true;

            }
            else
            {
                // Debug.Log("Player Two Failed to grab");
            }
        }

    }

    public void OnChange()
    {
        if (inAction || isGrabbing) return;
        inAction = true;
        camChange.ChangePerspective(this);
        isPlayerOne = !isPlayerOne;
        themeManager.SwitchTheme(isPlayerOne);

    }
    public void CalculatePlayerMove(bool xAxis, int num)
    {
        Vector2Int targetPos;

        if (isPlayerOne)
        {
            if (xAxis)
            {
                if (num == -1)
                {
                    targetPos = pControllerOne.currentPos + Vector2Int.left;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            pControllerOne.MovePlayer(Vector2Int.left, 3, this);
                            inAction = true;
                        }
                        else
                        {
                            pControllerOne.ChangeFace(3);
                        }

                    }

                }
                else
                {
                    targetPos = pControllerOne.currentPos + Vector2Int.right;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            inAction = true;
                            pControllerOne.MovePlayer(Vector2Int.right, 1, this);
                        }
                        else
                        {
                            pControllerOne.ChangeFace(1);
                        }

                    }
                }
            }
            else
            {
                if (num == -1)
                {
                    targetPos = pControllerOne.currentPos + Vector2Int.down;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            inAction = true;
                            pControllerOne.MovePlayer(Vector2Int.down, 2, this);
                        }
                        else
                        {
                            pControllerOne.ChangeFace(2);
                        }

                    }

                }
                else
                {
                    targetPos = pControllerOne.currentPos + Vector2Int.up;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            inAction = true;
                            pControllerOne.MovePlayer(Vector2Int.up, 0, this);
                        }
                        else
                        {
                            pControllerOne.ChangeFace(0);
                        }

                    }
                }
            }

        }
        //Player Second Horizontal Movement
        else
        {
            TileTest currentTile = grid.grid[pControllerTwo.currentPos.x, pControllerTwo.currentPos.y].GetComponent<TileTest>();
            if (xAxis)
            {
                inAction = true;
                if (num == -1)
                {

                    targetPos = pControllerTwo.currentPos + Vector2Int.left;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 0)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.left, 3, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, false, Vector2Int.left, 3, this);
                            }

                        }

                        else if (tile.StateIndex() == 1 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.left, 3, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.left, 3, this);
                            }

                        }

                        else if (tile.StateIndex() == 2 && !tile.hasPlayer)
                        {

                            if (currentTile.StateIndex() == 2)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.left, 3, this);
                            }
                            else if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.left, 3, this);

                            }
                            else
                            {
                                inAction = false;
                                pControllerTwo.ChangeFace(3);
                            }
                        }
                        else
                        {
                            pControllerTwo.ChangeFace(3);
                            inAction = false;
                        }

                    }
                    else
                    {
                        inAction = false;
                    }
                }
                else
                {
                    targetPos = pControllerTwo.currentPos + Vector2Int.right;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 0)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.right, 1, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, false, Vector2Int.right, 1, this);
                            }
                        }

                        else if (tile.StateIndex() == 1 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.right, 1, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.right, 1, this);
                            }

                        }

                        else if (tile.StateIndex() == 2 && !tile.hasPlayer)
                        {

                            if (currentTile.StateIndex() == 2)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.right, 1, this);
                            }
                            else if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.right, 1, this);

                            }
                            else
                            {
                                inAction = false;
                                pControllerTwo.ChangeFace(1);
                            }
                        }
                        else
                        {
                            pControllerTwo.ChangeFace(1);
                            inAction = false;
                        }

                    }
                    else
                    {
                        inAction = false;
                    }
                }
            }
            else
            {
                inAction = true;
                if (num == -1)
                {

                    targetPos = pControllerTwo.currentPos + Vector2Int.down;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 0)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.down, 2, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, false, Vector2Int.down, 2, this);
                            }

                        }

                        else if (tile.StateIndex() == 1 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.down, 2, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.down, 2, this);
                            }

                        }

                        else if (tile.StateIndex() == 2 && !tile.hasPlayer)
                        {

                            if (currentTile.StateIndex() == 2)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.down, 2, this);
                            }
                            else if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.down, 2, this);

                            }
                            else
                            {
                                inAction = false;
                                pControllerTwo.ChangeFace(2);
                            }
                        }
                        else
                        {
                            pControllerTwo.ChangeFace(2);
                            inAction = false;
                        }

                    }
                    else
                    {
                        inAction = false;
                    }
                }
                else
                {
                    targetPos = pControllerTwo.currentPos + Vector2Int.up;
                    if (grid.CheckInsideGrid(targetPos))
                    {

                        TileTest tile = grid.grid[targetPos.x, targetPos.y].GetComponent<TileTest>();
                        if (tile.StateIndex() == 0 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 0)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.up, 0, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, false, Vector2Int.right, 1, this);
                            }
                        }

                        else if (tile.StateIndex() == 1 && !tile.hasPlayer)
                        {
                            if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.up, 0, this);
                            }
                            else
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.up, 0, this);
                            }

                        }

                        else if (tile.StateIndex() == 2 && !tile.hasPlayer)
                        {

                            if (currentTile.StateIndex() == 2)
                            {
                                pControllerTwo.MovePlayer(Vector2Int.up, 0, this);
                            }
                            else if (currentTile.StateIndex() == 1)
                            {
                                pControllerTwo.Jump(tile.gameObject, true, Vector2Int.up, 0, this);

                            }
                            else
                            {
                                inAction = false;
                                pControllerTwo.ChangeFace(0);
                            }
                        }
                        else
                        {
                            pControllerTwo.ChangeFace(0);
                            inAction = false;
                        }

                    }
                    else
                    {
                        inAction = false;
                    }
                }
            }
        }

    }
    public void ReadPlayerInput()
    {
        if (inAction) return;

        int inputX = (int)inputMaster.Player.Horizontal.ReadValue<float>();
        int inputY = (int)inputMaster.Player.Vertical.ReadValue<float>();

        if (inputX != 0)
        {
            if (isGrabbing)
            {
                CalculatePushPull(true, inputX);
            }
            else
            {
                CalculatePlayerMove(true, inputX);
            }

        }
        else if (inputY != 0)
        {

            if (isGrabbing)
            {
                CalculatePushPull(false, inputY);
            }
            else
            {
                CalculatePlayerMove(false, inputY);
            }


        }

    }
    public void CalculatePushPull(bool xAxis, int num)
    {
        Vector2Int target;
        if (isPlayerOne)
        {
            if (!xAxis)
            {
                if (pControllerOne.facing == 0)
                {
                    if (num == 1)
                    {
                        target = pControllerOne.currentPos + (Vector2Int.up * 2);
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerOne.MoveWithBlock(Vector2Int.up, true, this);
                            }

                        }
                    }
                    else
                    {
                        target = pControllerOne.currentPos + Vector2Int.down;
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerOne.MoveWithBlock(Vector2Int.down, false, this);
                            }

                        }
                    }
                }
                else if (pControllerOne.facing == 2)
                {
                    if (num == 1)
                    {
                        target = pControllerOne.currentPos + Vector2Int.up;
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerOne.MoveWithBlock(Vector2Int.up, false, this);
                            }

                        }
                    }
                    else
                    {
                        target = pControllerOne.currentPos + (Vector2Int.down * 2);
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerOne.MoveWithBlock(Vector2Int.down, true, this);
                            }

                        }
                    }
                }
            }
        }
        else
        {
            if (xAxis)
            {
                if (pControllerTwo.facing == 1)
                {
                    if (num == 1)
                    {
                        target = pControllerTwo.currentPos + (Vector2Int.right * 2);
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerTwo.MoveWithBlock(Vector2Int.right, true, this);
                            }
                        }
                    }
                    else
                    {
                        target = pControllerTwo.currentPos + Vector2Int.left;
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerTwo.MoveWithBlock(Vector2Int.left, false, this);
                            }
                        }
                    }
                }
                else if (pControllerTwo.facing == 3)
                {
                    if (num == 1)
                    {
                        target = pControllerTwo.currentPos + Vector2Int.right;
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerTwo.MoveWithBlock(Vector2Int.right, false, this);
                            }
                        }
                    }
                    else
                    {
                        target = pControllerTwo.currentPos + (Vector2Int.left * 2);
                        if (grid.CheckInsideGrid(target))
                        {
                            TileTest tile = grid.grid[target.x, target.y].GetComponent<TileTest>();
                            if (tile.StateIndex() == 0 && !tile.hasPlayer)
                            {
                                inAction = true;
                                pControllerTwo.MoveWithBlock(Vector2Int.left, true, this);
                            }
                        }
                    }
                }
            }

        }
    }
}
