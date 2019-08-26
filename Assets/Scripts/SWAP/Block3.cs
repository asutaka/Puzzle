using System;

public class Block3 
{
    #region Swap Block3
    private static void Swap_Block3_R(int j)
    {
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int k = j; k <= j + 2; k++)
                {
                    if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k + 1];
                    }
                    else if (k == j + 2)
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k + 1];
                    }
                }
            }
            else
            {
                for (int k = j + 2; k >= j; k--)
                {
                    if (k == j + 2)
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k - 1];
                    }
                    else if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k - 1];
                    }
                }
            }
        }
    }
    private static void Swap_Block3_L(int j)
    {
        for (int i = 1; i <= Common.ROW - 2; i++)
        {
            var tmp = -1;
            if (i % 2 == 0)
            {
                for (int k = j + 2; k >= j; k--)
                {
                    if (k == j + 2)
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k - 1];
                    }
                    else if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k - 1];
                    }
                }
            }
            else
            {
                for (int k = j; k <= j + 2; k++)
                {
                    if ((i == 1 && k == Math.Max(2, j)) || (i != 1 && k == Math.Max(1, j)))
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k + 1];
                    }
                    else if (k == j + 2)
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i, k + 1];
                    }
                }
            }
        }
    }
    private static void Swap_Block3_U(int j)
    {
        for (int k = j; k <= j + 2; k++)
        {
            var tmp = -1;
            if (k % 2 == 0)
            {
                for (int i = Common.ROW - 2; i >= 1; i--)
                {
                    if (i == Common.ROW - 2)
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i - 1, k];
                    }
                    else if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i - 1, k];
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Common.ROW - 2; i++)
                {
                    if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i + 1, k];
                    }
                    else if (i == Common.ROW - 2)
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i + 1, k];
                    }
                }
            }
        }
    }
    private static void Swap_Block3_D(int j)
    {
        for (int k = j; k <= j + 2; k++)
        {
            var tmp = -1;
            if (k % 2 == 0)
            {
                for (int i = 1; i <= Common.ROW - 2; i++)
                {
                    if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i + 1, k];
                    }
                    else if (i == Common.ROW - 2)
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i + 1, k];
                    }
                }
            }
            else
            {
                for (int i = Common.ROW - 2; i >= 1; i--)
                {
                    if (i == Common.ROW - 2)
                    {
                        tmp = Common.arrMatrix[i, k];
                        Common.arrMatrix[i, k] = Common.arrMatrix[i - 1, k];
                    }
                    else if ((i == 2 && k == 1) || (i == 1 && k != 1))
                    {
                        Common.arrMatrix[i, k] = tmp;
                    }
                    else if (Common.arrMatrix[i, k] > -1)
                    {
                        Common.arrMatrix[i, k] = Common.arrMatrix[i - 1, k];
                    }
                }
            }
        }
    }
    #region Block3 A - RDLU
    public static void Swap_Block3A()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_R(j);
                    break;
                case 2:
                    Swap_Block3_U(j);
                    break;
                case 3:
                    Swap_Block3_L(j);
                    break;
                case 4:
                    Swap_Block3_D(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[1, 2];
        Common.arrMatrix[1, 2] = Common.arrMatrix[1, 5];
        Common.arrMatrix[1, 5] = tmp;
    }
    #endregion
    #region Block3 B - RULD
    public static void Swap_Block3B()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_R(j);
                    break;
                case 2:
                    Swap_Block3_D(j);
                    break;
                case 3:
                    Swap_Block3_L(j);
                    break;
                case 4:
                    Swap_Block3_U(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 2];
        Common.arrMatrix[2, 2] = Common.arrMatrix[3, 5];
        Common.arrMatrix[3, 5] = tmp;
    }
    #endregion
    #region Block3 C - LDRU
    public static void Swap_Block3C()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_L(j);
                    break;
                case 2:
                    Swap_Block3_U(j);
                    break;
                case 3:
                    Swap_Block3_R(j);
                    break;
                case 4:
                    Swap_Block3_D(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 5];
        Common.arrMatrix[2, 5] = Common.arrMatrix[3, 2];
        Common.arrMatrix[3, 2] = tmp;
    }
    #endregion
    #region Block3 D - LURD
    public static void Swap_Block3D()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_L(j);
                    break;
                case 2:
                    Swap_Block3_D(j);
                    break;
                case 3:
                    Swap_Block3_R(j);
                    break;
                case 4:
                    Swap_Block3_U(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[1, 2];
        Common.arrMatrix[1, 2] = Common.arrMatrix[1, 4];
        Common.arrMatrix[1, 4] = tmp;
    }
    #endregion
    #region Block3 E - URDL
    public static void Swap_Block3E()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_U(j);
                    break;
                case 2:
                    Swap_Block3_R(j);
                    break;
                case 3:
                    Swap_Block3_D(j);
                    break;
                case 4:
                    Swap_Block3_L(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    #endregion
    #region Block3 F - ULDR
    public static void Swap_Block3F()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_U(j);
                    break;
                case 2:
                    Swap_Block3_L(j);
                    break;
                case 3:
                    Swap_Block3_D(j);
                    break;
                case 4:
                    Swap_Block3_R(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    #endregion
    #region Block3 G - DRUL
    public static void Swap_Block3G()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_D(j);
                    break;
                case 2:
                    Swap_Block3_R(j);
                    break;
                case 3:
                    Swap_Block3_U(j);
                    break;
                case 4:
                    Swap_Block3_L(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    #endregion
    #region Block3 H - DLUR
    public static void Swap_Block3H()
    {
        Common.generateArray();
        int _type = 1;
        for (int j = 1; j <= Common.COL - 2; j++)
        {
            if (j + 2 > Common.COL - 2)
            {
                break;
            }
            switch (_type)
            {
                case 1:
                    Swap_Block3_D(j);
                    break;
                case 2:
                    Swap_Block3_L(j);
                    break;
                case 3:
                    Swap_Block3_U(j);
                    break;
                case 4:
                    Swap_Block3_R(j);
                    break;
            }
            _type++;
            if (_type > 4)
            {
                _type = 1;
            }
        }
        //Custom
        var tmp = Common.arrMatrix[2, 1];
        Common.arrMatrix[2, 1] = Common.arrMatrix[3, 1];
        Common.arrMatrix[3, 1] = tmp;
    }
    #endregion
    #endregion
}
