﻿namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public enum PropertyTagId
    {
        /// <summary>
        ///
        /// </summary>
        GpsVer = 0x0000,

        /// <summary>
        ///
        /// </summary>
        GpsLatitudeRef = 0x0001,

        /// <summary>
        ///
        /// </summary>
        GpsLatitude = 0x0002,

        /// <summary>
        ///
        /// </summary>
        GpsLongitudeRef = 0x0003,

        /// <summary>
        ///
        /// </summary>
        GpsLongitude = 0x0004,

        /// <summary>
        ///
        /// </summary>
        GpsAltitudeRef = 0x0005,

        /// <summary>
        ///
        /// </summary>
        GpsAltitude = 0x0006,

        /// <summary>
        ///
        /// </summary>
        GpsGpsTime = 0x0007,

        /// <summary>
        ///
        /// </summary>
        GpsGpsSatellites = 0x0008,

        /// <summary>
        ///
        /// </summary>
        GpsGpsStatus = 0x0009,

        /// <summary>
        ///
        /// </summary>
        GpsGpsMeasureMode = 0x000A,

        /// <summary>
        ///
        /// </summary>
        GpsGpsDop = 0x000B,

        /// <summary>
        ///
        /// </summary>
        GpsSpeedRef = 0x000C,

        /// <summary>
        ///
        /// </summary>
        GpsSpeed = 0x000D,

        /// <summary>
        ///
        /// </summary>
        GpsTrackRef = 0x000E,

        /// <summary>
        ///
        /// </summary>
        GpsTrack = 0x000F,

        /// <summary>
        ///
        /// </summary>
        GpsImgDirRef = 0x0010,

        /// <summary>
        ///
        /// </summary>
        GpsImgDir = 0x0011,

        /// <summary>
        ///
        /// </summary>
        GpsMapDatum = 0x0012,

        /// <summary>
        ///
        /// </summary>
        GpsDestLatRef = 0x0013,

        /// <summary>
        ///
        /// </summary>
        GpsDestLat = 0x0014,

        /// <summary>
        ///
        /// </summary>
        GpsDestLongRef = 0x0015,

        /// <summary>
        ///
        /// </summary>
        GpsDestLong = 0x0016,

        /// <summary>
        ///
        /// </summary>
        GpsDestBearRef = 0x0017,

        /// <summary>
        ///
        /// </summary>
        GpsDestBear = 0x0018,

        /// <summary>
        ///
        /// </summary>
        GpsDestDistRef = 0x0019,

        /// <summary>
        ///
        /// </summary>
        GpsDestDist = 0x001A,

        /// <summary>
        ///
        /// </summary>
        NewSubfileType = 0x00FE,

        /// <summary>
        ///
        /// </summary>
        SubfileType = 0x00FF,

        /// <summary>
        ///
        /// </summary>
        ImageWidth = 0x0100,

        /// <summary>
        ///
        /// </summary>
        ImageHeight = 0x0101,

        /// <summary>
        ///
        /// </summary>
        BitsPerSample = 0x0102,

        /// <summary>
        ///
        /// </summary>
        Compression = 0x0103,

        /// <summary>
        ///
        /// </summary>
        PhotometricInterp = 0x0106,

        /// <summary>
        ///
        /// </summary>
        ThreshHolding = 0x0107,

        /// <summary>
        ///
        /// </summary>
        CellWidth = 0x0108,

        /// <summary>
        ///
        /// </summary>
        CellHeight = 0x0109,

        /// <summary>
        ///
        /// </summary>
        FillOrder = 0x010A,

        /// <summary>
        ///
        /// </summary>
        DocumentName = 0x010D,

        /// <summary>
        ///
        /// </summary>
        ImageDescription = 0x010E,

        /// <summary>
        ///
        /// </summary>
        EquipMake = 0x010F,

        /// <summary>
        ///
        /// </summary>
        EquipModel = 0x0110,

        /// <summary>
        ///
        /// </summary>
        StripOffsets = 0x0111,

        /// <summary>
        ///
        /// </summary>
        Orientation = 0x0112,

        /// <summary>
        ///
        /// </summary>
        SamplesPerPixel = 0x0115,

        /// <summary>
        ///
        /// </summary>
        RowsPerStrip = 0x0116,

        /// <summary>
        ///
        /// </summary>
        StripBytesCount = 0x0117,

        /// <summary>
        ///
        /// </summary>
        MinSampleValue = 0x0118,

        /// <summary>
        ///
        /// </summary>
        MaxSampleValue = 0x0119,

        /// <summary>
        ///
        /// </summary>
        XResolution = 0x011A,

        /// <summary>
        ///
        /// </summary>
        YResolution = 0x011B,

        /// <summary>
        ///
        /// </summary>
        PlanarConfig = 0x011C,

        /// <summary>
        ///
        /// </summary>
        PageName = 0x011D,

        /// <summary>
        ///
        /// </summary>
        XPosition = 0x011E,

        /// <summary>
        ///
        /// </summary>
        YPosition = 0x011F,

        /// <summary>
        ///
        /// </summary>
        FreeOffset = 0x0120,

        /// <summary>
        ///
        /// </summary>
        FreeByteCounts = 0x0121,

        /// <summary>
        ///
        /// </summary>
        GrayResponseUnit = 0x0122,

        /// <summary>
        ///
        /// </summary>
        GrayResponseCurve = 0x0123,

        /// <summary>
        ///
        /// </summary>
        T4Option = 0x0124,

        /// <summary>
        ///
        /// </summary>
        T6Option = 0x0125,

        /// <summary>
        ///
        /// </summary>
        ResolutionUnit = 0x0128,

        /// <summary>
        ///
        /// </summary>
        PageNumber = 0x0129,

        /// <summary>
        ///
        /// </summary>
        TransferFunction = 0x012D,

        /// <summary>
        ///
        /// </summary>
        SoftwareUsed = 0x0131,

        /// <summary>
        ///
        /// </summary>
        DateTime = 0x0132,

        /// <summary>
        ///
        /// </summary>
        Artist = 0x013B,

        /// <summary>
        ///
        /// </summary>
        HostComputer = 0x013C,

        /// <summary>
        ///
        /// </summary>
        Predictor = 0x013D,

        /// <summary>
        ///
        /// </summary>
        WhitePoint = 0x013E,

        /// <summary>
        ///
        /// </summary>
        PrimaryChromaticities = 0x013F,

        /// <summary>
        ///
        /// </summary>
        ColorMap = 0x0140,

        /// <summary>
        ///
        /// </summary>
        HalftoneHints = 0x0141,

        /// <summary>
        ///
        /// </summary>
        TileWidth = 0x0142,

        /// <summary>
        ///
        /// </summary>
        TileLength = 0x0143,

        /// <summary>
        ///
        /// </summary>
        TileOffset = 0x0144,

        /// <summary>
        ///
        /// </summary>
        TileByteCounts = 0x0145,

        /// <summary>
        ///
        /// </summary>
        InkSet = 0x014C,

        /// <summary>
        ///
        /// </summary>
        InkNames = 0x014D,

        /// <summary>
        ///
        /// </summary>
        NumberOfInks = 0x014E,

        /// <summary>
        ///
        /// </summary>
        DotRange = 0x0150,

        /// <summary>
        ///
        /// </summary>
        TargetPrinter = 0x0151,

        /// <summary>
        ///
        /// </summary>
        ExtraSamples = 0x0152,

        /// <summary>
        ///
        /// </summary>
        SampleFormat = 0x0153,

        /// <summary>
        ///
        /// </summary>
        SMinSampleValue = 0x0154,

        /// <summary>
        ///
        /// </summary>
        SMaxSampleValue = 0x0155,

        /// <summary>
        ///
        /// </summary>
        TransferRange = 0x0156,

        /// <summary>
        ///
        /// </summary>
        JPEGProc = 0x0200,

        /// <summary>
        ///
        /// </summary>
        JPEGInterFormat = 0x0201,

        /// <summary>
        ///
        /// </summary>
        JPEGInterLength = 0x0202,

        /// <summary>
        ///
        /// </summary>
        JPEGRestartInterval = 0x0203,

        /// <summary>
        ///
        /// </summary>
        JPEGLosslessPredictors = 0x0205,

        /// <summary>
        ///
        /// </summary>
        JPEGPointTransforms = 0x0206,

        /// <summary>
        ///
        /// </summary>
        JPEGQTables = 0x0207,

        /// <summary>
        ///
        /// </summary>
        JPEGDCTables = 0x0208,

        /// <summary>
        ///
        /// </summary>
        JPEGACTables = 0x0209,

        /// <summary>
        ///
        /// </summary>
        YCbCrCoefficients = 0x0211,

        /// <summary>
        ///
        /// </summary>
        YCbCrSubsampling = 0x0212,

        /// <summary>
        ///
        /// </summary>
        YCbCrPositioning = 0x0213,

        /// <summary>
        ///
        /// </summary>
        REFBlackWhite = 0x0214,

        /// <summary>
        ///
        /// </summary>
        Gamma = 0x0301,

        /// <summary>
        ///
        /// </summary>
        ICCProfileDescriptor = 0x0302,

        /// <summary>
        ///
        /// </summary>
        SRGBRenderingIntent = 0x0303,

        /// <summary>
        ///
        /// </summary>
        ImageTitle = 0x0320,

        /// <summary>
        ///
        /// </summary>
        ResolutionXUnit = 0x5001,

        /// <summary>
        ///
        /// </summary>
        ResolutionYUnit = 0x5002,

        /// <summary>
        ///
        /// </summary>
        ResolutionXLengthUnit = 0x5003,

        /// <summary>
        ///
        /// </summary>
        ResolutionYLengthUnit = 0x5004,

        /// <summary>
        ///
        /// </summary>
        PrintFlags = 0x5005,

        /// <summary>
        ///
        /// </summary>
        PrintFlagsVersion = 0x5006,

        /// <summary>
        ///
        /// </summary>
        PrintFlagsCrop = 0x5007,

        /// <summary>
        ///
        /// </summary>
        PrintFlagsBleedWidth = 0x5008,

        /// <summary>
        ///
        /// </summary>
        PrintFlagsBleedWidthScale = 0x5009,

        /// <summary>
        ///
        /// </summary>
        HalftoneLPI = 0x500A,

        /// <summary>
        ///
        /// </summary>
        HalftoneLPIUnit = 0x500B,

        /// <summary>
        ///
        /// </summary>
        HalftoneDegree = 0x500C,

        /// <summary>
        ///
        /// </summary>
        HalftoneShape = 0x500D,

        /// <summary>
        ///
        /// </summary>
        HalftoneMisc = 0x500E,

        /// <summary>
        ///
        /// </summary>
        HalftoneScreen = 0x500F,

        /// <summary>
        ///
        /// </summary>
        JPEGQuality = 0x5010,

        /// <summary>
        ///
        /// </summary>
        GridSize = 0x5011,

        /// <summary>
        ///
        /// </summary>
        ThumbnailFormat = 0x5012,

        /// <summary>
        ///
        /// </summary>
        ThumbnailWidth = 0x5013,

        /// <summary>
        ///
        /// </summary>
        ThumbnailHeight = 0x5014,

        /// <summary>
        ///
        /// </summary>
        ThumbnailColorDepth = 0x5015,

        /// <summary>
        ///
        /// </summary>
        ThumbnailPlanes = 0x5016,

        /// <summary>
        ///
        /// </summary>
        ThumbnailRawBytes = 0x5017,

        /// <summary>
        ///
        /// </summary>
        ThumbnailSize = 0x5018,

        /// <summary>
        ///
        /// </summary>
        ThumbnailCompressedSize = 0x5019,

        /// <summary>
        ///
        /// </summary>
        ColorTransferFunction = 0x501A,

        /// <summary>
        ///
        /// </summary>
        ThumbnailData = 0x501B,

        /// <summary>
        ///
        /// </summary>
        ThumbnailImageWidth = 0x5020,

        /// <summary>
        ///
        /// </summary>
        ThumbnailImageHeight = 0x5021,

        /// <summary>
        ///
        /// </summary>
        ThumbnailBitsPerSample = 0x5022,

        /// <summary>
        ///
        /// </summary>
        ThumbnailCompression = 0x5023,

        /// <summary>
        ///
        /// </summary>
        ThumbnailPhotometricInterp = 0x5024,

        /// <summary>
        ///
        /// </summary>
        ThumbnailImageDescription = 0x5025,

        /// <summary>
        ///
        /// </summary>
        ThumbnailEquipMake = 0x5026,

        /// <summary>
        ///
        /// </summary>
        ThumbnailEquipModel = 0x5027,

        /// <summary>
        ///
        /// </summary>
        ThumbnailStripOffsets = 0x5028,

        /// <summary>
        ///
        /// </summary>
        ThumbnailOrientation = 0x5029,

        /// <summary>
        ///
        /// </summary>
        ThumbnailSamplesPerPixel = 0x502A,

        /// <summary>
        ///
        /// </summary>
        ThumbnailRowsPerStrip = 0x502B,

        /// <summary>
        ///
        /// </summary>
        ThumbnailStripBytesCount = 0x502C,

        /// <summary>
        ///
        /// </summary>
        ThumbnailResolutionX = 0x502D,

        /// <summary>
        ///
        /// </summary>
        ThumbnailResolutionY = 0x502E,

        /// <summary>
        ///
        /// </summary>
        ThumbnailPlanarConfig = 0x502F,

        /// <summary>
        ///
        /// </summary>
        ThumbnailResolutionUnit = 0x5030,

        /// <summary>
        ///
        /// </summary>
        ThumbnailTransferFunction = 0x5031,

        /// <summary>
        ///
        /// </summary>
        ThumbnailSoftwareUsed = 0x5032,

        /// <summary>
        ///
        /// </summary>
        ThumbnailDateTime = 0x5033,

        /// <summary>
        ///
        /// </summary>
        ThumbnailArtist = 0x5034,

        /// <summary>
        ///
        /// </summary>
        ThumbnailWhitePoint = 0x5035,

        /// <summary>
        ///
        /// </summary>
        ThumbnailPrimaryChromaticities = 0x5036,

        /// <summary>
        ///
        /// </summary>
        ThumbnailYCbCrCoefficients = 0x5037,

        /// <summary>
        ///
        /// </summary>
        ThumbnailYCbCrSubsampling = 0x5038,

        /// <summary>
        ///
        /// </summary>
        ThumbnailYCbCrPositioning = 0x5039,

        /// <summary>
        ///
        /// </summary>
        ThumbnailRefBlackWhite = 0x503A,

        /// <summary>
        ///
        /// </summary>
        ThumbnailCopyRight = 0x503B,

        /// <summary>
        ///
        /// </summary>
        LuminanceTable = 0x5090,

        /// <summary>
        ///
        /// </summary>
        ChrominanceTable = 0x5091,

        /// <summary>
        ///
        /// </summary>
        FrameDelay = 0x5100,

        /// <summary>
        ///
        /// </summary>
        LoopCount = 0x5101,

        /// <summary>
        ///
        /// </summary>
        GlobalPalette = 0x5102,

        /// <summary>
        ///
        /// </summary>
        IndexBackground = 0x5103,

        /// <summary>
        ///
        /// </summary>
        IndexTransparent = 0x5104,

        /// <summary>
        ///
        /// </summary>
        PixelUnit = 0x5110,

        /// <summary>
        ///
        /// </summary>
        PixelPerUnitX = 0x5111,

        /// <summary>
        ///
        /// </summary>
        PixelPerUnitY = 0x5112,

        /// <summary>
        ///
        /// </summary>
        PaletteHistogram = 0x5113,

        /// <summary>
        ///
        /// </summary>
        Copyright = 0x8298,

        /// <summary>
        ///
        /// </summary>
        ExifExposureTime = 0x829A,

        /// <summary>
        ///
        /// </summary>
        ExifFNumber = 0x829D,

        /// <summary>
        ///
        /// </summary>
        ExifIFD = 0x8769,

        /// <summary>
        ///
        /// </summary>
        ICCProfile = 0x8773,

        /// <summary>
        ///
        /// </summary>
        ExifExposureProg = 0x8822,

        /// <summary>
        ///
        /// </summary>
        ExifSpectralSense = 0x8824,

        /// <summary>
        ///
        /// </summary>
        GpsIFD = 0x8825,
        /// <summary>
        /// 整个场景中的主要被摄体的位置和面积
        /// </summary>
        SubjectLocation = 0x9214,
        /// <summary>
        ///
        /// </summary>
        ExifISOSpeed = 0x8827,
        /// <summary>
        /// 
        /// </summary>
        CameraSerialNumber = 0xc62f,
        /// <summary>
        ///
        /// </summary>
        ExifOECF = 0x8828,

        /// <summary>
        ///
        /// </summary>
        ExifVer = 0x9000,

        /// <summary>
        ///
        /// </summary>
        ExifDTOrig = 0x9003,

        /// <summary>
        ///
        /// </summary>
        ExifDTDigitized = 0x9004,

        /// <summary>
        ///
        /// </summary>
        ExifCompConfig = 0x9101,

        /// <summary>
        ///
        /// </summary>
        ExifCompBPP = 0x9102,

        /// <summary>
        ///
        /// </summary>
        ExifShutterSpeed = 0x9201,

        /// <summary>
        ///
        /// </summary>
        ExifAperture = 0x9202,

        /// <summary>
        ///
        /// </summary>
        ExifBrightness = 0x9203,

        /// <summary>
        ///
        /// </summary>
        ExifExposureBias = 0x9204,

        /// <summary>
        ///
        /// </summary>
        ExifMaxAperture = 0x9205,

        /// <summary>
        ///
        /// </summary>
        ExifSubjectDist = 0x9206,

        /// <summary>
        ///
        /// </summary>
        ExifMeteringMode = 0x9207,

        /// <summary>
        ///
        /// </summary>
        ExifLightSource = 0x9208,

        /// <summary>
        ///
        /// </summary>
        ExifFlash = 0x9209,

        /// <summary>
        ///
        /// </summary>
        ExifFocalLength = 0x920A,

        /// <summary>
        ///
        /// </summary>
        ExifMakerNote = 0x927C,

        /// <summary>
        ///
        /// </summary>
        ExifUserComment = 0x9286,

        /// <summary>
        ///
        /// </summary>
        ExifDTSubsec = 0x9290,

        /// <summary>
        ///
        /// </summary>
        ExifDTOrigSS = 0x9291,

        /// <summary>
        ///
        /// </summary>
        ExifDTDigSS = 0x9292,

        /// <summary>
        ///
        /// </summary>
        ExifFPXVer = 0xA000,

        /// <summary>
        ///
        /// </summary>
        ExifColorSpace = 0xA001,

        /// <summary>
        ///
        /// </summary>
        ExifPixXDim = 0xA002,

        /// <summary>
        ///
        /// </summary>
        ExifPixYDim = 0xA003,

        /// <summary>
        ///
        /// </summary>
        ExifRelatedWav = 0xA004,

        /// <summary>
        ///
        /// </summary>
        ExifInterop = 0xA005,

        /// <summary>
        ///
        /// </summary>
        ExifFlashEnergy = 0xA20B,

        /// <summary>
        ///
        /// </summary>
        ExifSpatialFR = 0xA20C,

        /// <summary>
        ///
        /// </summary>
        ExifFocalXRes = 0xA20E,

        /// <summary>
        ///
        /// </summary>
        ExifFocalYRes = 0xA20F,

        /// <summary>
        ///
        /// </summary>
        ExifFocalResUnit = 0xA210,

        /// <summary>
        ///
        /// </summary>
        ExifSubjectLoc = 0xA214,

        /// <summary>
        ///
        /// </summary>
        ExifExposureIndex = 0xA215,

        /// <summary>
        ///
        /// </summary>
        ExifSensingMethod = 0xA217,

        /// <summary>
        ///
        /// </summary>
        ExifFileSource = 0xA300,

        /// <summary>
        ///
        /// </summary>
        ExifSceneType = 0xA301,

        /// <summary>
        ///
        /// </summary>
        ExifCfaPattern = 0xA302
    }
}
