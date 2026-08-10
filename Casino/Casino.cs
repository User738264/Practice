using System;

public class Casino
{
    private const int MaxRandomNumberExclusive = 21;
    private const int WinThreshold = 17;
    private const int WinMultiplicator = 2;
    private const int FormulaModulus = 17;

    private int balance;

    public void Run()
    {
        PrintLogo();

        balance = RequestInitialBalance();

        while ( true )
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
                    PlayGame();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine( "Неверный выбор. Попробуйте снова." );
                    break;
            }
        }
    }

    private void PrintLogo()
    {
        Console.WriteLine( " ####     ##     ####   ######  #    #   #### " );
        Console.WriteLine( "#    #   #  #   #    #    ##    ##   #  #    #" );
        Console.WriteLine( "#       #    #  #         ##    # #  #  #    #" );
        Console.WriteLine( "#       ######   ####     ##    #  # #  #    #" );
        Console.WriteLine( "#    #  #    #       #    ##    #   ##  #    #" );
        Console.WriteLine( " ####   #    #  ####    ######  #    #   #### " );
    }

    private int RequestInitialBalance()
    {
        Console.WriteLine( "Введите начальный баланс:" );
        int initialBalance;

        while ( !int.TryParse( Console.ReadLine(), out initialBalance ) || initialBalance <= 0 )
        {
            Console.WriteLine( "Баланс должен быть целым числом больше нуля, попробуйте снова:" );
        }

        return initialBalance;
    }

    private void PlayGame()
    {
        Console.WriteLine( $"Введите ставку, от 1 до {balance}:" );

        if ( !int.TryParse( Console.ReadLine(), out int bet ) || bet <= 0 || bet > balance )
        {
            Console.WriteLine( "Неверная ставка. Попробуйте снова." );
            return;
        }

        int randomNum = Random.Shared.Next( 1, MaxRandomNumberExclusive );
        Console.WriteLine( $"Выпало число: {randomNum}" );

        if ( randomNum > WinThreshold )
        {
            // {bet} * (1 + ({multiplicator} * {random_num} % 17))
            int newBalance = bet * ( 1 + ( WinMultiplicator * randomNum % FormulaModulus ) );
            Console.WriteLine( $"Поздравляем! Вы выиграли! Ваш баланс был увеличен на {newBalance}" );
            balance += newBalance;
            Console.WriteLine( $"Ваш новый баланс: {balance}" );
        }
        else
        {
            Console.WriteLine( $"К сожалению, вы проиграли. Ваш баланс был уменьшен на {bet}" );
            balance -= bet;
            Console.WriteLine( $"Ваш новый баланс: {balance}" );
        }
    }
}