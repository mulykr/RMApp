using System.Collections.Generic;
using System.Configuration;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MVCApp.Models
{
    public class GallerySearch
    {
        private readonly SearchIndexClient _searchIndex;

        public GallerySearch()
        {
            _searchIndex = new SearchIndexClient(ConfigurationManager.AppSettings["searchServiceName"], ConfigurationManager.AppSettings["indexName"], new SearchCredentials(ConfigurationManager.AppSettings["apiKey"]));
        }

        public IEnumerable<GalleryItem> Search(string term)
        {
            var res = _searchIndex.Documents.Search<GalleryItem>(term, new SearchParameters()).Results;
            var docs = new List<GalleryItem>();
            foreach (var item in res)
            {
                docs.Add(item.Document);
            }

            return docs;
        }
    }
}