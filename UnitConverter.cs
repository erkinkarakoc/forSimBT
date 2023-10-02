





using System;


public enum UnitNameEnums
{
    meter,
    kilometer,
    inch,
    foot,
    yard,
    mile,
    nauticalmile,
    meterspersecond,
    kilometersperhour,
    milesperhour,
    knot
}
public static class UnitConverter
{
    private static Unit[] units;
    private static UnitNameEnums currentLengthUnit = UnitNameEnums.meter;
    private static UnitNameEnums currentSpeedUnit = UnitNameEnums.meterspersecond;

    public delegate void OnChangeUnitSystem();
    public static OnChangeUnitSystem UnitSystemOnChanged;

    public static UnitNameEnums CurrentLengthUnit
    {
        get { return currentLengthUnit; }
        set { currentLengthUnit = value; UnitSystemOnChanged.Invoke(); }
    }
    public static UnitNameEnums CurrentSpeedUnit
    {
        get { return currentSpeedUnit; }
        set { currentSpeedUnit = value; UnitSystemOnChanged.Invoke(); }
    }
    public static void SwitchCurrentLengthUnit() 
    {
        int enumStartIndex = 0;
        int enumEndIndex = 6;

        int cur = (int)currentLengthUnit;
        cur++;
        if (cur > enumEndIndex) cur = enumStartIndex;

        currentLengthUnit = (UnitNameEnums)cur;
        UnitSystemOnChanged.Invoke();
    }
    public static void SwitchCurrentSpeedUnit()
    {
        int enumStartIndex = 7;
        int enumEndIndex = 10;

        int cur = (int)currentLengthUnit;
        cur++;
        if (cur > enumEndIndex) cur = enumStartIndex;

        currentLengthUnit = (UnitNameEnums)cur;
        UnitSystemOnChanged.Invoke();
    }
    static UnitConverter()
    {

        units = new Unit[]
        {
            new Unit(UnitNameEnums.meter, "m", "length", 1.0),
            new Unit(UnitNameEnums.kilometer, "km", "length", 1000.0),
            new Unit(UnitNameEnums.inch, "in", "length", 0.0254),
            new Unit(UnitNameEnums.foot, "ft", "length", 0.3048),
            new Unit(UnitNameEnums.yard, "yd", "length", 0.9144),
            new Unit(UnitNameEnums.mile, "mi", "length", 1609.344),
            new Unit(UnitNameEnums.nauticalmile, "nmi", "length", 1852),

            new Unit(UnitNameEnums.meterspersecond, "m/s", "speed", 1.0),
            new Unit(UnitNameEnums.kilometersperhour, "km/h", "speed", 0.27777778),
            new Unit(UnitNameEnums.milesperhour, "mph", "speed", 0.44704),
            new Unit(UnitNameEnums.knot, "mph", "speed", 0.514444444)
        };
    }

    public static string Convert(float value,UnitNameEnums fromUnit)
    {
        return Convert((double)value, fromUnit);
    }
    public static string Convert(double value, UnitNameEnums fromUnit)
    {
        
        Unit from = GetUnit(fromUnit);
        UnitNameEnums toUnit = UnitNameEnums.meter;
        if (from.DimensionName == "length")
        {
           toUnit = currentLengthUnit;
        }else if(from.DimensionName == "speed")
        {
           toUnit = currentSpeedUnit;
        }

        Unit to = GetUnit(toUnit);

        if (from != null && to != null)
        {
            if (from.DimensionName == to.DimensionName)
            {
                double result = value * (from.ConversionFactor / to.ConversionFactor);
                return $"{result.ToString("N2")} {to.Symbol}";
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

    public static string ConvertTo(float value, UnitNameEnums fromUnit, UnitNameEnums toUnit)
    {
        return ConvertTo((double)value, fromUnit, toUnit);
    }
    public static string ConvertTo(double value, UnitNameEnums fromUnit, UnitNameEnums toUnit)
    {
        Unit from = GetUnit(fromUnit);
        Unit to = GetUnit(toUnit);

        if (from != null && to != null)
        {
            if (from.DimensionName == to.DimensionName)
            {
                double result = value * (from.ConversionFactor / to.ConversionFactor);
                return $"{result.ToString("N2")} {to.Symbol}";
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




