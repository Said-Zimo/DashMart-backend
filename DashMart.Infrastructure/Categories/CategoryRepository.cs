

using DashMart.Domain.Categories;
using DashMart.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Categories
{
    public sealed class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)=>  _context.Categories.Add(category);

        public void Delete(Category category) => _context.Categories.Remove(category);
        

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Categories.FirstOrDefaultAsync(x=> x.Id == id, cancellationToken);

        public async Task<Category?> GetByPublicIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Categories.FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);


        public async Task<int> GetInternalIdByPublicIdAsync(Guid PublicId, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.PublicId == PublicId, cancellationToken);

            if (category == null) 
                return -1; 
            else
                return category.Id;
        }

        public async Task<bool> IsExistAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Categories.AnyAsync(x=> x.CategoryName == name, cancellationToken);

    }
}
