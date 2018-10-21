using MvcSiteMapProvider;
using SpodIglyMVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Infrastructure
{
    // aby nam działało dynamiczne generowanie stron (niestatycznych takich jak np kazdy gatunek)
    // musimy stworzyc klase odpowiedzialna za generowanie takich wpisow w pliku mvc.sitemap
    public class ProductDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext(); 

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>(); 

            foreach (var item in db.Albums.ToList())
            {
                DynamicNode dynamicNode = new DynamicNode()
                {
                    Title = item.AlbumTitle,
                    Key = "Album_" + item.AlbumId,
                    ParentKey = "Genre_" + item.GenreId
                };
                dynamicNode.RouteValues.Add("id", item.AlbumId);
                returnValue.Add(dynamicNode);
            }

            return returnValue;
        }
    }

    public class ProductListDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();

            foreach (var item in db.Genres.ToList())
            {
                DynamicNode dynamicNode = new DynamicNode()
                {
                    Title = item.Name,
                    Key = "Genre_" + item.GenreId
                };
                dynamicNode.RouteValues.Add("genrename", item.Name);
                returnValue.Add(dynamicNode);
            }

            return returnValue;
        }
    }
}