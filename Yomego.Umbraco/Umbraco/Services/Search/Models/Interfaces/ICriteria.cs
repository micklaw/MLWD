using System;
using System.Collections.Generic;
using Yomego.Umbraco.Umbraco.Services.Search.Enums;

namespace Yomego.Umbraco.Umbraco.Services.Search.Models.Interfaces
{
    public interface ICriteria
    {
        List<Tuple<List<string>, List<string>, OperatorEnum>> SearchItems { get; set; }

        IList<Type> TypesToSearch { get; set; }

        List<string> TypesAsStringToSearch { get; set; }

        string CustomOrder { get; set; }

        string FacetField { get; set; }

        SearchOrder Order { get; set; }

        int PageSize { get; set; }

        int Page { get; set; }

        bool? OrderAscending { get; }
    }
}
