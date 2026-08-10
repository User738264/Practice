public class OrderManager
{
    private const int DeliveryDays = 3;
    public void Run()
    {
        bool isOrderConfirmed = false;
        string productName = "";
        int count = 0;
        string clientName = "";
        string address = "";

        while ( !isOrderConfirmed )
        {
            productName = RequestNonEmptyString( "Введите название продукта: " );
            count = RequestPositiveInteger( "Введите количество: " );
            clientName = RequestNonEmptyString( "Введите имя клиента: " );
            address = RequestNonEmptyString( "Введите адрес доставки: " );

            isOrderConfirmed = IsOrderConfirmed( clientName, count, productName, address );

            if ( isOrderConfirmed )
            {
                continue;
            }
            Console.WriteLine( "Пожалуйста, повторите ввод данных." );
        }
        string formattedDate = DateTime.Today.AddDays( DeliveryDays ).ToString( "dd.MM.yyyy" );

        Console.WriteLine( $"{clientName}! Ваш заказ {productName} в количестве {count} оформлен! Ожидайте доставку по адресу {address} к {formattedDate}" );
    }

    private string RequestNonEmptyString( string prompt )
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

    private int RequestPositiveInteger( string prompt )
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

    private bool IsOrderConfirmed( string clientName, int count, string productName, string address )
    {
        Console.WriteLine( $"Здравствуйте, {clientName}, вы заказали {count} {productName} на адрес {address}, все верно? (да/нет)" );
        string? confirmation = Console.ReadLine();
        return confirmation?.ToLower().Trim() == "да";
    }
}