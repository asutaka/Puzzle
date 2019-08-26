public static class Sole
{
    #region Swap Sole
    private static void Swap_Sole_Base()
    {
        Common.generateArray();
        for (int i = 0; i < Common.ROW; i++)
        {
            for (int j = 0; j < Common.COL; j++)
            {
                if (Common.arrMatrix[i, j] > -1 && (j + 2) < Common.COL)
                {
                    if (i % 2 != 0 && j == 1)
                        continue;
                    var tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    Common.arrMatrix[i, j + 1] = tmp;
                    j++;
                }
            }
        }
        //Custom
        var tmpCustom = Common.arrMatrix[2, 2];
        Common.arrMatrix[2, 2] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmpCustom;
    }
    public static void Swap_Sole()
    {
        Common.generateArray();
        for (int i = 0; i < Common.ROW; i++)
        {
            for (int j = 0; j < Common.COL; j++)
            {
                if (Common.arrMatrix[i, j] > -1 && (j + 2) < Common.COL)
                {
                    if (i % 2 != 0 && j == 1)
                        continue;
                    var tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    Common.arrMatrix[i, j + 1] = tmp;
                    j++;
                }
            }
        }
    }
    public static void Swap_Sole_Right()
    {
        Swap_Sole_Base();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = Common.COL - 2; j >= 1; j--)
            {
                if (j == Common.COL - 2)
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                }
                else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else if (Common.arrMatrix[i, j] > -1)
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                }
            }
        }
    }
    public static void Swap_Sole_Left()
    {
        Swap_Sole_Base();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            for (int j = 1; j <= Common.COL - 2; j++)
            {
                if ((i == 1 && j == 2) || (i != 1 && j == 1))
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                }
                else if (j == Common.COL - 2)
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else if (Common.arrMatrix[i, j] > -1)
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                }
            }
        }
    }
    public static void Swap_Sole_Up()
    {
        Swap_Sole_Base();

        for (int j = 1; j <= Common.COL - 2; j++)
        {
            var tmp = -1;
            for (int i = 1; i <= Common.ROW - 2; i++)
            {
                if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i + 1, j];
                }
                else if (i == Common.ROW - 2)
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else if (Common.arrMatrix[i, j] > -1)
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i + 1, j];
                }
            }
        }
    }
    public static void Swap_Sole_Down()
    {
        Swap_Sole_Base();

        for (int j = 1; j <= Common.COL - 2; j++)
        {
            var tmp = -1;
            for (int i = Common.ROW - 2; i >= 1; i--)
            {
                if (i == Common.ROW - 2)
                {
                    tmp = Common.arrMatrix[i, j];
                    Common.arrMatrix[i, j] = Common.arrMatrix[i - 1, j];
                }
                else if ((i == 2 && j == 1) || (i == 1 && j != 1))
                {
                    Common.arrMatrix[i, j] = tmp;
                }
                else if (Common.arrMatrix[i, j] > -1)
                {
                    Common.arrMatrix[i, j] = Common.arrMatrix[i - 1, j];
                }
            }
        }
    }
    public static void Swap_Sole_RL()
    {
        Swap_Sole_Base();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = 1; j <= Common.COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                    else if (j == Common.COL - 2)
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else if (Common.arrMatrix[i, j] > -1)
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                }
            }
            else
            {
                for (int j = Common.COL - 2; j >= 1; j--)
                {
                    if (j == Common.COL - 2)
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else if (Common.arrMatrix[i, j] > -1)
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                }
            }
        }
    }
    public static void Swap_Sole_LR()
    {
        Swap_Sole_Base();
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int j = Common.COL - 2; j >= 1; j--)
                {
                    if (j == Common.COL - 2)
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                    else if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else if (Common.arrMatrix[i, j] > -1)
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j - 1];
                    }
                }
            }
            else
            {
                for (int j = 1; j <= Common.COL - 2; j++)
                {
                    if ((i == 1 && j == 2) || (i != 1 && j == 1))
                    {
                        tmp = Common.arrMatrix[i, j];
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                    else if (j == Common.COL - 2)
                    {
                        Common.arrMatrix[i, j] = tmp;
                    }
                    else if (Common.arrMatrix[i, j] > -1)
                    {
                        Common.arrMatrix[i, j] = Common.arrMatrix[i, j + 1];
                    }
                }
            }
        }
    }
    #endregion
}
