

public static class UnitConverter
{
    private static Unit[] units;

    static UnitConverter()
    {
        
        units = new Unit[]
        {
            new Unit(UnitNameEnums.Meter, "m", "Length", 1.0),
            new Unit(UnitNameEnums.Foot, "ft", "Length", 0.3048),
            new Unit(UnitNameEnums.Inch, "in", "Length", 0.0254),

            new Unit(UnitNameEnums.Kilogram, "kg", "Weight", 1.0),
            new Unit(UnitNameEnums.Pound, "lb", "Weight", 0.45359237),
            new Unit(UnitNameEnums.Gram, "g", "Weight", 0.001),

            new Unit(UnitNameEnums.MetersPerSecond, "m/s", "Speed", 1.0),
            new Unit(UnitNameEnums.KilometersPerHour, "km/h", "Speed", 0.27777778),
            new Unit(UnitNameEnums.MilesPerHour, "mph", "Speed", 0.44704)
        };
    }

    public static string Convert(double value, UnitNameEnums fromUnit, UnitNameEnums toUnit)
    {
        Unit from = GetUnit(fromUnit);
        Unit to = GetUnit(toUnit);

        if (from != null && to != null)
        {
            if (from.DimensionName == to.DimensionName)
            {
                double result = value * (from.ConversionFactor / to.ConversionFactor);
                return $"{value} {from.Symbol} = {result} {to.Symbol}";
            }
            else
            {
                throw new Exception($"Cannot convert between units of different dimensions: {from.Name} to {to.Name}. {System.Environment.StackTrace}");
                
            }
        }
        else
        {
            throw new Exception($"Invalid units specified.: {fromUnit} to {toUnit}. {System.Environment.StackTrace}");
            
        }
    }
    public static void AddUnit(UnitNameEnums name, string symbol, string dimensionName, double conversionFactor)
    {
        Unit unit = new Unit(name, symbol, dimensionName, conversionFactor);
        units.Append(unit);
    }
    private static Unit GetUnit(UnitNameEnums name)
    {
        foreach (var unit in units)
        {
            if (unit.Name == name)
            {
                return unit;
            }
        }
        return null;
    }
    class Unit
    {
        public UnitNameEnums Name { get; }
        public string Symbol { get; }
        public double ConversionFactor { get; }
        public string DimensionName { get; }

        public Unit(UnitNameEnums name, string symbol, string dimensionName, double conversionFactor)
        {
            Name = name;
            Symbol = symbol;
            ConversionFactor = conversionFactor;
            DimensionName = dimensionName;
        }
    }
}

public enum UnitNameEnums
{
    Meter,
    Foot,
    Inch,
    Kilogram,
    Pound,
    Gram,
    MetersPerSecond,
    KilometersPerHour,
    MilesPerHour
}


