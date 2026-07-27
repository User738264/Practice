using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main( string[] args )
    {
        Casino casino = new Casino();
        casino.Run();
    }
}

public class Casino
{
    public void Run()
    {
        int balance = 100;
        bool isRunning = true;

        Console.WriteLine( " ####     ##     ####   ######  #    #   #### " );
        Console.WriteLine( "#    #   #  #   #    #    ##    ##   #  #    #" );
        Console.WriteLine( "#       #    #  #         ##    # #  #  #    #" );
        Console.WriteLine( "#       ######   ####     ##    #  # #  #    #" );
        Console.WriteLine( "#    #  #    #       #    ##    #   ##  #    #" );
        Console.WriteLine( " ####   #    #  ####    ######  #    #   #### " );

        while ( isRunning )
        {
            Console.WriteLine( "Выберите действие:" );
            Console.WriteLine( "1. Баланс" );
            Console.WriteLine( "2. Играть" );
            Console.WriteLine( "3. Выход" );

            switch ( Console.ReadLine() )
            {
                case "1":
                    Console.WriteLine( $"Ваш баланс: {balance}" );
                    break;
                case "2":
                    balance = PlayGame( balance );
                    break;
                case "3":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine( "Неверный выбор. Попробуйте снова." );
                    break;
            }
        }
    }

    public int PlayGame( int balance )
    {
        Console.WriteLine( $"Введите ставку, от нуля до {balance}:" );
        if ( int.TryParse( Console.ReadLine(), out int bet ) && bet > 0 && bet <= balance )
        {
            int newBalance = 0;
            Random random = new Random();
            int randomNum = random.Next( 1, 21 );
            Console.WriteLine( $"Выпало число: {randomNum}" );
            if ( randomNum > 17 )
            {
                // {bet} * (1 + ({multiplicator} * {random_num} % 17))
                newBalance = bet * ( 1 + ( 2 * randomNum % 17 ) );
                Console.WriteLine( "Поздравляем! Вы выиграли! Ваш баланс был увеличен на " + newBalance );
                balance += newBalance;
                Console.WriteLine( "Ваш новый баланс: " + ( balance ) );
                return balance;
            }
            else
            {
                Console.WriteLine( "К сожалению, вы проиграли. Ваш баланс был уменьшен на " + bet );
                balance -= bet;
                Console.WriteLine( "Ваш новый баланс: " + ( balance ) );
                return balance;
            }
        }
        else
        {
            Console.WriteLine( "Неверная ставка. Попробуйте снова." );
            return balance;
        }
    }
}