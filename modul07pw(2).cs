using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IObserver
{
    void Update(string stock, decimal price);
}

public interface ISubject
{
    void RegisterObserver(string stock, IObserver observer);
    void RemoveObserver(string stock, IObserver observer);
    void NotifyObservers(string stock);
}

public class StockExchange : ISubject
{
    private Dictionary<string, decimal> _stockPrices = new Dictionary<string, decimal>();
    private Dictionary<string, List<IObserver>> _observers = new Dictionary<string, List<IObserver>>();

    public void RegisterObserver(string stock, IObserver observer)
    {
        if (!_observers.ContainsKey(stock))
        {
            _observers[stock] = new List<IObserver>();
        }
        _observers[stock].Add(observer);
        Console.WriteLine($"{observer.GetType().Name} подписан на {stock}");
    }

    public void RemoveObserver(string stock, IObserver observer)
    {
        if (_observers.ContainsKey(stock))
        {
            _observers[stock].Remove(observer);
            Console.WriteLine($"{observer.GetType().Name} отписан от {stock}");
        }
    }

    public void NotifyObservers(string stock)
    {
        if (_observers.ContainsKey(stock))
        {
            foreach (var observer in _observers[stock])
            {
                observer.Update(stock, _stockPrices[stock]);
            }
        }
    }

    public void SetStockPrice(string stock, decimal price)
    {
        _stockPrices[stock] = price;
        Console.WriteLine($"Цена акции {stock} изменилась на {price:C}");
        NotifyObservers(stock);
    }
}

public class Trader : IObserver
{
    private string _name;

    public Trader(string name)
    {
        _name = name;
    }

    public void Update(string stock, decimal price)
    {
        Console.WriteLine($"{_name} получил обновление: Акция {stock}, Новая цена: {price:C}");
    }
}

public class TradingRobot : IObserver
{
    private decimal _buyThreshold;
    private decimal _sellThreshold;

    public TradingRobot(decimal buyThreshold, decimal sellThreshold)
    {
        _buyThreshold = buyThreshold;
        _sellThreshold = sellThreshold;
    }

    public void Update(string stock, decimal price)
    {
        if (price <= _buyThreshold)
        {
            Console.WriteLine($"Торговый робот покупает {stock} по цене {price:C}");
        }
        else if (price >= _sellThreshold)
        {
            Console.WriteLine($"Торговый робот продает {stock} по цене {price:C}");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        StockExchange exchange = new StockExchange();

        Trader trader1 = new Trader("Трейдер Иван");
        Trader trader2 = new Trader("Трейдер Ольга");
        TradingRobot robot1 = new TradingRobot(100, 150);
        
        exchange.RegisterObserver("AAPL", trader1);
        exchange.RegisterObserver("AAPL", robot1);
        exchange.RegisterObserver("GOOG", trader2);

        exchange.SetStockPrice("AAPL", 95);
        await Task.Delay(1000);

        exchange.SetStockPrice("AAPL", 155);
        await Task.Delay(1000);

        exchange.SetStockPrice("GOOG", 1200);
        await Task.Delay(1000);

        exchange.RemoveObserver("AAPL", trader1);

        exchange.SetStockPrice("AAPL", 90);
    }
}