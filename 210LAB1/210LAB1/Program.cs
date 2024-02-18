class Program
{
    #region main
    static void Main(string[] args)
    {
        //переменная для выбора действия
        int choice = 0;
        //присваиваем переменным возвращаемые из методов значения
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        Console.WriteLine("Список действий");
        Console.WriteLine("1. Найти фабрику соответствующую установке");
        Console.WriteLine("2. Найти установку по имени принадлежащего ей резервуара");
        Console.WriteLine("3. Вычислить суммарный объём резервуаров в массиве");
        Console.Write("Выберите действие: ");

        //ввод пользователя с проверкой
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Значение должно быть числом");
            return;
        }

        try
        {
            switch (choice)
            {
                // Находим фабрику по установке
                case 1:
                    try
                    {
                        var foundFactory = FindFactory(factories, units[0]);
                        Console.WriteLine("Фабрика найдена: " + foundFactory.Name);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("Индекс вышел за пределы массива 'units'");
                    }
                    break;
                // Находим принадлежность резервуара установке и заводу
                case 2:

                    var foundUnit = FindUnit(units, tanks, "Резервуар 2");
                    var factory = FindFactory(factories, foundUnit);
                    Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");
                    break;
                // Находи общий объём резервуаров в массиве
                case 3:
                    var totalVolume = GetTotalVolume(tanks);
                    Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    #endregion
    #region Методы, возвращающие массивы данных
    /// <summary>
    /// Метод, возвращающий массив резервуаров, установленный согласно приложенным таблицам
    /// </summary>
    public static Tank[] GetTanks()
    {
        //Создаём массив экземпляров класса Tank
        Tank[] tanksList =
        [
            new Tank(1, "Резервуар 1", "Надземный-вертикальный", 1500, 2000, 1),
            new Tank(2, "Резервуар 2", "Надземный-горизонтальный", 2500, 3000, 1),
            new Tank(3, "Дополнительный резервуар 24", "Надземный-горизонтальный", 3000, 3000, 2),
            new Tank(4, "Резервуар 35", "Надземный-вертикальный", 3000, 3000, 2),
            new Tank(5, "Резервуар 47", "Подземный-двустенный", 4000, 5000, 2),
            new Tank(6, "Резервуар 256", "Подводный", 500, 500, 3)
        ];

        return tanksList;
    }
    /// <summary>
    /// Метод, возвращающий массив установок, установленный согласно приложенным таблицам
    /// </summary>
    public static Unit[] GetUnits()
    {
        // Создаем массив экземпляров класса Unit
        Unit[] unitsList =
        [
            new Unit(1, "ГФУ-2", "Газофракционирующая установка", 1),
            new Unit(2, "АВТ-6", "Атмосферно-вакуумная трубчатка", 1),
            new Unit(3, "АВТ-10", "Атмосферно-вакуумная трубчатка", 2)
        ];
        // Возвращаем массив данных
        return unitsList;
    }

    /// <summary>
    /// Метод, возвращающий массив заводов, установленный согласно приложенным таблицам
    /// </summary>
    public static Factory[] GetFactories()
    {
        // Создаем массив экземпляров класса Factory
        Factory[] factoriesList =
            [
                new Factory(1, "НПЗ№1", "Первый нефтеперерабатывающий завод"),
                new Factory(2, "НПЗ№2", "Второй нефтеперерабатывающий завод")
            ];
        // Возвращаем массив данных
        return factoriesList;
    }
    #endregion
    #region Методы поиска данных в массивах данных

    /// <summary>
    /// Метод, возвращающий установку (Unit), которой
    /// принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
    /// </summary>
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string unitName)
    {
        int? foundedTankID = null;
        // Проходим по массиву резервуаров
        foreach (Tank tank in tanks)
        {
            // Если находим резервуар по имени, то запоминаем его UnitID и выходим из цикла
            if (unitName == tank.Name)
            {
                foundedTankID = tank.UnitId;
                break;
            }
        }
        // Проверяем найден ли резервуар, если найден, то проходим по массиву установок
        // если находим подходящую, то возвращаем её, иначе вызываем исключение
        if (foundedTankID != null)
        {
            foreach (Unit unit in units)
            {
                if (foundedTankID == unit.Id)
                {
                    return unit;
                }
            }
        }
        throw new Exception($"Фабрика не была найдена для установки '{unitName}'");
    }

    /// <summary>
    /// Метод, возвращающий экземпляр завода, соответствующий установке
    /// </summary>
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        foreach (Factory factory in factories)
        {
            if (unit.FactoryId == factory.Id)
            {
                return factory;
            }
        }

        // Если ни один завод не соответствует условиям, выбросим исключение
        throw new Exception("Фабрика не была найдена для заданной установки");
    }

    /// <summary>
    /// Метод, возвращающий суммарный объем резервуаров в массиве
    /// </summary>
    public static int GetTotalVolume(Tank[] units)
    {
        int totalVolume = 0;

        foreach (Tank tank in units)
        {
            totalVolume += tank.Volume;
        }
        return totalVolume;
    }
    #endregion
}
#region Классы, описывающие таблицы
/// <summary>
/// Установка
/// </summary>
public class Unit
{
    // Свойства класса
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryId { get; set; }

    // Конструктор с параметрами
    public Unit(int id, string name, string description, int factoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        FactoryId = factoryId;
    }
}

/// <summary>
/// Завод
/// </summary>
public class Factory
{
    // Свойства класса
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Конструктор с параметрами
    public Factory(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

}

/// <summary>
/// Резервуар
/// </summary>
public class Tank
{
    // Свойства класса
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }

    // Конструктор с параметрами
    public Tank(int id, string name, string description, int volume, int maxVolume, int unitId)
    {
        Id = id;
        Name = name;
        Description = description;
        Volume = volume;
        MaxVolume = maxVolume;
        UnitId = unitId;
    }
}
#endregion
