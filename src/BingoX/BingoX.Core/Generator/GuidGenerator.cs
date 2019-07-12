using System;

namespace BingoX.Generator
{
    public class GuidGenerator : IGenerator<Guid>
    {
        public GuidGenerator(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException(String.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId));
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(String.Format("datacenter Id can't be greater than {0} or less than 0", MaxDatacenterId));
            }
        }
        const int WorkerIdBits = 5;
        const int DatacenterIdBits = 5;
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }
        public long Sequence
        {
            get { return _sequence; }
            protected set { _sequence = value; }
        }
        private static readonly DateTime Jan1st1970 = new DateTime
         (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = InternalCurrentTimeMillis();
            while (timestamp <= lastTimestamp)
            {
                timestamp = InternalCurrentTimeMillis();
            }
            return timestamp;
        }
        private long _sequence = 0L;
        object lockobj = new object();
        private long _lastTimestamp;
        public Guid New()
        {
            lock (lockobj)
            {

                var timestamp = InternalCurrentTimeMillis();

                if (timestamp < _lastTimestamp)
                {
                    //exceptionCounter.incr(1);
                    //log.Error("clock is moving backwards.  Rejecting requests until %d.", _lastTimestamp);
                    throw new InvalidSystemClock(String.Format(
                        "Clock moved backwards.  Refusing to generate id for {0} milliseconds", _lastTimestamp - timestamp));
                }

                if (_lastTimestamp == timestamp)
                {

                    timestamp = TilNextMillis(_lastTimestamp);

                }
                _sequence = (_sequence + 1) ;
                _lastTimestamp = timestamp;
                //    var uid = Guid.NewGuid().ToByteArray();
                var binDate = BitConverter.GetBytes(_lastTimestamp);
                var binsequence = BitConverter.GetBytes(_sequence);
                var bitShift = BitConverter.GetBytes((DatacenterId << 16) | (WorkerId ));
                var secuentialGuid = new byte[16];
                secuentialGuid[0] = binsequence[0];
                secuentialGuid[1] = binsequence[1];
                secuentialGuid[2] = binsequence[2];
                secuentialGuid[3] = binsequence[3];

                secuentialGuid[4] = bitShift[0];
                secuentialGuid[5] = bitShift[1];
                secuentialGuid[6] = bitShift[2];
                secuentialGuid[7] = bitShift[3];

                secuentialGuid[8] = binDate[0];
                secuentialGuid[9] = binDate[1];
                secuentialGuid[10] = binDate[2];
                secuentialGuid[11] = binDate[3];
                secuentialGuid[12] = binDate[4];
                secuentialGuid[13] = binDate[5];
                secuentialGuid[14] = binDate[6];
                secuentialGuid[15] = binDate[7];

                return new Guid(secuentialGuid);
            }
        }
    }
}
