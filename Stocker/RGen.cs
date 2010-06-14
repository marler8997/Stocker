using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stocker.TreeMath;
using Stocker.GP;

namespace Stocker
{
    class RGen : Random
    {
        private static readonly RGen instance = new RGen();
        private RGen() : base()
        {
            quickCoin = new QuickCoin();
        }

        public static RGen gen { get { return instance; } }

        QuickCoin quickCoin;

        public MathCell newRandMathCell(Range mathCellSizeRange, int argLength)
        {
            return randCell(RGen.gen.Next(mathCellSizeRange.min, mathCellSizeRange.max), argLength);
        }

        private MathCell randCell(int cellCount, int argLength)
        {
            if (cellCount <= 1)
            {
                //must return either a variable or a double cell
                if (quickCoin.flip())
                {
                    return new VariableCell(Next(0, argLength - 1));
                }
                else
                {
                    return new DoubleCell((double)Next(-100, 100));
                }
            }
            else
            {
                //For now just always assume ops take two arguments
                if (cellCount == 2)
                {
                    return new ExpressionCell(randOp(), new MathCell[2]{
                        randCell(1,argLength), randCell(1,argLength)});
                }
                else
                {
                    int split = Next(2, cellCount - 1);
                    return new ExpressionCell(randOp(), new MathCell[2]{
                        randCell(split,argLength), randCell(cellCount-split,argLength)});
                }
            }
        }

        public OpEnum randOp()
        {
            return quickCoin.flip() ? OpEnum.add : OpEnum.mult;
        }

       


    }

    class QuickCoin
    {
        Random quickCoinRandomizer;

        private byte[] rbytes;
        private byte rIdx;
        private byte bitCount;
        private const int bitMax = 8;

        public QuickCoin()
        {
            quickCoinRandomizer = new Random();
            rbytes = new byte[4];
            rIdx = 0;
            bitCount = 0;
            quickCoinRandomizer.NextBytes(rbytes);
        }


        public bool flip()
        {
            if (bitCount >= bitMax)
            {
                bitCount = 0;
                rIdx++;
            }
            if (rIdx >= rbytes.Length)
            {
                rIdx = 0;
                quickCoinRandomizer.NextBytes(rbytes);
            }

            bool result = (byte)(rbytes[rIdx] & 00000001) > 0;

            //iterate the bits
            rbytes[rIdx] >>= 1;
            bitCount++;

            return result;
        }
    }

}
