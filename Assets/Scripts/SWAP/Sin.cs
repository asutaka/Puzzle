public class Sin 
{
    #region Thuật toán hình Sin
    private static void Swap_Sin_Base()
    {
        Common.generateArray();
        var arrTmp = new int[Common.ROW, Common.COL];
        for (int i = 0; i < Common.ROW; i++)
        {
            for (int j = 0; j < Common.COL; j++)
            {
                arrTmp[i, j] = Common.arrMatrix[i, j];
            }
        }
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            for (int j = 1; j <= Common.COL - 2; j++)
            {
                if ((i + j) >= Common.ROW)
                {
                    if (i == (Common.ROW - 2) && j == (Common.COL - 2))
                    {
                        arrTmp[i, j] = Common.arrMatrix[1, j];
                    }
                    else
                    {
                        arrTmp[i, j] = Common.arrMatrix[1 + (i + j - 1) % (Common.ROW - 1), j];
                    }
                }
                else
                {
                    arrTmp[i, j] = Common.arrMatrix[i + j - 1, j];
                }
            }
        }
        //Chuẩn hóa
        var tmp = arrTmp[2, 1];
        arrTmp[2, 1] = arrTmp[3, 1];
        arrTmp[3, 1] = tmp;
        Common.arrMatrix = arrTmp;
    }
    public static void Swap_Sin()
    {
        Swap_Sin_Base();
        //Chuẩn hóa
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    public static void Swap_Sin_R()
    {
        Swap_Sin_Base();
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
    public static void Swap_Sin_L()
    {
        Swap_Sin_Base();
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
    public static void Swap_Sin_U()
    {
        Swap_Sin_Base();
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
    public static void Swap_Sin_D()
    {
        Swap_Sin_Base();
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
    public static void Swap_Sin_RL()
    {
        Swap_Sin_Base();
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
    public static void Swap_Sin_LR()
    {
        Swap_Sin_Base();
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
