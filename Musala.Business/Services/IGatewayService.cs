using Musala.Business.DTO;
using Musala.Business.Filters;
using Musala.Business.Payload;
using Musala.Business.Exceptions;
using System;
using System.Threading.Tasks;

namespace Musala.Business.Services
{
    public interface IGatewayService
    {
        Task<IPayload> GetAllGateways(QueryObject queryObject);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IPayload> FindById(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="AlreadyExistException"/>
        /// <param name="id"></param>
        /// <returns></returns>
        IPayload SaveGateway(SaveGatewayDto saveGatewayDto);
    }
}
