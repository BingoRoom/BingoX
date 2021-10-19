using BingoX.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class ImageExif
    {
        #region FileInfo

        //40092
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Comment")]
        public string Comment { get; set; }

        //40093
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Author")]
        public string Author { get; set; }

        //40094
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Keyword")]
        public string Keyword { get; set; }

        //40095
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Subject")]
        public string Subject { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Pix")]
        public Size? Pix { get; set; }

        //40091
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Title")]
        public string Title { get; set; }

        //33432
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Copyright")]
        public string Copyright { get; set; }

        //315
        //     public string Artist { get; set; }
        //18246 星级
        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"Rating")]
        public int Rating { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// 36864
        [Category("FileInfo"), DisplayName(@"ExifVersion")]
        public string ExifVersion { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// 270
        [Category("FileInfo"), DisplayName(@"Description")]
        public string Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"RawFormatID")]
        public Guid RawFormatID { get; set; }

        private static readonly Guid BMP = new Guid("B96B3CAB-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid PNG = new Guid("B96B3CAF-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid GIF = new Guid("B96B3CB0-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid JPEG = new Guid("B96B3CAE-0728-11D3-9D7B-0000F81EF32E");
        private static readonly Guid TIFF = new Guid("B96B3CB1-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        ///
        /// </summary>
        [Category("FileInfo"), DisplayName(@"RawFormat")]
        public string RawFormat
        {
            get
            {
                if (RawFormatID == BMP) return "BMP";
                if (RawFormatID == PNG) return "PNG";
                if (RawFormatID == GIF) return "GIF";
                if (RawFormatID == JPEG) return "JPEG";
                if (RawFormatID == TIFF) return "TIFF";
                return "unkwon";
            }
        }

        #endregion FileInfo

        #region 相機信息

        //271 镜头公司
        /// <summary>
        ///
        /// </summary>
        [Category("EquipmentInfo"), DisplayName(@"EquipmentMake")]
        public string EquipmentMake { get; set; }

        //272 镜头型号
        /// <summary>
        ///
        /// </summary>
        [Category("EquipmentInfo"), DisplayName(@"EquipmentModel")]
        public string EquipmentModel { get; set; }

        #endregion 相機信息

        #region base
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Orientation")]
        public Orientation Orientation { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ResolutionUnit")]
        public int ResolutionUnit { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"YCbCrPositioning")]
        public int YCbCrPositioning { get; set; }

        //41994 清晰度(一般\柔和\强烈)
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Sharpness")]
        public int Sharpness { get; set; }

        //37384 光源
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"LightSource")]
        public int LightSource { get; set; }

        //34850 曝光方式(無\手動\一般\光圈先決\快門先決\景深優先\快門優先\直向模式\橫向模式)
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ExposureProgram")]
        public ExposurePrograms ExposureProgram { get; set; }

        //41989 35mm胶卷
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"FocalLengthIn35mmFilm")]
        public int FocalLengthIn35mmFilm { get; set; }

        //20625
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ChrominanceTable")]
        public int ChrominanceTable { get; set; }

        //20624
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"LuminanceTable")]
        public int LuminanceTable { get; set; }

        //36867
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"DateTimeOriginal")]
        public DateTime? DateTimeOriginal { get; set; }

        //36868
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"DateTimeDigitized")]
        public DateTime? DateTimeDigitized { get; set; }

        //34855 光圈
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ISOSpeedRatings")]
        public int ISOSpeedRatings { get; set; }

        //37383 测光模式
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"MeteringMode")]
        public int MeteringMode { get; set; }

        //37385 闪光灯模式
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Flash")]
        public int Flash { get; set; }

        //41987 白平衡(手动\自动)
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"WhiteBalance")]
        public int WhiteBalance { get; set; }

        //41992 比对(标准\柔和\强烈)
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Contrast")]
        public int Contrast { get; set; }

        //41993 饱和度(标准\低饱和\高饱和)
        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Saturation")]
        public int Saturation { get; set; }

        /// <summary>
        /// 282,283
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"Resolution")]
        public SizeF? Resolution { get; set; }

        /// <summary>
        /// 37377
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ShutterSpeed")]
        public double ShutterSpeed { get; set; }

        /// <summary>
        /// 33434
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"ExposureTime")]
        public Rational ExposureTime { get; set; }

        #endregion base

        #region Thumbnail

        /// <summary>
        /// 20525,20526
        /// </summary>
        [Category("Thumbnail"), DisplayName(@"Resolution")]
        public Size? ThumbnailResolution { get; set; }

        /// <summary>
        /// 20528
        /// </summary>
        [Category("Thumbnail"), DisplayName(@"ResolutionUnit")]
        public int ThumbnailResolutionUnit { get; set; }

        /// <summary>
        /// 20515
        /// </summary>
        [Category("Thumbnail"), DisplayName(@"Compression")]
        public int ThumbnailCompression { get; set; }

        /// <summary>
        /// 20507
        /// </summary>
        [Category("Thumbnail"), DisplayName(@"Thumbnail")]
        public byte[] ThumbnailData { get; set; }

        #endregion Thumbnail

        #region GPS

        /// <summary>
        ///
        /// </summary>
        [Category("GeoInfo"), DisplayName(@"GPS")]
        public GPSGeo GPS { get; set; }

        #endregion GPS

        /// <summary>
        ///
        /// </summary>
        [Category("ImageInfo"), DisplayName(@"AllExifs")]
        public ExifPropertyCollection Properties { get; protected set; }




        public static ImageExif GetExifInfo(byte[] buffer)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer);
            return GetExifInfo(Bitmap.FromStream(ms));
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageExif GetExifInfo(string filepath)
        {
            return GetExifInfo(Bitmap.FromFile(filepath));
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageExif GetExifInfo(Image image)
        {

            //http://www.exiv2.org/tags.html
            ImageExif exif = new ImageExif
            {
                OriginalImage = image
            };
            List<ExifProperty> properties = new List<ExifProperty>();
            exif.RawFormatID = image.RawFormat.Guid;
            try
            {
                foreach (int hex in image.PropertyIdList)
                {



                    var exit = new ExifProperty(image.GetPropertyItem(hex));
                    properties.Add(exit);
                    switch ((int)hex)
                    {

                        case 274:
                            {
                                var value = Convert.ToUInt16(exit.DisplayValue);
                                if (value != 0)
                                    exif.Orientation = ObjectUtility.Cast<Orientation>(value);
                                break;
                            }
                        case 40091: exif.Title = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 40092: exif.Comment = GetStringUnicode(image, hex); break;
                        case 40093: exif.Author = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 40094: exif.Keyword = GetStringUnicode(image, hex); break;
                        case 40095: exif.Subject = GetStringUnicode(image, hex); break;
                        case 33432: exif.Copyright = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 270: exif.Description = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 271: exif.EquipmentMake = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 272: exif.EquipmentModel = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 34850:
                            {
                                var value = ObjectUtility.Cast<short>(exit.DisplayValue);
                                exif.ExposureProgram = ObjectUtility.Cast<ExposurePrograms>(value); ; break;
                            }
                        case 34855: exif.ISOSpeedRatings = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 37384: exif.Flash = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 37385: exif.LightSource = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 37383: exif.MeteringMode = ObjectUtility.Cast<int>(exit.DisplayValue); break;
                        case 18246: exif.Rating = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 41987: exif.WhiteBalance = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 41992: exif.Contrast = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 41993: exif.Saturation = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 41994: exif.Sharpness = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 33434: exif.ExposureTime = Rational.GetRational(exit.Value); break;
                        case 41989: exif.FocalLengthIn35mmFilm = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 36867: exif.DateTimeOriginal = GetDateTime(image, hex); break;
                        case 37377: exif.ShutterSpeed = GetDouble(image, hex); break;
                        case 36868: exif.DateTimeDigitized = GetDateTime(image, hex); break;
                        case 36864: exif.ExifVersion = ObjectUtility.Cast<string>(exit.DisplayValue); break;
                        case 531: exif.YCbCrPositioning = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 20625: exif.ChrominanceTable = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 20624: exif.LuminanceTable = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 20507: exif.ThumbnailData = image.GetPropertyItem(hex).Value; break;
                        case 20528: exif.ThumbnailResolutionUnit = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 20515: exif.ThumbnailCompression = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 296: exif.ResolutionUnit = ObjectUtility.Cast<short>(exit.DisplayValue); break;
                        case 282:
                        case 283:
                            {
                                var unit = exif.ResolutionUnit;
                                if (unit == 0 && image.PropertyIdList.Contains(296))
                                {
                                    unit = GetInt(image, 296);
                                }
                                var r = (unit == 3) ? 2.54f : 1f;
                                if (exif.Resolution == null && image.PropertyIdList.Contains(282) && image.PropertyIdList.Contains(283))
                                    exif.Resolution = new SizeF(r * GetInt(image, 282), r * GetInt(image, 283)); break;
                            }
                        case 20525:
                        case 20526:
                            {
                                if (exif.Pix == null && image.PropertyIdList.Contains(20525) && image.PropertyIdList.Contains(20526))
                                    exif.ThumbnailResolution = new Size(GetInt(image, 20525), GetInt(image, 20526)); break;
                            }
                        case 40962:
                        case 40963:
                            {
                                if (exif.Pix == null && image.PropertyIdList.Contains(40962) && image.PropertyIdList.Contains(40963))
                                    exif.Pix = new Size(GetInt(image, 40962), GetInt(image, 40963)); break;
                            }

                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 29:
                            {
                                if (image.PropertyIdList.Contains(1) && image.PropertyIdList.Contains(2))
                                {
                                    if (exif.GPS == null)
                                    {
                                        exif.GPS = new GPSGeo()
                                        {
                                            LatitudeRef = GetStringAsc(image, 1),
                                            Latitude = GetDouble(image, 2),
                                        };
                                    }

                                    else
                                    {
                                        exif.GPS.LatitudeRef = GetStringAsc(image, 1);
                                        exif.GPS.Latitude = GetDouble(image, 2);

                                    }

                                }
                                if (image.PropertyIdList.Contains(3) && image.PropertyIdList.Contains(4))
                                {

                                    if (exif.GPS == null)
                                    {
                                        exif.GPS = new GPSGeo()
                                        {
                                            LongitudeRef = GetStringAsc(image, 3),
                                            Longitude = GetDouble(image, 4),
                                        };
                                    }

                                    else
                                    {
                                        exif.GPS.LongitudeRef = GetStringAsc(image, 3);
                                        exif.GPS.Longitude = GetDouble(image, 4);

                                    }
                                }

                                if (image.PropertyIdList.Contains(5) && image.PropertyIdList.Contains(6))
                                {

                                    if (exif.GPS == null)
                                    {
                                        exif.GPS = new GPSGeo()
                                        {
                                            AltitudeRef = GetStringAsc(image, 5),
                                            Altitude = GetDouble(image, 6),
                                        };
                                    }

                                    else
                                    {
                                        exif.GPS.AltitudeRef = GetStringAsc(image, 5);
                                        exif.GPS.Altitude = GetDouble(image, 6);

                                    }
                                }
                                if (image.PropertyIdList.Contains(29))
                                {
                                    if (exif.GPS == null)
                                    {
                                        exif.GPS = new GPSGeo()
                                        {
                                            DateStamp = DateTime.ParseExact(GetStringAsc(image, 29), "yyyy:MM:dd", null)
                                        };
                                    }
                                    else
                                    {

                                        exif.GPS.DateStamp = DateTime.ParseExact(GetStringAsc(image, 29), "yyyy:MM:dd", null);
                                    }
                                }

                                break;
                            }
                    }

                }
                exif.Properties = new ExifPropertyCollection(properties);
            }
            catch (Exception ex)
            {


            }
            return exif;
        }
        public Image OriginalImage { get; private set; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="meteringMode"></param>
        /// <returns></returns>
        public static string GetMeteringModeName(int meteringMode)
        {
            switch (meteringMode)
            {
                case 0: return DrawResource.Unknown;
                case 1: return DrawResource.Metering_ModeAverage;
                case 2: return DrawResource.Metering_CenterWeightedAverage;
                case 3: return DrawResource.Metering_Spot;
                case 4: return DrawResource.Metering_MultiSpot;
                case 5: return DrawResource.Metering_MultiSegment;
                case 6: return DrawResource.Metering_Partial;
                case 255: return DrawResource.Other;
            }

            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lightSource"></param>
        /// <returns></returns>
        public static string GetLightSourceName(int lightSource)
        {
            switch (lightSource)
            {
                case 0: return DrawResource.Unknown;
                case 1: return DrawResource.LightSource_Daylight;
                case 2: return DrawResource.LightSource_Fluorescent;
                case 3: return DrawResource.LightSource_Tungsten;
                case 10: return DrawResource.LightSource_Flash;
                case 17: return DrawResource.LightSource_StandardLightA;
                case 18: return DrawResource.LightSource_StandardLightB;
                case 19: return DrawResource.LightSource_StandardLightC;
                case 20: return DrawResource.LightSource_D55;
                case 21: return DrawResource.LightSource_D65;
                case 22: return DrawResource.LightSource_D75;
                case 255: return DrawResource.Other;
            }
            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flash"></param>
        /// <returns></returns>
        public static string GetFlashName(int flash)
        {
            switch (flash)
            {
                case 0: return DrawResource.Flash_Name0;
                case 1: return DrawResource.Flash_Name1;
                case 5: return DrawResource.Flash_Name5;
                case 7: return DrawResource.Flash_Name7;
                case 9: return DrawResource.Flash_Name9;
                case 13: return DrawResource.Flash_Name13;
                case 15: return DrawResource.Flash_Name15;
                case 16: return DrawResource.Flash_Name16;
                case 24: return DrawResource.Flash_Name24;
                case 25: return DrawResource.Flash_Name25;
                case 29: return DrawResource.Flash_Name29;
                case 31: return DrawResource.Flash_Name31;
                case 32: return DrawResource.Flash_Name32;
                case 65: return DrawResource.Flash_Name65;
                case 69: return DrawResource.Flash_Name69;
                case 71: return DrawResource.Flash_Name71;
                case 73: return DrawResource.Flash_Name73;
                case 77: return DrawResource.Flash_Name77;
                case 79: return DrawResource.Flash_Name79;
                case 83: return DrawResource.Flash_Name83;
                case 93: return DrawResource.Flash_Name93;
                case 95: return DrawResource.Flash_Name95;
            }
            return string.Empty;
        }
        /// <summary>
        ///  Image 转化为 字节流
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public byte[] ImageToByteArray(ImageFormat format)
        {
            var ms = new MemoryStream();
            try
            {

                OriginalImage.Save(ms, format);
                return ms.ToArray();
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }

        }
        /// <summary>
        /// 主要色調（平均色）
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public Color GetDominantColor()
        {
            if (this.OriginalImage is Bitmap == false) throw new NotSupportedException("原图像类型不为 Bitmap ");
            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;
            PointBitmap bitmap = new PointBitmap(this.OriginalImage as Bitmap);
            for (int x = 0; x < OriginalImage.Width; x++)
            {
                for (int y = 0; y < OriginalImage.Height; y++)
                {
                    Color clr = bitmap.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }
        private static DateTime? GetDateTime(Image getImage, int hex)
        {
            string dateTakenTag = Encoding.ASCII.GetString(getImage.GetPropertyItem(hex).Value);
            if (dateTakenTag == null || dateTakenTag.Contains("0000:00:00 00:00:00")) return null;
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            int hour = int.Parse(parts[3]);
            int minute = int.Parse(parts[4]);
            int second = int.Parse(parts[5]);
            if (year == 0 || month == 0 || day == 0) return null;
            return new DateTime(year, month, day, hour, minute, second);
        }

        private static short GetShort(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            if (propety.Value.Length == 2)
                return BitConverter.ToInt16(value, 0);
            return 0;
        }

        private static int GetInt(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            if (propety.Value.Length == 2)
                return BitConverter.ToInt16(value, 0);

            if (propety.Value.Length == 4)
                return BitConverter.ToInt32(value, 0);
            return 0;
        }

        private static double GetDouble(Image getImage, int hex)
        {
            var propety = getImage.GetPropertyItem(hex);
            //     if (propety.Type != 5) return 0;
            var value = propety.Value;
            switch (propety.Value.Length)
            {
                case 2:
                    return BitConverter.ToInt16(value, 0);
                case 4:
                    return BitConverter.ToUInt32(value, 0);

                case 24:
                    {
                        float degrees = BitConverter.ToUInt32(value, 0) / (float)BitConverter.ToUInt32(value, 4);

                        float minutes = BitConverter.ToUInt32(value, 8) / (float)BitConverter.ToUInt32(value, 12);

                        float seconds = BitConverter.ToUInt32(value, 16) / (float)BitConverter.ToUInt32(value, 20);
                        float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
                        return coorditate;
                    }
                case 8:
                    {
                        return BitConverter.ToUInt32(value, 0) / (float)BitConverter.ToUInt32(value, 4);
                    }
            }
            return 0;
        }

        private static string GetStringUnicode(Image getImage, int hex)
        {
            string dateTakenTag = Encoding.Unicode.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }

        private static string GetStringAsc(Image getImage, int hex)
        {
            string dateTakenTag = Encoding.ASCII.GetString(getImage.GetPropertyItem(hex).Value);
            return dateTakenTag.Replace("\0", string.Empty);
        }


    }
}
