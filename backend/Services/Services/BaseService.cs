using AutoMapper;
using DAL;

namespace Services.Services;

public class BaseService
{
    protected readonly IMapper _mapper;
    protected readonly MyDbContext _dbContext;

    public BaseService(IMapper mapper, MyDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
}