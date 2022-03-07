using System;
using Services.Account.Surface.Common;

namespace Services.Account.Surface
{
    [BsonCollection("MeterDetails")]
    public class MeterDetails : Document
    {
        /// <summary>
        /// Date of the meter reading
        /// </summary>
        public string MeterReadingDate { get; set; }

        /// <summary>
        /// Value of meter read
        /// </summary>
        public string MeterReadValue { get; set; }

    }
}

