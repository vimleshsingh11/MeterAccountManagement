using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Account.Surface;

namespace Services.Account.DomainApi.Domain.Account.Processor
{
    public interface IAccountProcessor
    {
        /// <summary>
        /// Import the meter reading
        /// </summary>
        /// <returns></returns>
        Task<string> ImportMeterReading();

        /// <summary>
        /// Reterive the list of meter reading
        /// </summary>
        /// <returns></returns>
        List<MeterDetails> GetAllMeterReading();
    }
}
