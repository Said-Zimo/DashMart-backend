

using DashMart.Application.Categories.Mapper;
using DashMart.Application.Categories.Query;
using DashMart.Application.Categories.Query.Interface;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Categories
{
    public sealed class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<GetAllCategoryQueryResponse>> GetAllCategoriesAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        => CategoryMapper.ToGetAllQueryResponse(await _context.Categories
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken));

        public async Task<GetCategoryByIdQueryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x=> x.PublicId == id, cancellationToken);

            if (response == null) return null;

            return new GetCategoryByIdQueryResponse(response.CategoryName);
        }
    }
}
