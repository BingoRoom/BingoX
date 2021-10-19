using System;
using System.Drawing.Imaging;
using System.Text;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ExifProperty
    {
        private byte[] _value;
        private int _hex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        public ExifProperty(PropertyItem property)
        {
            Hex = property.Id;
            Type = (PropertyTagType)property.Type;
            Value = property.Value;
            //  Len = property.Len;
        }

        /// <summary>
        ///
        /// </summary>
        public PropertyTagId TagId { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public ExifProperty(int hex, PropertyTagType type, byte[] value)
        {
            this.Hex = hex;
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        public int Len { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public byte[] Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Len = _value != null ? _value.Length : 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public PropertyTagType Type { get; private set; }

        private object _displayValue;

        /// <summary>
        ///
        /// </summary>
        public object DisplayValue
        {
            get
            {
                if (_displayValue != null) return _displayValue;
                switch (Type)
                {
                    case PropertyTagType.ASCII:
                    case PropertyTagType.Undefined:
                        {
                            var str = Encoding.ASCII.GetString(Value);
                            _displayValue = str != null ? str.Trim().Replace("\0", string.Empty) : null; break;
                        }
                    case PropertyTagType.Short: _displayValue = BitConverter.ToUInt16(Value, 0); break;
                    case PropertyTagType.Long:
                    case PropertyTagType.Rational:
                        {
                            switch (Len)
                            {
                                case 2:
                                    _displayValue = BitConverter.ToInt16(Value, 0); break;
                                case 4:
                                    _displayValue = BitConverter.ToInt32(Value, 0); break;
                                case 24:
                                    {
                                        float degrees = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);

                                        float minutes = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);

                                        float seconds = BitConverter.ToInt32(Value, 16) / (float)BitConverter.ToInt32(Value, 20);
                                        float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                                        _displayValue = coorditate;
                                        break;
                                    }
                                case 8:
                                    {
                                        _displayValue = Rational.GetRational(Value);
                                        break;
                                    }
                                case 16:
                                    {
                                        float degrees = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);
                                        float minutes = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);
                                        float coorditate = degrees + (minutes / 60f);
                                        _displayValue = coorditate;
                                        break;
                                    }
                                case 48:
                                    {
                                        // red[x], red[y], green[x], green[y], blue[x], and blue[y]
                                        float redx = BitConverter.ToInt32(Value, 0) / (float)BitConverter.ToInt32(Value, 4);
                                        float redy = BitConverter.ToInt32(Value, 8) / (float)BitConverter.ToInt32(Value, 12);
                                        float greenx = BitConverter.ToInt32(Value, 16) / (float)BitConverter.ToInt32(Value, 20);
                                        float greeny = BitConverter.ToInt32(Value, 24) / (float)BitConverter.ToInt32(Value, 28);
                                        float bluex = BitConverter.ToInt32(Value, 32) / (float)BitConverter.ToInt32(Value, 36);
                                        float bluey = BitConverter.ToInt32(Value, 40) / (float)BitConverter.ToInt32(Value, 44);
                                        _displayValue = string.Format("red[x]{0} ,red[y]{1} ,green[x]{2}, green[y]{3},blue[x]{4},blue[y]{5}", redx, redy, greenx, greeny, bluex, bluey);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PropertyTagType.SLONG:
                    case PropertyTagType.SRational:
                        {
                            switch (Len)
                            {
                                case 2:
                                    _displayValue = BitConverter.ToInt16(Value, 0); break;
                                case 4:
                                    _displayValue = BitConverter.ToUInt32(Value, 0); break;
                                case 24:
                                    {
                                        float degrees = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);

                                        float minutes = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);

                                        float seconds = BitConverter.ToUInt32(Value, 16) / (float)BitConverter.ToUInt32(Value, 20);
                                        float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                                        _displayValue = coorditate;
                                        break;
                                    }
                                case 8:
                                    {
                                        _displayValue = URational.GetRational(Value);
                                        break;
                                    }
                                case 16:
                                    {
                                        float degrees = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);
                                        float minutes = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);
                                        float coorditate = degrees + (minutes / 60f);
                                        _displayValue = coorditate;
                                        break;
                                    }
                                case 48:
                                    {
                                        // red[x], red[y], green[x], green[y], blue[x], and blue[y]
                                        float redx = BitConverter.ToUInt32(Value, 0) / (float)BitConverter.ToUInt32(Value, 4);
                                        float redy = BitConverter.ToUInt32(Value, 8) / (float)BitConverter.ToUInt32(Value, 12);
                                        float greenx = BitConverter.ToUInt32(Value, 16) / (float)BitConverter.ToUInt32(Value, 20);
                                        float greeny = BitConverter.ToUInt32(Value, 24) / (float)BitConverter.ToUInt32(Value, 28);
                                        float bluex = BitConverter.ToUInt32(Value, 32) / (float)BitConverter.ToUInt32(Value, 36);
                                        float bluey = BitConverter.ToUInt32(Value, 40) / (float)BitConverter.ToUInt32(Value, 44);
                                        _displayValue = string.Format("red[x]{0} ,red[y]{1} ,green[x]{2}, green[y]{3},blue[x]{4},blue[y]{5}", redx, redy, greenx, greeny, bluex, bluey);
                                        break;
                                    }
                            }
                            break;
                        }
                }
                return _displayValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Hex
        {
            private set
            {
                _hex = value;
                TagId = (PropertyTagId)value;
            }
            get { return _hex; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", TagId, Type);
        }
    }
}
