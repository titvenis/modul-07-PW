using System;

public interface ICostCalculationStrategy
{
    decimal CalculateCost(decimal distance, int passengers, string serviceClass, bool hasDiscount);
}

public class PlaneCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, int passengers, string serviceClass, bool hasDiscount)
    {
        decimal baseCost = distance * 0.5m * passengers;
        decimal serviceMultiplier = serviceClass == "business" ? 1.5m : 1.0m;
        decimal discount = hasDiscount ? 0.9m : 1.0m;

        return baseCost * serviceMultiplier * discount;
    }
}

public class TrainCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, int passengers, string serviceClass, bool hasDiscount)
    {
        decimal baseCost = distance * 0.2m * passengers;
        decimal serviceMultiplier = serviceClass == "business" ? 1.3m : 1.0m;
        decimal discount = hasDiscount ? 0.85m : 1.0m;

        return baseCost * serviceMultiplier * discount;
    }
}

public class BusCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(decimal distance, int passengers, string serviceClass, bool hasDiscount)
    {
        decimal baseCost = distance * 0.1m * passengers;
        decimal serviceMultiplier = serviceClass == "business" ? 1.2m : 1.0m;
        decimal discount = hasDiscount ? 0.8m : 1.0m;

        return baseCost * serviceMultiplier * discount;
    }
}

public class TravelBookingContext
{
    private ICostCalculationStrategy _costStrategy;

    public void SetCostStrategy(ICostCalculationStrategy strategy)
    {
        _costStrategy = strategy;
    }

    public decimal CalculateTravelCost(decimal distance, int passengers, string serviceClass, bool hasDiscount)
    {
        if (_costStrategy == null)
        {
            throw new InvalidOperationException("Стратегия расчета стоимости не установлена.");
        }

        return _costStrategy.CalculateCost(distance, passengers, serviceClass, hasDiscount);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var context = new TravelBookingContext();

        Console.WriteLine("Выберите тип транспорта: 1 - Самолет, 2 - Поезд, 3 - Автобус");
        int transportChoice = int.Parse(Console.ReadLine());

        switch (transportChoice)
        {
            case 1:
                context.SetCostStrategy(new PlaneCostStrategy());
                break;
            case 2:
                context.SetCostStrategy(new TrainCostStrategy());
                break;
            case 3:
                context.SetCostStrategy(new BusCostStrategy());
                break;
            default:
                Console.WriteLine("Неверный выбор транспорта.");
                return;
        }

        Console.WriteLine("Введите расстояние в километрах:");
        decimal distance = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Введите количество пассажиров:");
        int passengers = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите класс обслуживания: economy / business");
        string serviceClass = Console.ReadLine();

        Console.WriteLine("Есть ли скидка? (да / нет)");
        bool hasDiscount = Console.ReadLine().ToLower() == "да";

        try
        {
            decimal cost = context.CalculateTravelCost(distance, passengers, serviceClass, hasDiscount);
            Console.WriteLine($"Стоимость поездки: {cost:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}