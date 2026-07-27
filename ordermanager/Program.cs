using System;

public class Program
{
    public static void Main( string[] args )
    {
        OrderManager orderManager = new OrderManager();
        orderManager.Run();
    }
}

public class OrderManager
{
    public void Run()
    {
        bool isOrderConfirmed = false;
        string productName = "";
        int num = 0;
        string clientName = "";
        string address = "";

        while ( !isOrderConfirmed )
        {
            productName = RequestNonEmptyString( "Введите название продукта: " );
            num = RequestPositiveInteger( "Введите количество: " );
            clientName = RequestNonEmptyString( "Введите имя клиента: " );
            address = RequestNonEmptyString( "Введите адрес доставки: " );

            isOrderConfirmed = ConfirmOrder( productName, num, clientName, address );

            if ( !isOrderConfirmed )
            {
                Console.WriteLine( "Пожалуйста, повторите ввод данных." );
            }
        }
        DateTime currentDate = DateTime.Today.AddDays( 3 );
        string formattedDate = currentDate.ToString( "dd.MM.yyyy" );

        Console.WriteLine( $"{clientName}! Ваш заказ {productName} в количестве {num} оформлен! Ожидайте доставку по адресу {address} к {formattedDate}" );
    }

    public string RequestNonEmptyString( string prompt )
    {
        Console.Write( prompt );
        string? input = Console.ReadLine();

        while ( string.IsNullOrWhiteSpace( input ) )
        {
            Console.Write( "Значение не может быть пустым, повторите ввод: " );
            input = Console.ReadLine();
        }

        return input;
    }

    public int RequestPositiveInteger( string prompt )
    {
        Console.Write( prompt );
        string? input = Console.ReadLine();
        int num;

        while ( !int.TryParse( input, out num ) || num <= 0 )
        {
            Console.Write( "Количество должно быть целым числом больше нуля, повторите ввод: " );
            input = Console.ReadLine();
        }

        return num;
    }

    public bool ConfirmOrder( string productName, int num, string clientName, string address )
    {
        Console.WriteLine( $"Здравствуйте, {clientName}, вы заказали {num} {productName} на адрес {address}, все верно? (да/нет)" );
        string? confirmation = Console.ReadLine();
        return confirmation?.ToLower() == "да";
    }
}