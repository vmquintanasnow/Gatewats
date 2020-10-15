using Musala.Business.DTO;
using Musala.Business.Filters;
using Musala.Business.Payload;
using System;
using System.Threading.Tasks;

namespace Musala.Business.Services
{
    public interface IDeviceService
    {
        Task<IPayload> GetAllDevices(QueryObject queryObject);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IPayload> FindById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(int id);


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IPayload> SaveDevice(SaveDeviceDto saveDeviceDto);


    }
}
