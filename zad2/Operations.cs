using System;


namespace zad2
{
    public interface IOperation
    {
        decimal DoOperation(decimal val1, decimal val2);
        string Symbol { get; }
    }

    public class Sum : IOperation
    {
        public decimal DoOperation(decimal val1, decimal val2) => val1 + val2;
        public string Symbol => "+";
    }

    public class Subtraction : IOperation
    {
        public decimal DoOperation(decimal val1, decimal val2) => val1 - val2;
        public string Symbol => "-";
    }

    public class Division : IOperation
    {
        public decimal DoOperation(decimal val1, decimal val2) => val1 / val2;

        public string Symbol => "/";
    }

    public class Multiplication : IOperation
    {
        public decimal DoOperation(decimal val1, decimal val2) => val1 * val2;

        public string Symbol => "*";
    }
}
