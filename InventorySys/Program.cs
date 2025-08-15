using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Interface
public interface IInventoryEntity
{
    int Id { get; }
}

// Record
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// Generic Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    // Add item to in memory list
    public void Add(T item)
    {
        _log.Add(item);
    }

    // Get all items in memory
    public List<T> GetAll()
    {
        return new List<T>(_log);
    }

    // Save all items to file as JSON
    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.Write(json);
            }
            Console.WriteLine($"Data saved to {_filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving file: " + ex.Message);
        }
    }

    // Load all items from file into memory
    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File not found. No data loaded.");
                return;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string json = reader.ReadToEnd();
                var items = JsonSerializer.Deserialize<List<T>>(json);
                if (items != null)
                {
                    _log = items;
                }
            }
            Console.WriteLine("Data loaded from file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading file: " + ex.Message);
        }
    }

    // Clear in-memory list (simulate a fresh start)
    public void ClearMemory()
    {
        _log.Clear();
    }
}

// App Class
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Printer", 2, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Mouse", 10, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Keyboard", 7, DateTime.Now));
        _logger.Add(new InventoryItem(5, "Monitor", 3, DateTime.Now));
        Console.WriteLine("Sample data seeded.");
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("No items to display.");
            return;
        }

        Console.WriteLine("\nInventory Items:");
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
        }
    }

    public void ClearMemory()
    {
        _logger.ClearMemory();
    }
}

// Main
public class Program
{
    public static void Main()
    {
        string filePath = "inventory.json";
        var app = new InventoryApp(filePath);

        // Step 1: Seed data and save to file
        app.SeedSampleData();
        app.SaveData();

        // Step 2: Clear memory to simulate fresh start
        app.ClearMemory();

        // Step 3: Load from file and print
        app.LoadData();
        app.PrintAllItems();
    }
}

