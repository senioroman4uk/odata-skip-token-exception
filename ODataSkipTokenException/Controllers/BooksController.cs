using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataSkipTokenException.Model;
using static Microsoft.AspNetCore.OData.Query.AllowedLogicalOperators;
using static Microsoft.AspNetCore.OData.Query.AllowedQueryOptions;

namespace ODataSkipTokenException.Controllers;

public class BooksController : ODataController
{
    private static readonly IReadOnlyCollection<Review> Reviews = Enumerable.Range(0, 101).Select(i => new Review
    {
        Id = i.ToString(),
        Author = new Author
        {
            Name = i.ToString(),
            Me = i == 0,
        },
    }).ToArray();

    [EnableQuery(
        PageSize = 100,
        EnsureStableOrdering = false,
        AllowedFunctions = AllowedFunctions.Length,
        AllowedLogicalOperators = Equal | And | Or | GreaterThan | LessThan,
        AllowedQueryOptions = SkipToken | Filter | OrderBy | Top | Select | Count | Expand,
        MaxExpansionDepth = 1)
    ]
    public async Task<IActionResult> GetReviewsFromBookAsync(
        [FromRoute] string key,
        ODataQueryOptions<Review> query,
        CancellationToken cancellationToken)
    {
        return Ok(Reviews);
    }
}