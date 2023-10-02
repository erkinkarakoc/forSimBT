  public enum UnitSystemEnum
        {
            Metric,
            ImperialFtMi,
            ImperialYdNmi,
            ImperialInchMi
        }
public enum UnitNameEnums
{
    Meter,
    KiloMeter,
    Inch,
    Foot,
    Yard,
    Mile,
    NauticalMile,
    MetersPerSecond,
    KilometersPerHour,
    MilesPerHour,
    Knot
}
public static class UnitConverter
{
    private static Unit[] units;
    private static UnitSystemEnum currentUnitSystemEnum;

    public delegate void OnChangeUnitSystem();
    public static OnChangeUnitSystem UnitSystemOnChanged;
    
    public static UnitSystemEnum CurrentUnitSystemEnum{
        get {return currentUnitSystemEnum;}
        set {currentUnitSystemEnum = value; UnitSystemOnChanged.Invoke();}
    }
    
    static UnitConverter()
    {
        
        units = new Unit[]
        {
            new Unit(UnitNameEnums.Meter, "m", "length", 1.0),
            new Unit(UnitNameEnums.Meter, "km", "length", 1000.0),
            new Unit(UnitNameEnums.Inch, "m", "length", 1.0),
            new Unit(UnitNameEnums.Foot, "ft", "length", 0.3048),
            new Unit(UnitNameEnums.Inch, "in", "Length", 0.0254),
            
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




