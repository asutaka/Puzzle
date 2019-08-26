public static class UpDown 
{
    #region Swap UpDown
    private static void UpDownBase(bool _isTypeA)
    {
        for (int j = 1; j <= Common.COL - 2; j++)
        {

            var isChan = (j % 2 == 0);
            if (!_isTypeA)
            {
                isChan = !isChan;
            }
            var tmp = -1;
            if (isChan)
            {
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
            else
            {
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
    }
    private static void Swap_UpDown(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
        //Custom
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    private static void Swap_UpDown_R(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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
    private static void Swap_UpDown_L(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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
    private static void Swap_UpDown_U(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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
    private static void Swap_UpDown_D(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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
    private static void Swap_UpDown_LR(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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
    private static void Swap_UpDown_RL(bool _isTypeA)
    {
        Common.generateArray();
        UpDownBase(_isTypeA);
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

    public static void Swap_UpDownA()
    {
        Swap_UpDown(true);
    }
    public static void Swap_UpDownA_R()
    {
        Swap_UpDown_R(true);
    }
    public static void Swap_UpDownA_L()
    {
        Swap_UpDown_L(true);
    }
    public static void Swap_UpDownA_U()
    {
        Swap_UpDown_U(true);
    }
    public static void Swap_UpDownA_D()
    {
        Swap_UpDown_D(true);
    }
    public static void Swap_UpDownA_LR()
    {
        Swap_UpDown_LR(true);
    }
    public static void Swap_UpDownA_RL()
    {
        Swap_UpDown_RL(true);
    }
    public static void Swap_UpDownB()
    {
        Swap_UpDown(false);
    }
    public static void Swap_UpDownB_R()
    {
        Swap_UpDown_R(false);
    }
    public static void Swap_UpDownB_L()
    {
        Swap_UpDown_L(false);
    }
    public static void Swap_UpDownB_U()
    {
        Swap_UpDown_U(false);
    }
    public static void Swap_UpDownB_D()
    {
        Swap_UpDown_D(false);
    }
    public static void Swap_UpDownB_LR()
    {
        Swap_UpDown_LR(false);
    }
    public static void Swap_UpDownB_RL()
    {
        Swap_UpDown_RL(false);
    }
    #endregion
}
