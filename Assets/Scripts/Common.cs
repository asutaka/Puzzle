using System;
using UnityEngine;

public static class Common
{
    //Số dòng
    public static int ROW = 0;
    //Số cột
    public static int COL = 0;
    //Độ dài, độ rộng miếng ghép
    public static float stepW = 0, stepH = 0;
    //Tọa độ ban đầu của X,Y
    public static float startPosX = 0, startPosY = 0;
    //Mode game đang được thiết lập
    public static Mode ModeGame;
    //Hằng số giá trị số lần đảo ảnh
    public const int MAX_SWAP = 4;
    //Biến lưu index của lần swap 
    public static int curSwap = MAX_SWAP;
    //Biến lưu index của ảnh hiện tại(dùng lưu thông tin index của ảnh trong Asset)
    public static int curImage = 1;
    //Mảng nguyên gốc
    private static int[,] arrMatrix_Origin;
    //Mảng hiển thị
    public static int[,] arrMatrix;

    //các mode game
    public enum Mode : int
    {
        EASY = 0,
        NORMAL = 1,
    }
    public static void setMode(bool _mode)
    {
        if (_mode)
        {
            ModeGame = Mode.EASY;
            ROW = 5;
            COL = 7;
        }
        else
        {
            ModeGame = Mode.NORMAL;
            ROW = 7;
            COL = 10;
        }
        arrMatrix = new int[ROW, COL];
        arrMatrix_Origin = new int[ROW, COL];
        generateArrayOrigin();
        generateArray();
        ThietLapThongSo();
    }
    /// <summary>
    /// Thiết lập thông số: độ rộng miếng ghép, độ dài miếng ghép, tọa độ ban đầu của X, tọa độ ban đầu của Y
    /// </summary>
    /// 
    private static void ThietLapThongSo()
    {
        stepW = (float)Math.Round((float)Screen.width / (COL - 2), 2);
        stepH = (float)Math.Round((float)Screen.height / (ROW - 2), 2);
        startPosX = -(Screen.width - stepW) / 2;
        startPosY = (Screen.height - stepH) / 2;
    }
    //fake
    private static void showArray()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                Debug.Log(string.Format("index[{0},{1}]: {2} ", i, j, arrMatrix[i, j]));
            }
        }
    }
    /// <summary>
    /// Khởi tạo giá trị cho mảng nguyên gốc
    /// </summary>
    /// 
    private static void generateArrayOrigin()
    {
        int count = 1;
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if ((i < 1) || (j < 1) || (i == 1 && j == 1) || (i == ROW - 1) || (j == COL - 1))
                {
                    arrMatrix_Origin[i, j] = -1;
                    continue;
                }
                arrMatrix_Origin[i, j] = count++;
            }
        }
    }
    /// <summary>
    /// Khởi tạo giá trị cho mảng hiển thị
    /// </summary>
    /// 
    public static void generateArray()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                arrMatrix[i, j] = arrMatrix_Origin[i, j];
            }
        }
    }
    /// <summary>
    /// Kiểm tra xem người chơi đã hoàn thành game hay chưa
    /// </summary>
    /// <returns></returns>
    public static bool checkComplete()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if (arrMatrix[i, j] != arrMatrix_Origin[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// Phương thức chi tiết: Thiết lập giá trị cho đối tượng Info
    /// </summary>
    /// <param name="info"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="isOrigin"></param>
    public static void initInfo(ref Info info, int row, int col, bool isOrigin = false)
    {
        info.Current = arrMatrix[row, col];
        if (isOrigin)
        {
            info.Origin = arrMatrix[row, col];
        }
    }
    /// <summary>
    /// Phương thức Swap
    /// </summary>
    public static void swap()
    {
        //Tên Packet : 1_Disney ==> 1
        var x = 1;
        var index = 0;

        curSwap++;
        if (curSwap > MAX_SWAP)
        {
            curSwap = 1;
        }
        switch (curSwap)
        {
            case 1:
                index = 0 + curImage;
                break;
            case 2:
                index = 13 + (((x * x) + curImage) % 15);
                break;
            case 3:
                index = 28 + (((x * x) + curImage) % 16);
                break;
            case 4:
                index = 0;
                break;
        }
        int indexSwap = 0 + index;
        switch (indexSwap)
        {
            case 0: Swap_Normal(); break;
            case 1: UpDown.Swap_UpDownA(); break;
            case 2: UpDown.Swap_UpDownA_L(); break;
            case 3: UpDown.Swap_UpDownA_R(); break;
            case 4: UpDown.Swap_UpDownA_U(); break;
            case 5: UpDown.Swap_UpDownA_D(); break;
            case 6: UpDown.Swap_UpDownA_LR(); break;
            case 7: UpDown.Swap_UpDownA_RL(); break;
            case 8: UpDown.Swap_UpDownB(); break;
            case 9: UpDown.Swap_UpDownB_L(); break;
            case 10: UpDown.Swap_UpDownB_R(); break;
            case 11: UpDown.Swap_UpDownB_U(); break;
            case 12: UpDown.Swap_UpDownB_D(); break;
            case 13: Sole.Swap_Sole(); break;
            case 14: Block3.Swap_Block3A(); break;
            case 15: Sole.Swap_Sole_Left(); break;
            case 16: Block3.Swap_Block3B(); break;
            case 17: Sole.Swap_Sole_Right(); break;
            case 18: Block3.Swap_Block3C(); break;
            case 19: Sole.Swap_Sole_Up(); break;
            case 20: Block3.Swap_Block3D(); break;
            case 21: Sole.Swap_Sole_Down(); break;
            case 22: Block3.Swap_Block3E(); break;
            case 23: Sole.Swap_Sole_LR(); break;
            case 24: Block3.Swap_Block3F(); break;
            case 25: Sole.Swap_Sole_RL(); break;
            case 26: Block3.Swap_Block3G(); break;
            case 27: Block3.Swap_Block3H(); break;
            case 28: UpDown.Swap_UpDownB_LR(); break;
            case 29: UpDown.Swap_UpDownB_RL(); break;
            case 30: Sin.Swap_Sin(); break;
            case 31: HoanViCheo.SwapHoanVi(); break;
            case 32: Sin.Swap_Sin_L(); break;
            case 33: HoanViCheo.SwapHoanVi_L(); break;
            case 34: Sin.Swap_Sin_R(); break;
            case 35: HoanViCheo.SwapHoanVi_R(); break;
            case 36: Sin.Swap_Sin_U(); break;
            case 37: HoanViCheo.SwapHoanVi_U(); break;
            case 38: Sin.Swap_Sin_D(); break;
            case 39: HoanViCheo.SwapHoanVi_D(); break;
            case 40: Sin.Swap_Sin_LR(); break;
            case 41: HoanViCheo.SwapHoanVi_LR(); break;
            case 42: Sin.Swap_Sin_RL(); break;
            case 43: HoanViCheo.SwapHoanVi_RL(); break;
            default: Swap_Normal(); break;
        }
    }
    /// <summary>
    /// Trở về trạng thái ban đầu
    /// </summary>
    public static void Swap_Normal()
    {
        generateArray();
    }
    public static void showLog()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                Debug.Log(string.Format("arr[{0},{1}] = {2}",i,j,arrMatrix[i,j]));
            }
        }
    }
}
