using System.Text.RegularExpressions;

public class Program
{
    public static void Main( string[] args )
    {
        string pattern = @"^(?<first>-?\d+)(?<op>[+\-*/])(?<second>-?\d+)$";
        Console.WriteLine( "Введите выражение в формате: {firstOperand}{operation}{secondOperand}" );
        string? input = Console.ReadLine();
        while ( string.IsNullOrEmpty( input ) )
        {
            Console.WriteLine( "Ввод отсутствует." );
            input = Console.ReadLine();
        }
        Match match = Regex.Match( input, pattern );
        if ( match.Success )
        {
            string firstOperand = match.Groups[ "first" ].Value;
            string op = match.Groups[ "op" ].Value;
            string secondOperand = match.Groups[ "second" ].Value;
            if ( !int.TryParse( firstOperand, out int first ) || !int.TryParse( secondOperand, out int second ) )
            {
                Console.WriteLine( "Операнд слишком большой или некорректный." );
                return;
            }
            try
            {
                int result = Calculator.Calculate( first, op, second );
                Console.WriteLine( result );
            }
            catch ( OverflowException )
            {
                Console.WriteLine( "Переполнение!" );
            }
            catch ( DivideByZeroException )
            {
                Console.WriteLine( "Деление на ноль!" );
            }
        }
        else
        {
            Console.WriteLine( "Выражение не соответствует формату." );
        }
    }
}

public class Calculator
{
    public static int Calculate( int first, string op, int second )
    {
        return op switch
        {
            "+" => checked(first + second),
            "-" => checked(first - second),
            "*" => checked(first * second),
            "/" => second != 0 ? checked(first / second) : throw new DivideByZeroException(),
            _ => throw new InvalidOperationException( "Недопустимая операция." ),
        };
    }
}