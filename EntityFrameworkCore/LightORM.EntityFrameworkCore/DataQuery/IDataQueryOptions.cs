using LightORM.EntityFrameworkCore.DataQuery.Filters;
using LightORM.EntityFrameworkCore.DataQuery.Pages;
using LightORM.EntityFrameworkCore.DataQuery.Sorts;

namespace LightORM.EntityFrameworkCore.DataQuery
{
    public interface IDataQueryOptions
    {
        IPageOptions? Page { get; }

        IMultiSortOptions? Sorts { get; }

        IMultiFilterOptions? Filters { get; }
    }
}
