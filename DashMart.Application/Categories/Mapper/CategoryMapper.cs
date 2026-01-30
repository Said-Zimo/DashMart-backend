

using DashMart.Application.Categories.Query;
using DashMart.Domain.Categories;

namespace DashMart.Application.Categories.Mapper
{
    public static class CategoryMapper
    {

        public static IReadOnlyList<GetAllCategoryQueryResponse> ToGetAllQueryResponse(IList<Category> categories)
        {
            var list = new List<GetAllCategoryQueryResponse>();

            foreach (var category in categories)
            {
                list.Add(new GetAllCategoryQueryResponse(category.CategoryName));
            }

            return list;
        }

    }
}
