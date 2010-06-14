using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocker.TreeMath
{

    public enum OpEnum { add, mult };

 



    public interface MathCell
    {
        double eval(double[] args);
        void printLisp();
        void printInfix();
        
    }





    public class VariableCell : MathCell
    {



        int argIndex;

        public VariableCell(int argIndex)
        {
            this.argIndex = argIndex;
        }

        public double eval(double[] args)
        {
            try
            {
                return args[argIndex];
            }
            catch (IndexOutOfRangeException iore)
            {
                Display.cout.writeLine(iore.Message + "In evaluation of a variable cell, the provided argument list has " +
                    args.Length.ToString() + " argument(s), but the current variable cell has an index of " +
                    argIndex.ToString());
                return -1;
            }
        }



        public void printLisp()
        {
            Display.cout.write("[" + argIndex.ToString() + "]");
        }

        public void printInfix()
        {
            Display.cout.write("[" + argIndex.ToString() + "]");
        }

    }


    public class DoubleCell : MathCell
    {
        private double val;
        public DoubleCell(double val)
        {
            this.val = val;
        }

        public double eval(double[] args)
        {
            return val;
        }
        

        public void printLisp()
        {
            Display.cout.write(val.ToString());
        }

        public void printInfix()
        {
            Display.cout.write(val.ToString());
        }
    }


    public class ExpressionCell : MathCell
    {
        private OpEnum op;
        private MathCell[] operationArgs;

        public ExpressionCell(OpEnum op, MathCell[] operationArgs)
        {
            this.op = op;
            this.operationArgs = operationArgs;
            checkOperationArgs();               
        }

        public void printLisp()
        {
            Display.cout.write("(" + opString(op));
            for (int i = 0; i < operationArgs.Length; i++)
            {
                Display.cout.write(" ");
                operationArgs[i].printLisp();
            }
            Display.cout.write(")");
        }

        public void printInfix()
        {
            if (operationArgs.Length == 2)
            {
                Display.cout.write("( ");
                operationArgs[0].printInfix();
                Display.cout.write(" " + opString(op) + " ");
                operationArgs[1].printInfix();
                Display.cout.write(")");
            }
            else
            {
                throw new Exception("Error: when printing infix notation for op: " +
                    op.ToString() + ", the number of arguments provided (" + operationArgs.Length +
                    ") is not being handled."); 
            }
        }


        public string opString(OpEnum op)
        {
            switch (op)
            {
                case OpEnum.add:
                    return "+";
                case OpEnum.mult:
                    return "*";
                default:
                    throw new Exception("Error: when looking up opstring, the op " +
                        op.ToString() + " was not recognized.");
            }

        }



        public double eval(double[] args)
        {
            switch (op)
            {
                case OpEnum.add:
                    return operationArgs[0].eval(args) + operationArgs[1].eval(args);
                case OpEnum.mult:
                    return operationArgs[0].eval(args) * operationArgs[1].eval(args);
                default:
                    throw new Exception("The expression op: " + op.ToString() + " is unrecognized.");
            }
        }

        public void replaceArg(int i, MathCell newCell)
        {
            if (i > operationArgs.Length)
            {
                throw new Exception("Invalid index " + i.ToString() +
                    " in replacing an argument in the " + op.ToString() + " operation.");
            }
            if (newCell == null)
            {
                throw new Exception("Cannot have a null argument.");
            }
            operationArgs[i] = newCell;
        }


        private void checkOperationArgs() {
            switch(op)
            {
                case OpEnum.add:
                    checkOperationArgLength(2);
                    break;
                case OpEnum.mult:
                    checkOperationArgLength(2);
                    break;
                default:
                    throw new Exception("The expression op: " + op.ToString() + " is unrecognized.");
            }
        }

        private void checkOperationArgLength(int length)
        {
            if (length == 0)
            {
                if (operationArgs != null)
                {
                    throw new Exception("The operation " + op.ToString() +
                        " requires no arguments but there were " + length.ToString() + " arguments provided.");
                }
            }
            else if (operationArgs == null)
            {
                throw new Exception("No arguments were provided for the " + op.ToString() + " operation.");
            }
            else if (operationArgs.Length != length)
            {
                throw new Exception("Operation " + op.ToString() + " requires " + length.ToString() +
                    " arguments but was given " + operationArgs.Length.ToString());
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    if (operationArgs[i] == null)
                    {
                        throw new Exception("Argument " + i.ToString() + " is null :(");
                    }
                }
            }
        }
        
    }
}
