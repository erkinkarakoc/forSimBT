using System;
using System.Collections.Generic;

public class UnitConverter
{
    private Dictionary<string, Unit> unitTable;
   

    public UnitConverter()
    {
        unitTable = new Dictionary<string, Unit>();
       

        // Add predefined units
        AddUnit("meter", "m", "Length", 1.0);
        AddUnit("foot", "ft", "Length", 0.3048);
        AddUnit("inch", "in", "Length", 0.0254);

        AddUnit("kilogram", "kg", "Weight", 1.0);
        AddUnit("pound", "lb", "Weight", 0.45359237);
        AddUnit("gram", "g", "Weight", 0.001);

        AddUnit("m/s", "m/s", "Speed", 1.0);
        AddUnit("km/h", "km/h", "Speed", 0.27777778);
        AddUnit("mph", "mph", "Speed", 0.44704);
    }

   

    public void AddUnit(string name, string symbol, string dimensionName, double conversionFactor)
    {
        
            if (!unitTable.ContainsKey(name))
            {
                unitTable[name] = new Unit(name, symbol, dimensionName, conversionFactor);
            }
            else
            {
                Console.WriteLine($"Unit '{name}' already exists in the table.");
            }
        
        
    }

    public string Convert(double value, string fromUnitName, string toUnitName)
    {
        if (unitTable.ContainsKey(fromUnitName) && unitTable.ContainsKey(toUnitName))
        {
            var fromUnit = unitTable[fromUnitName];
            var toUnit = unitTable[toUnitName];

            if (fromUnit.DimensionName == toUnit.DimensionName)
            {
                double result = value * (fromUnit.ConversionFactor / toUnit.ConversionFactor);
                return $"{value} {fromUnit.Symbol} = {result} {toUnit.Symbol}";
            }
            else
            {
                return $"Cannot convert between units of different dimensions: {fromUnit.Name} to {toUnit.Name}.";
            }
        }
        else
        {
            return "Invalid units specified.";
        }
    }
}

public class Unit
{
    public string Name { get; }
    public string Symbol { get; }
    public string DimensionName { get; }
    public double ConversionFactor { get; }

    public Unit(string name, string symbol, string dimensionName, double conversionFactor)
    {
        Name = name;
        Symbol = symbol;
        DimensionName = dimensionName;
        ConversionFactor = conversionFactor;
    }
}


