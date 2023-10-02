using System.Globalization;
using System.Text;



    public static class UnitConverter
    {
        

        #region Consts
        const string METER_SYMBOL = " m";
        const string KILOMETER_SYMBOL = " km";
        const string INCH_SYMBOL = " in";
        const string FOOT_SYMBOL = " ft";
        const string YARD_SYMBOL = " yd";
        const string MILE_SYMBOL = " mi";
        const string NAUTICALMILE_SYMBOL = " nmi";
        const string METERPERSECOND_SYMBOL = " m/s";
        const string KILOMETERPERHOUR_SYMBOL = " kph";
        const string MILEPERHOUR_SYMBOL = " mph";
        const string KNOT_SYMBOL = " knot";

        const float METER_TO_KILOMETER = 0.001F;
        const float METER_TO_INCH = 39.3700787F;
        const float METER_TO_FOOT = 3.2808399F;
        const float METER_TO_YARD = 1.0936133F;
        const float METER_TO_MILE = 0.000621371192F;
        const float METER_TO_NAUTICALMILE = 0.000539956803F;
        const float METERPERSECOND_TO_KMPERHOUR = 3.6F;
        const float METERPERSECOND_TO_MILEPERHOUR = 2.23693629F;
        const float METERPERSECOND_TO_KNOT = 1.94384449F;

        #endregion

        public delegate void OnChangeUnitSystem();
        public static OnChangeUnitSystem UnitSystemOnChanged;

        public static UnitSystemEnum currentUnitSystemEnum = UnitSystemEnum.Metric;

        #region Enums


        public enum UnitSystemEnum
        {
            Metric,
            ImperialFtMi,
            ImperialYdNmi,
            ImperialInchMi
        }
        public enum LengthUnitsEnum
        {
            meter,
            kilometer,
            inch,
            foot,
            yard,
            mile,
            nauticalmile
        }
        public enum SpeedUnitsEnum
        {
            meterPerSecond,
            kilometerPerHour,
            milePerHour,
            knot
        }
        #endregion


        #region GeneralMethods
        private static string SetDecimalPrecision(float length, int decimalPrecision)
        {
            NumberFormatInfo setPrecision = new NumberFormatInfo();
            setPrecision.NumberDecimalDigits = decimalPrecision;

            return length.ToString("N", setPrecision);
        }
        #endregion




        #region Length Methods

        public static float ConvertLength(double length, LengthUnitsEnum fromLengthUnit)
        {
            return ConvertLength((float)length, fromLengthUnit);
        }
        public static float ConvertLength(float length, LengthUnitsEnum fromLengthUnit)
        {
            float result = ConvertToMeter(length, fromLengthUnit);
            LengthUnitsEnum curLengthUnit;
            return ConvertToCurLengthUnit(result, out curLengthUnit);
        }
        public static string ConvertLength(double length, LengthUnitsEnum fromLengthUnit, int decimalPrecision, bool AddSymbol = false)
        {
            return ConvertLength((float)length, fromLengthUnit, decimalPrecision, AddSymbol);
        }
        public static string ConvertLength(float length,LengthUnitsEnum fromLengthUnit,int decimalPrecision,bool AddSymbol = false)
        {
            float result = ConvertToMeter(length,fromLengthUnit);
            LengthUnitsEnum curLengthUnit;
            result = ConvertToCurLengthUnit(result, out curLengthUnit);

            if (AddSymbol)
            {
                string resultStr = SetDecimalPrecision(result, decimalPrecision);
                return AddLengthSymbolToString(resultStr, curLengthUnit);
            }
            else
            {
                return SetDecimalPrecision(result, decimalPrecision);
            }
        }

      

        public static float ConvertLengthTo(double length, LengthUnitsEnum fromLengthUnit, LengthUnitsEnum wantedLengthUnit)
        {
            return ConvertLengthTo((float)length, fromLengthUnit, wantedLengthUnit);
        }
        public static float ConvertLengthTo(float length, LengthUnitsEnum fromLengthUnit, LengthUnitsEnum wantedLengthUnit)
        {
            float result = ConvertToMeter(length, fromLengthUnit);
            result = ConvertMeterToWantedLengthUnit(result, wantedLengthUnit);

            return result;
        }

        public static string ConvertLengthTo(double length, LengthUnitsEnum fromLengthUnit, LengthUnitsEnum wantedLengthUnit, int decimalPrecision, bool AddSymbol = false)
        {
            return ConvertLengthTo((float)length, fromLengthUnit, wantedLengthUnit,decimalPrecision, AddSymbol);
        }

        public static string ConvertLengthTo(float length, LengthUnitsEnum fromLengthUnit, LengthUnitsEnum wantedLengthUnit, int decimalPrecision,bool AddSymbol =false)
        {
            float result = ConvertToMeter(length, fromLengthUnit);
            result = ConvertMeterToWantedLengthUnit(result, wantedLengthUnit);

            if (AddSymbol)
            {
                string resultStr = SetDecimalPrecision(result, decimalPrecision);
                return AddLengthSymbolToString(resultStr, wantedLengthUnit);
            }
            else
            {
                return SetDecimalPrecision(result, decimalPrecision);
            }
            
        }
        
        
        private static float ConvertToMeter(float length,LengthUnitsEnum fromLengthUnit)
        {
            switch (fromLengthUnit)
            {
                case LengthUnitsEnum.meter:
                    return length;
                    
                case LengthUnitsEnum.kilometer:
                    return length / METER_TO_KILOMETER;
                    
                case LengthUnitsEnum.inch:
                    return length / METER_TO_INCH;
                    
                case LengthUnitsEnum.foot:
                    return length / METER_TO_FOOT;
                    
                case LengthUnitsEnum.yard:
                    return length / METER_TO_YARD;
                    
                case LengthUnitsEnum.mile:
                    return length / METER_TO_MILE;
                    
                case LengthUnitsEnum.nauticalmile:
                    return length / METER_TO_NAUTICALMILE;
                    
                default:
                    return length;
                    
            }
        }
        private static float ConvertToCurLengthUnit(float length, out LengthUnitsEnum curLengthUnit)
        {
            if(length > 1000f)
            {
                switch (currentUnitSystemEnum)
                {
                    case UnitSystemEnum.Metric:
                        curLengthUnit = LengthUnitsEnum.kilometer;
                        return length * METER_TO_KILOMETER;

                    case UnitSystemEnum.ImperialFtMi:
                        curLengthUnit = LengthUnitsEnum.mile;
                        return length * METER_TO_MILE;
                        
                    case UnitSystemEnum.ImperialYdNmi:
                        curLengthUnit = LengthUnitsEnum.nauticalmile;
                        return length * METER_TO_NAUTICALMILE;
                        
                    case UnitSystemEnum.ImperialInchMi:
                        curLengthUnit = LengthUnitsEnum.mile;
                        return length * METER_TO_MILE;
                        
                    default:
                        curLengthUnit = LengthUnitsEnum.kilometer;
                        return length * METER_TO_KILOMETER;
                }
            }
            else
            {
                switch (currentUnitSystemEnum)
                {
                    case UnitSystemEnum.Metric:
                        curLengthUnit = LengthUnitsEnum.meter;
                        return length;

                    case UnitSystemEnum.ImperialFtMi:
                        curLengthUnit = LengthUnitsEnum.foot;
                        return length * METER_TO_FOOT;

                    case UnitSystemEnum.ImperialYdNmi:
                        curLengthUnit = LengthUnitsEnum.yard;
                        return length * METER_TO_YARD;

                    case UnitSystemEnum.ImperialInchMi:
                        curLengthUnit = LengthUnitsEnum.inch;
                        return length * METER_TO_INCH;

                    default:
                        curLengthUnit = LengthUnitsEnum.meter;
                        return length;
                }
            }
          
        }
        private static float ConvertMeterToWantedLengthUnit(float length,LengthUnitsEnum wantedLengthUnit)
        {
            switch (wantedLengthUnit)
            {
                case LengthUnitsEnum.meter:
                    return length;
                    
                case LengthUnitsEnum.kilometer:
                    return length * METER_TO_KILOMETER;
                    
                case LengthUnitsEnum.inch:
                    return length * METER_TO_INCH;
                
                case LengthUnitsEnum.foot:
                    return length * METER_TO_FOOT;

                case LengthUnitsEnum.yard:
                    return length * METER_TO_YARD;

                case LengthUnitsEnum.mile:
                    return length * METER_TO_MILE;

                case LengthUnitsEnum.nauticalmile:
                    return length * METER_TO_NAUTICALMILE;

                default:
                    return length;
                    
            }
        }
     

        private static string AddLengthSymbolToString(string str,LengthUnitsEnum curLengthUnit)
        {
            StringBuilder @string = new StringBuilder();
            @string.Append(str);

            switch (curLengthUnit)
            {
                case LengthUnitsEnum.meter:
                    @string.Append(METER_SYMBOL);
                    break;
                case LengthUnitsEnum.kilometer:
                    @string.Append(KILOMETER_SYMBOL);
                    break;
                case LengthUnitsEnum.inch:
                    @string.Append(INCH_SYMBOL);
                    break;
                case LengthUnitsEnum.foot:
                    @string.Append(FOOT_SYMBOL);
                    break;
                case LengthUnitsEnum.yard:
                    @string.Append(YARD_SYMBOL);
                    break;
                case LengthUnitsEnum.mile:
                    @string.Append(MILE_SYMBOL);
                    break;
                case LengthUnitsEnum.nauticalmile:
                    @string.Append(NAUTICALMILE_SYMBOL);
                    break;
                default:
                    @string.Append(METER_SYMBOL);
                    break;
            }

            return @string.ToString();
        }
        #endregion

        #region Speed Methods

        public static float ConvertSpeed(double speed, SpeedUnitsEnum fromSpeedUnit)
        {
            return ConvertSpeed((float)speed, fromSpeedUnit);
        }
        public static float ConvertSpeed(float speed, SpeedUnitsEnum fromSpeedUnit)
        {
            float result = ConvertToMPS(speed, fromSpeedUnit);
            SpeedUnitsEnum curSpeedUnit;
            return ConvertToCurSpeedUnit(result, out curSpeedUnit);
        }
        public static string ConvertSpeed(double speed, SpeedUnitsEnum fromSpeedUnit, int decimalPrecision, bool AddSymbol = false)
        {
            return ConvertSpeed((float)speed, fromSpeedUnit, decimalPrecision, AddSymbol);
        }
        public static string ConvertSpeed(float speed, SpeedUnitsEnum fromSpeedUnit, int decimalPrecision, bool AddSymbol = false)
        {
            float result = ConvertToMPS(speed, fromSpeedUnit);
            SpeedUnitsEnum curSpeedUnit;
            result = ConvertToCurSpeedUnit(result, out curSpeedUnit);

            if (AddSymbol)
            {
                string resultStr = SetDecimalPrecision(result, decimalPrecision);
                return AddSpeedSymbolToString(resultStr, curSpeedUnit);
            }
            else
            {
                return SetDecimalPrecision(result, decimalPrecision);
            }
        }

        public static float ConvertSpeedTo(double speed, SpeedUnitsEnum fromSpeedUnit, SpeedUnitsEnum wantedSpeedUnit)
        {
            return ConvertSpeedTo((float)speed, fromSpeedUnit, wantedSpeedUnit);
        }
        public static float ConvertSpeedTo(float speed, SpeedUnitsEnum fromSpeedUnit, SpeedUnitsEnum wantedSpeedUnit)
        {
            float result = ConvertToMPS(speed, fromSpeedUnit);
            result = ConvertMPSToWantedSpeedUnit(result, wantedSpeedUnit);

            return result;
        }
        public static string ConvertSpeedTo(double speed, SpeedUnitsEnum fromSpeedUnit, SpeedUnitsEnum wantedSpeedUnit, int decimalPrecision, bool AddSymbol = false)
        {
            return ConvertSpeedTo((float)speed, fromSpeedUnit, wantedSpeedUnit, decimalPrecision, AddSymbol);
        }
        public static string ConvertSpeedTo(float speed, SpeedUnitsEnum fromSpeedUnit, SpeedUnitsEnum wantedSpeedUnit, int decimalPrecision, bool AddSymbol = false)
        {
            float result = ConvertToMPS(speed, fromSpeedUnit);
            result = ConvertMPSToWantedSpeedUnit(result, wantedSpeedUnit);

            if (AddSymbol)
            {
                string resultStr = SetDecimalPrecision(result, decimalPrecision);
                return AddSpeedSymbolToString(resultStr, wantedSpeedUnit);
            }
            else
            {
                return SetDecimalPrecision(result, decimalPrecision);
            }

        }

        private static float ConvertToMPS(float speed, SpeedUnitsEnum fromSpeedUnit)
        {
            switch (fromSpeedUnit)
            {
                case SpeedUnitsEnum.meterPerSecond:
                    return speed;

                case SpeedUnitsEnum.kilometerPerHour:
                    return speed / METER_TO_KILOMETER;

                case SpeedUnitsEnum.milePerHour:
                    return speed / METERPERSECOND_TO_MILEPERHOUR;

                case SpeedUnitsEnum.knot:
                    return speed / METERPERSECOND_TO_KNOT;

                default:
                    return speed;
            }
        }
        private static float ConvertToCurSpeedUnit(float speed, out SpeedUnitsEnum curSpeedUnit)
        {
            switch (currentUnitSystemEnum)
            {
                case UnitSystemEnum.Metric:
                    curSpeedUnit = SpeedUnitsEnum.kilometerPerHour;
                    return speed * METERPERSECOND_TO_KMPERHOUR;

                case UnitSystemEnum.ImperialFtMi:
                    curSpeedUnit = SpeedUnitsEnum.milePerHour;
                    return speed * METERPERSECOND_TO_MILEPERHOUR;

                case UnitSystemEnum.ImperialYdNmi:
                    curSpeedUnit = SpeedUnitsEnum.knot;
                    return speed * METERPERSECOND_TO_KNOT;

                case UnitSystemEnum.ImperialInchMi:
                    curSpeedUnit = SpeedUnitsEnum.milePerHour;
                    return speed * METERPERSECOND_TO_MILEPERHOUR;

                default:
                    curSpeedUnit = SpeedUnitsEnum.kilometerPerHour;
                    return speed * METERPERSECOND_TO_KMPERHOUR;
            }

        }
        private static float ConvertMPSToWantedSpeedUnit(float speed, SpeedUnitsEnum wantedSpeedUnit)
        {
            switch (wantedSpeedUnit)
            {
                case SpeedUnitsEnum.meterPerSecond:
                    return speed;
                case SpeedUnitsEnum.kilometerPerHour:
                    return speed * METERPERSECOND_TO_KMPERHOUR;
                case SpeedUnitsEnum.milePerHour:
                    return speed * METERPERSECOND_TO_MILEPERHOUR;
                case SpeedUnitsEnum.knot:
                    return speed * METERPERSECOND_TO_KNOT;
                default:
                    return speed;
            }
            
        }
        private static string AddSpeedSymbolToString(string str, SpeedUnitsEnum curSpeedUnit)
        {
            StringBuilder @string = new StringBuilder();
            @string.Append(str);

            switch (curSpeedUnit)
            {
                case SpeedUnitsEnum.meterPerSecond:
                    @string.Append(METERPERSECOND_SYMBOL);
                    break;
                case SpeedUnitsEnum.kilometerPerHour:
                    @string.Append(KILOMETERPERHOUR_SYMBOL);
                    break;
                case SpeedUnitsEnum.milePerHour:
                    @string.Append(MILEPERHOUR_SYMBOL);
                    break;
                case SpeedUnitsEnum.knot:
                    @string.Append(KNOT_SYMBOL);
                    break;
                default:
                    @string.Append(METERPERSECOND_SYMBOL);
                    break;
            }

            return @string.ToString();
        }
        #endregion

    }

