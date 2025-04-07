using System.Globalization;
Console.Title = "Калькулятор";
PrintMenuCommand( "Выполнил: Клыков Михаил \n" );
PrintMenuCommand( "Программа: Калькулятор!\n" );
PrintMenuCommand( "Введите 'начать' или 'help': " );
string start = ReadLine();
if ( start == "help" )
{
    DisplayHelpMenu();
    Primer();
}
else if ( start == "начать" )
{
    Primer();
}
else
{
    PrintMenuCommand( "Неправильный ввод!!! \nПерезапутите программу и введите 'начать' или 'help': \n" );
}

static void DisplayHelpMenu() // Решил добавить информацию для корректного использование программы 
{
    Console.WriteLine( $@"
            Руководство:  

Программа принимает как целые так и дробные числа.
Возможные операции:
+ - сложение 2 + 3 = 5
- - вычитание 2 - 3 = -1
/ - деление 2 / 2 = 1
* - умножение 2 * 3 = 6
^ - возведение в степень 2 ^ 3 = 8
** - извлечение корня  16 ** 2 = 4
" );

    Console.WriteLine( "\nНажмите любую клавишу для выхода..." );
    Console.ReadKey();
    Console.Clear();
}
void PrintMenuCommand( string velue )  //Делали на лекции поэтому добавил
{
    Console.Write( velue );
}

string ReadLine()   //Делали на лекции поэтому добавил
{
    return Console.ReadLine();
}

static double ParseNumber( string numStr )
{
    numStr = numStr.Replace( ',', '.' );   // Меняю ,<- на точки на всякий случай

    if ( double.TryParse( numStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double result ) )
    {
        return result;
    }
    throw new Exception( $"Неверный формат числа: {numStr}" );
}

static bool Operator( char c )  // Проверяю являються ли символы математической операцией
{
    return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
}
static bool SqrtOperator( string s, int index )  // Проверка на нахождения корня, не добавил изночально так как были проблемы
{
    return index + 1 < s.Length && s[ index ] == '*' && s[ index + 1 ] == '*';
}

static List<object> DoubleArray( string input )   // Разбиваю строку на составляющие
{
    var elements = new List<object>();
    string currentNumber = "";

    for ( int i = 0; i < input.Length; i++ )
    {
        char c = input[ i ];

        if ( char.IsDigit( c ) || c == '.' || c == ',' )
        {
            currentNumber += c;
        }
        else if ( SqrtOperator( input, i ) ) // Обработка корня **
        {
            if ( !string.IsNullOrEmpty( currentNumber ) )
            {
                elements.Add( ParseNumber( currentNumber ) );
                currentNumber = "";
            }
            elements.Add( "**" ); // Добавляем оператор корня
            i++;
        }
        else if ( Operator( c ) ) // можно потом добавить еще операторов
        {
            if ( !string.IsNullOrEmpty( currentNumber ) )
            {
                elements.Add( ParseNumber( currentNumber ) );
                currentNumber = "";
            }
            elements.Add( c );
        }
        else if ( !char.IsWhiteSpace( c ) )
        {
            throw new Exception( $"Недопустимый символ: '{c}' \n Вот пример: 3.4 - 5.1" );
        }
    }

    if ( !string.IsNullOrEmpty( currentNumber ) )
    {
        elements.Add( ParseNumber( currentNumber ) );
    }

    return elements;
}

static double ComputeResult( List<object> elements ) // Это... Решает пример из сделанного массива 
{
    if ( elements.Count == 0 )
    {
        throw new Exception( "Пустое выражение" );
    }

    double result = ( double )elements[ 0 ];

    for ( int i = 1; i < elements.Count; i += 2 )
    {
        if ( i + 1 >= elements.Count )
        {
            throw new Exception( "Незавершенное выражение" );
        }

        object op = elements[ i ];
        double num = ( double )elements[ i + 1 ];

        switch ( op )
        {
            case '+': result += num; break;
            case '-': result -= num; break;
            case '*': result *= num; break;
            case '/':
                if ( num == 0 ) throw new Exception( "Деление на ноль" );
                result /= num;
                break;
            case '^':
                result = Math.Pow( result, num );
                break;
            case "**":
                if ( num == 2 )
                    result = Math.Sqrt( result );
                else
                    result = Math.Pow( result, 1.0 / num ); // Корень n-ной степени
                break;
            default:
                throw new Exception( $"Неизвестный оператор: {op}" );
        }
    }

    return result;
}

void Primer()
{
    while ( true )
    {
        PrintMenuCommand( "Введите пример: " );
        string input = ReadLine();
        try
        {
            List<object> elements = DoubleArray( input );
            double result = ComputeResult( elements );
            PrintMenuCommand( $"Ответ: {input} = {result}\n" );

        }
        catch ( Exception ex )
        {
            PrintMenuCommand( "Ошибка: " + ex.Message + "\n" );
        }
    }
}