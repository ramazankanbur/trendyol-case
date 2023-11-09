using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Service
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateList(List<LogDTO> data)
        {
            var logRepository = _unitOfWork.GetRepository<Log>();

            var packageStatusTypeToAdd = _mapper.Map<List<Log>>(data);

            packageStatusTypeToAdd.ForEach(x =>
            {
                logRepository.Add(x);
            });

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<ListResult<LogDTO>> GetLogsAsnyc(int page = 1, int size = 50)
        {
            var response = new ListResult<LogDTO>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 50 : size;

            var logRepository = _unitOfWork.GetRepository<Log>();

            var basePackageQuery = logRepository.GetAll().Where(x => x.IsActive);

            var rowCount = await basePackageQuery.CountAsync();

            var packageList = await basePackageQuery
                .Skip((page - 1) * size)
                .Take(size)
                .ProjectTo<LogDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response.List = packageList;
            response.Size = size;
            response.Page = page;
            response.Count = rowCount;

            return response;
        }
        
    }
}
