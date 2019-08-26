public class HoanViCheo 
{
    #region Swap Hoán vị chéo
    private static void SwapHoanViBase()
    {
        Common.generateArray();
        int div = 1;
        var count = 0;
        var countTemp = 0;
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            countTemp = 0;
            for (int j = div; j <= Common.COL - 2; j++)
            {
                if (Common.arrMatrix[i, j] <= -1)
                {
                    j++;
                    if (i == 1)
                    {
                        count++;
                    }
                    continue;
                }
                if (j + 1 <= Common.COL - 2)
                {
                    var tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    Common.arrMatrix[i, j + 1] = tmp;
                    j++;
                    if (i == 1)
                    {
                        count++;
                    }
                    else
                    {
                        countTemp++;
                    }
                    if (countTemp == count)
                    {
                        break;
                    }
                }
                if (j == Common.COL - 2)
                {
                    if (div <= 2)
                        continue;
                    for (int k = 1; k < div; k++)
                    {
                        var tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k + 1];
                        Common.arrMatrix[i, k + 1] = tmp;
                        k++;
                        if (i == 1)
                        {
                            count++;
                        }
                        else
                        {
                            countTemp++;
                        }
                        if (countTemp == count)
                        {
                            j = Common.COL - 2;
                            break;
                        }
                        if (k + 1 >= div)
                        {
                            j = Common.COL - 2;
                            break;
                        }
                    }
                }
                if ((j + 1) == Common.COL - 2)
                {
                    for (int k = 1; k < div; k++)
                    {
                        var tmp = Common.arrMatrix[i, j + 1];
                        Common.arrMatrix[i, j + 1] = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = tmp;
                        k++;
                        if (i == 1)
                        {
                            count++;
                        }
                        else
                        {
                            countTemp++;
                        }
                        if (countTemp == count)
                        {
                            j = Common.COL - 2;
                            break;
                        }
                        if (k + 1 >= div)
                        {
                            j = Common.COL - 2;
                            break;
                        }
                    }
                }
            }
            div++;
        }
    }
    public static void SwapHoanVi()
    {
        SwapHoanViBase();
        //Chuẩn hóa
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 5];
        Common.arrMatrix[3, 5] = tmp;
    }
    public static void SwapHoanVi_R()
    {
        SwapHoanViBase();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = Common.COL - 2; j >= 1; j--)
            {
                if (Common.arrMatrix[i, j] <= -1)
                    continue;
                if (j == Common.COL - 2)
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                }
            }
        }
    }
    public static void SwapHoanVi_L()
    {
        SwapHoanViBase();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= Common.COL - 2; j++)
            {
                if (Common.arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                }
                else if (j == Common.COL - 2)
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                }
            }
        }
    }
    public static void SwapHoanVi_U()
    {
        SwapHoanViBase();
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= Common.ROW - 2; i++)
            {
                if (Common.arrMatrix[i, j] <= -1)
                    continue;
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i + 1, j];
                }
                else if (i == Common.ROW - 2)
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i + 1, j];
                }
            }
        }
    }
    public static void SwapHoanVi_D()
    {
        SwapHoanViBase();
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            var tmp = -1;
            for (int i = Common.ROW - 2; i >= 1; i--)
            {
                if (Common.arrMatrix[i, j] <= -1)
                    continue;
                if (i == Common.ROW - 2)
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i - 1, j];
                }
            }
        }
    }
    public static void SwapHoanVi_RL()
    {
        SwapHoanViBase();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = 1; j <= Common.COL - 2; j++)
                {
                    if (Common.arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                    else if (j == Common.COL - 2)
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = Common.COL - 2; j >= 1; j--)
                {
                    if (Common.arrMatrix[i, j] <= -1)
                        continue;
                    if (j == Common.COL - 2)
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                }
            }
        }
    }
    public static void SwapHoanVi_LR()
    {
        SwapHoanViBase();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = Common.COL - 2; j >= 1; j--)
                {
                    if (Common.arrMatrix[i, j] <= -1)
                        continue;
                    if (j == Common.COL - 2)
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                }
            }
            else
            {
                for (int j = 1; j <= Common.COL - 2; j++)
                {
                    if (Common.arrMatrix[i, j] <= -1)
                        continue;
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                    else if (j == Common.COL - 2)
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    #endregion
}
