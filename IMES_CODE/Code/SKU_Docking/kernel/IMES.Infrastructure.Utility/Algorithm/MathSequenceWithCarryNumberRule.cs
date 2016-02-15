///<summary>
/// INVENTEC corporation (c)2009 all rights reserved. 
/// Description: 有進位數學Bits數規則类
///             
/// Update: 
/// Date       Name                  Reason 
///========== ===================== =====================================
/// 2009-10-15  Liu Dong(eB1-4)         Create 
///</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility
{
    /// <summary>
    /// 有進位數學Bits數規則
    /// </summary>
    public class MathSequenceWithCarryNumberRule
    {
        private int _iBits  = 1;
        private string _charCollection = string.Empty;
        private string _maxNumber = string.Empty;
        private string _minNumber = string.Empty;

        /// <summary>
        /// 最大位数
        /// </summary>
        public int iBits
        {
            get { return _iBits; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public string MaxNumber
        {
            get{return _maxNumber;}
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public string MinNumber
        {
            get{return _minNumber;}
        }

        private string _generalMaxNumber = string.Empty;
        private string _generalMinNumber = string.Empty;

        private int _iStep = 1;

        /// <summary>
        /// 步进值
        /// </summary>
        public int iStep
        {
            get{return this._iStep;}
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iBits"></param>
        /// <param name="charCollection"></param>
        public MathSequenceWithCarryNumberRule(int iBits, string charCollection)
        {
            //輸入參數檢查：長度、字符集等等

            if (iBits < 1) 
            {
                throw new FisException("GEN001", new string[] { });
            }

            if (charCollection == null) 
            {
                throw new FisException("GEN002", new string[] { });
            }
            charCollection = charCollection.Trim();
            if (charCollection == string.Empty)
            {
                throw new FisException("GEN003", new string[] { });
            }

            if (charCollection.Length < 2)
            {
                throw new FisException("GEN004", new string[] { });
            }

            this._iBits = iBits;
            this._charCollection = charCollection;

            _maxNumber = this.GetGeneralMaxNumber();
            _minNumber = this.GetGeneralMinNumber();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iBits"></param>
        /// <param name="charCollection"></param>
        /// <param name="maxNumber"></param>
        /// <param name="minNumber"></param>
        public MathSequenceWithCarryNumberRule(int iBits, string charCollection, string maxNumber, string minNumber) : this(iBits, charCollection)
        {
            //輸入參數檢查：長度、字符集等等

            if (maxNumber == null)
            {
                throw new FisException("GEN005", new string[] { });
            }

            maxNumber = maxNumber.Trim();
            if (maxNumber == string.Empty)
            {
                throw new FisException("GEN006", new string[] { });
            }

            if (maxNumber.Length != this._iBits)
            {
                throw new FisException("GEN007", new string[] { this._iBits.ToString() });
            }

            if (minNumber == null)
            {
                throw new FisException("GEN008", new string[] { });
            }
            minNumber = minNumber.Trim();
            if (minNumber == string.Empty)
            {
                throw new FisException("GEN009", new string[] { });
            }

            if (minNumber.Length != this._iBits)
            {
                throw new FisException("GEN010", new string[] { this._iBits.ToString() });
            }

            if (! InCharCollection(maxNumber))
            {
                throw new FisException("GEN011", new string[] { this._charCollection });
            }

            if (! InCharCollection(minNumber))
            {
                throw new FisException("GEN012", new string[] { this._charCollection });
            }

            if (this.Cmp(maxNumber, minNumber) == -1)
            {
                throw new FisException("GEN013", new string[] { });
            }

            //不會發生以下2個問題, 故注掉
            //If Me.Cmp(maxNumber, Me.GetGeneralMaxNumber()) = 1 Then
            //    Throw New FisException(String.Format("The customized max number exceeds the Max limit! [{0}] ", Me.GetGeneralMaxNumber()))
            //End If

            //If Me.Cmp(minNumber, Me.GetGeneralMinNumber()) = -1 Then
            //    Throw New FisException(String.Format("The customized min number exceeds the Min limit! [{0}] ", Me.GetGeneralMinNumber()))
            //End If

            this._maxNumber = maxNumber;
            this._minNumber = minNumber;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iBits"></param>
        /// <param name="charCollection"></param>
        /// <param name="maxNumber"></param>
        /// <param name="minNumber"></param>
        /// <param name="iStep"></param>
        public MathSequenceWithCarryNumberRule(int iBits, string charCollection, string maxNumber, string minNumber, int iStep) : this(iBits, charCollection, maxNumber, minNumber)
        {
            this._iStep = iStep;
        }

        /// <summary>
        /// 得到一個數步進幾次的值
        /// </summary>
        /// <param name="numberStart">起始數</param>
        /// <param name="count">步進次數</param>
        /// <returns>所得值</returns>
        /// <remarks></remarks>
        public string IncreaseToNumber(string numberStart, int count)
        {
            //輸入參數檢查：長度、字符集等等

            #region Check

            if (numberStart == null)
            {
                throw new FisException("GEN014", new string[] { });
            }
            numberStart = numberStart.Trim();
            if (numberStart == String.Empty) 
            {
                throw new FisException("GEN015", new string[] { });
            }

            if (numberStart.Length != this._iBits)
            {
                throw new FisException("GEN016", new string[] { this._iBits.ToString() });
            }

            if (! InCharCollection(numberStart))
            {
                throw new FisException("GEN017", new string[] { this._charCollection });
            }

            if (this.Cmp(numberStart, this.GetGeneralMaxNumber()) == 1)
            {
                throw new FisException("GEN018", new string[] { this.GetGeneralMaxNumber() });
            }

            if (this.Cmp(numberStart, this.GetGeneralMinNumber()) == -1)
            {
                throw new FisException("GEN019", new string[] { this.GetGeneralMinNumber() });
            }

            #endregion

            if (count < 0)
            {
                throw new FisException("GEN020", new string[] { });
            }

            if (count == 0)
                return numberStart;

            char[] chsNumberStart = numberStart.ToCharArray();

            Array.Reverse(chsNumberStart);

            char[] result = (char[])(chsNumberStart.Clone());

            //每位上需要加的位數
            int[] addNumberBits = new int[chsNumberStart.Length];
            for (int t = 0; t <= addNumberBits.Length - 1; t++)
            {
                addNumberBits[t] = 0;
            }

            int div = (this._iStep * count) / this._charCollection.Length;
            int remain = (this._iStep * count) % this._charCollection.Length;

            int j = 0; //記錄需要加數的最高位數
            int i;
            for (i = 0 ; i <= chsNumberStart.Length - 1; i++) //Step 1
            {
                addNumberBits[i] = remain;
                j = j + 1;

                if (div == 0)
                {
                    break;
                }
                else if( i == chsNumberStart.Length - 1 )
                {
                    this.ThrowExceedGeneralMaxException();
                }

                int divNew;
                divNew = div / this._charCollection.Length;
                remain = div % this._charCollection.Length;
                div = divNew;
            }

            bool isCarry = false;
            for (i = 0; i <= chsNumberStart.Length - 1; i++) //Step 1
            {
                if (i == j)
                {
                    break;
                }

                char newChar = Increase(chsNumberStart[i], addNumberBits[i]);

                result[i] = newChar;
                // 2009-06-22  Liu Dong(eB1-4)         Modify BUG: 進位使高一位的加數9變成0,沒有再進位
                isCarry = (Cmp(newChar, chsNumberStart[i]) == -1) || addNumberBits[i].Equals(this._charCollection.Length); //是否進位

                if (isCarry)
                {
                    if (i < chsNumberStart.Length - 1)
                    {
                        addNumberBits[i + 1] = addNumberBits[i + 1] + 1;

                        if (j == i + 1)
                        {
                            j = j + 1;   //需要加數的最高位數因爲進位而增一
                        }
                    }
                    else
                    {
                        this.ThrowExceedGeneralMaxException();
                    }
                }
            }

            string sResult = Convert(result);

            CheckAndThrowExceedGeneralMaxException(sResult);

            if (this._maxNumber != string.Empty && this.Cmp(sResult, this._maxNumber) == 1)
            {
                throw new FisException("GEN021", new string[] { this._maxNumber.ToString() });
            }
            return sResult;
        }

        /// <summary>
        /// 计算2个数之间的差
        /// </summary>
        /// <param name="numberStart"></param>
        /// <param name="numberEnd"></param>
        /// <returns></returns>
        public long CalculateDifference(string numberStart, string numberEnd)
        {
            long ret = 0;

            #region Check

            if (numberStart == null)
            {
                throw new FisException("GEN032", new string[] { });
            }
            numberStart = numberStart.Trim();
            if (numberStart == String.Empty)
            {
                throw new FisException("GEN033", new string[] { });
            }

            if (numberStart.Length != this._iBits)
            {
                throw new FisException("GEN034", new string[] { this._iBits.ToString() });
            }

            if (!InCharCollection(numberStart))
            {
                throw new FisException("GEN035", new string[] { this._charCollection });
            }

            if (this.Cmp(numberStart, this.GetGeneralMaxNumber()) == 1)
            {
                throw new FisException("GEN036", new string[] { this.GetGeneralMaxNumber() });
            }

            if (this.Cmp(numberStart, this.GetGeneralMinNumber()) == -1)
            {
                throw new FisException("GEN037", new string[] { this.GetGeneralMinNumber() });
            }

            //--------------------------

            if (numberEnd == null)
            {
                throw new FisException("GEN038", new string[] { });
            }
            numberEnd = numberEnd.Trim();
            if (numberEnd == String.Empty)
            {
                throw new FisException("GEN039", new string[] { });
            }

            if (numberEnd.Length != this._iBits)
            {
                throw new FisException("GEN040", new string[] { this._iBits.ToString() });
            }

            if (!InCharCollection(numberEnd))
            {
                throw new FisException("GEN041", new string[] { this._charCollection });
            }

            if (this.Cmp(numberEnd, this.GetGeneralMaxNumber()) == 1)
            {
                throw new FisException("GEN042", new string[] { this.GetGeneralMaxNumber() });
            }

            if (this.Cmp(numberEnd, this.GetGeneralMinNumber()) == -1)
            {
                throw new FisException("GEN043", new string[] { this.GetGeneralMinNumber() });
            }

            #endregion

            if (numberEnd.Equals(numberStart))
                return 0;

            char[] chsNumberStart = numberStart.ToCharArray();
            char[] chsNumberEnd = numberEnd.ToCharArray();

            //Array.Reverse(chsNumberStart);
            //Array.Reverse(chsNumberEnd);

            //每位上的差
            int[] diffNumbersByBit = new int[chsNumberStart.Length];
            bool isMinus = false;
            bool isSetMinus = false;//是否找到第一个不为0的位差
            for (int t = 0; t <= diffNumbersByBit.Length - 1; t++)
            {
                diffNumbersByBit[t] = Substract(chsNumberEnd[t], chsNumberStart[t]);
                if (isSetMinus == false && diffNumbersByBit[t] != 0)
                {
                    isMinus = diffNumbersByBit[t] < 0;
                    isSetMinus = true;
                }
                if (isSetMinus && isMinus)
                {
                    diffNumbersByBit[t] = 0 - diffNumbersByBit[t];
                }
                if (isSetMinus && diffNumbersByBit[t] < 0)
                {
                    int i = t;
                    do
                    {
                        diffNumbersByBit[i] = this._charCollection.Length + diffNumbersByBit[i];
                        diffNumbersByBit[i - 1] = diffNumbersByBit[i - 1] - 1;
                        i = i - 1;
                    }
                    while (i > 0 && diffNumbersByBit[i] < 0);//一直借位
                }
            }

            Array.Reverse(diffNumbersByBit);

            for (int p = 0; p <= diffNumbersByBit.Length - 1; p++)
            {
                if (diffNumbersByBit[p] != 0)
                {
                    ret = ret + System.Convert.ToInt64(diffNumbersByBit[p] * Math.Pow(this._charCollection.Length, p));
                }
            }
            return isMinus ? 0 - ret : ret;
        }

        #region Inner

        /// <summary>
        /// 一位字符增一個Step
        /// </summary>
        /// <param name="bit"></param>
        /// <returns>得到的步進一次結果</returns>
        /// <remarks></remarks>
        private char Increase(char bit)
        {
            int now = this._charCollection.IndexOf(bit);
            if (now + this._iStep > this._charCollection.Length - 1)
            {
                return char.Parse(this._charCollection.Substring(now + this._iStep - this._charCollection.Length, 1));
            }
            else
            {
                return char.Parse(this._charCollection.Substring(now + this._iStep, 1));
            }
        }

        /// <summary>
        /// 一位字符增指定次數
        /// </summary>
        /// <param name="bit"></param>
        /// <returns>得到的步進一次結果</returns>
        /// <remarks></remarks>
        private char Increase(char bit, int count)
        {
            int now = this._charCollection.IndexOf(bit);
            if (now + count > this._charCollection.Length - 1)
            {
                return char.Parse(this._charCollection.Substring(now + count - this._charCollection.Length, 1));
            }
            else
            {
                return char.Parse(this._charCollection.Substring(now + count, 1));
            }
        }

        ///// <summary>
        ///// 一位字符减指定次數
        ///// </summary>
        ///// <param name="bit"></param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //private char Decrease(char bit, int count)
        //{
        //    int now = this._charCollection.IndexOf(bit);
        //    if (now - count < 0)
        //    {
        //        return char.Parse(this._charCollection.Substring(now - count + this._charCollection.Length, 1));
        //    }
        //    else
        //    {
        //        return char.Parse(this._charCollection.Substring(now - count, 1));
        //    }
        //}

        /// <summary>
        /// 比較2個字符在charCollection中的大小關係
        /// </summary>
        /// <param name="bit1"></param>
        /// <param name="bit2"></param>
        /// <returns>1: 大於,0: 等於, -1: 小於</returns>
        /// <remarks></remarks>
        private int Cmp(char bit1, char bit2)
        {
            return this._charCollection.IndexOf(bit1).CompareTo(this._charCollection.IndexOf(bit2));
        }

        /// <summary>
        /// 比較2個數的大小關係
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>1: 大於,0: 等於, -1: 小於</returns>
        /// <remarks></remarks>
        private int Cmp(string num1, string num2)
        {
            if (num1.Equals(num2))
            {
                return 0;
            }

            if (num1.Length > num2.Length)
            {
                return 1;
            }

            if (num1.Length < num2.Length)
            {
                return -1;
            }

            int i;
            for (i = 0; i <= num1.Length - 1; i++)
            {
                int resCmp = this.Cmp(num1.ToCharArray()[i], num2.ToCharArray()[i]);
                if (resCmp == 1)
                {
                    return 1;
                }
                else if (resCmp == -1)
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// bit1 - bit2
        /// </summary>
        /// <param name="bit1"></param>
        /// <param name="bit2"></param>
        /// <returns>差</returns>
        private int Substract(char bit1, char bit2)
        {
            return this._charCollection.IndexOf(bit1) - this._charCollection.IndexOf(bit2);
        }

        private string Convert(char[] chars)
        {
            string ret = string.Empty;
            foreach (char chrct in chars)
            {
                ret = chrct.ToString() + ret;
            }
            return ret;
        }

        /// <summary>
        /// 判斷一個數的字符是否在指定字符集中
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool InCharCollection(string str)
        {
            foreach (char s in str.ToCharArray())
            {
                if (this._charCollection.IndexOf(s) == -1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 獲得現有bit和charCollection的最大數
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetGeneralMaxNumber()
        {
            if (_generalMaxNumber == string.Empty)
            {
                char charMax = char.Parse(this._charCollection.Substring(this._charCollection.Length - 1, 1));
                string ret = charMax.ToString();
                _generalMaxNumber = ret.PadLeft(this._iBits, charMax);
            }
            return _generalMaxNumber;
        }

        /// <summary>
        /// 獲得現有bit和charCollection的最小數
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetGeneralMinNumber()
        {
            if (_generalMinNumber == string.Empty)
            {
                char charMin = char.Parse(this._charCollection.Substring(0, 1));
                string ret = charMin.ToString();
                _generalMinNumber = ret.PadLeft(this._iBits, charMin);
            }
            return _generalMinNumber;
        }

        private void CheckAndThrowExceedGeneralMaxException(string str)
        {
            if (this.Cmp(str, this.GetGeneralMaxNumber()) == 1)
            {
                ThrowExceedGeneralMaxException();
                //Throw New FisException(String.Format("The result number exceeds the general Max limit! [{0}] ", Me.GetGeneralMaxNumber()))
            }
        }

        private void ThrowExceedGeneralMaxException()
        {
            throw new FisException("GEN022", new string[] { this.GetGeneralMaxNumber() });
        }

        #endregion
    }
}
