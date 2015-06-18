using System;
using System.Collections.Generic;
using BoboBrowse.Api;
using BoboBrowse.Facets;
using BoboBrowse.Facets.impl;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Yomego.Umbraco.Umbraco.Services.Content.Facets
{
    public class FacetGenerator
    {
        string IndexPath;
        string LuceneQuery;
        IEnumerable<string> FieldsToFacetOn;
        int MinTermFrequency;
        string LuceneFieldQueried;
        string BrowseSelection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexPath">full path to lucene index</param>
        /// <param name="luceneQuery">query that was initially run needs to be examine format</param>
        /// <param name="fieldsToFacectOn">list of fields we want to generate facets on</param>
        /// <param name="minTermFrequency">minimum frequency ie want for than 2 facets per hit</param>
        /// <param name="luceneFieldQueried">can be any field in index need it to build query object</param>
        /// <param name="browseSelection">one of the fields in initial list need it to actually do a browse</param>
        public FacetGenerator(string indexPath, string luceneQuery, IEnumerable<string> fieldsToFacectOn, int minTermFrequency, string luceneFieldQueried, string browseSelection)
        {
            IndexPath = indexPath;
            LuceneQuery = luceneQuery;
            FieldsToFacetOn = fieldsToFacectOn;
            MinTermFrequency = minTermFrequency;
            LuceneFieldQueried = luceneFieldQueried;
            BrowseSelection = browseSelection;
        }


        public Dictionary<string, IEnumerable<BrowseFacet>> GenerateFacets()
        {
            var facets = new Dictionary<string, IEnumerable<BrowseFacet>>();

            Directory idx = null;
            IndexReader reader = null;
            BoboIndexReader boboReader = null;

            try
            {
                idx = FSDirectory.Open(new System.IO.DirectoryInfo(IndexPath));

                reader = IndexReader.Open(idx, true);

                var handlerList = GetFacetHandlers();

                boboReader = BoboIndexReader.GetInstance(reader, handlerList);

                var browseRequest = new BrowseRequest { Count = 10, Offset = 0, FetchStoredFields = false };

                // add a selection one of the fields in handler list
                var selection = new BrowseSelection(BrowseSelection);

                browseRequest.AddSelection(selection);

                browseRequest.Query = GetQuery(LuceneFieldQueried, GetLuceneQueryFromExamineQuery(LuceneQuery));

                var facetSpec = new FacetSpec { OrderBy = FacetSpec.FacetSortSpec.OrderHitsDesc };

                if (MinTermFrequency != 0)
                {
                    facetSpec.MinHitCount = MinTermFrequency;
                }

                foreach (var facetField in FieldsToFacetOn)
                {
                    facets.Add(facetField, GetHits(browseRequest, facetSpec, boboReader, facetField));
                }

            }
            finally
            {
                //cleanup
                if (idx != null)
                {
                    idx.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (boboReader != null)
                {
                    boboReader.Close();
                }
            }

            return facets;
        }

        #region private methods

        private IEnumerable<BrowseFacet> GetHits(BrowseRequest browseRequest, FacetSpec facetSpec, BoboIndexReader boboReader, string facetField)
        {
            browseRequest.SetFacetSpec(facetField, facetSpec);

            // perform browse
            using (var browser = new BoboBrowser(boboReader))
            {
                BrowseResult result = browser.Browse(browseRequest);

                Dictionary<String, IFacetAccessible> facetMap = result.FacetMap;

                IFacetAccessible facets = facetMap[facetField];

                return facets.GetFacets();
            }
        }

        private ICollection<FacetHandler> GetFacetHandlers()
        {
            ICollection<FacetHandler> handlerList = new List<FacetHandler>();

            foreach (var field in FieldsToFacetOn)
            {
                var handler = new MultiValueFacetHandler(field);
 
                handlerList.Add(handler);
            }

            return handlerList;
        }

        private string GetLuceneQueryFromExamineQuery(string examineQuery)
        {
            int position = examineQuery.IndexOf("LuceneQuery: ") + 13;
            int noOfCharsToGet = examineQuery.Length - 2 - position;
            string luceneQuery = examineQuery.Substring(position, noOfCharsToGet);
            return luceneQuery;
        }

        private Query GetQuery(string fieldName, string queryString)
        {
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldName, new WhitespaceAnalyzer());

            var q = parser.Parse(queryString);

            return q;
        }

        #endregion
    }
}
