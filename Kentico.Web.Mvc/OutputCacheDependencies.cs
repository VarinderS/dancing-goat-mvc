using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.DocumentEngine;

namespace Kentico.Web.Mvc
{
    public sealed class OutputCacheDependencies : IOutputCacheDependencies
    {
        public void Add<T>(IEnumerable<T> items) where T : TreeNode, new()
        {
            TreeNode item = items.FirstOrDefault();
            if (item == null)
            {
                item = new T();
            }

            var cacheKey = String.Format("nodes|testmvcdemo|{0}|all", item.ClassName.ToLowerInvariant());
            HttpContext.Current.Response.AddCacheItemDependency(cacheKey);
        }


        public void Add<T>(T item) where T : TreeNode, new()
        {
            var cacheKey = String.Format("nodes|testmvcdemo|{0}|all", item.ClassName.ToLowerInvariant());
            HttpContext.Current.Response.AddCacheItemDependency(cacheKey);
        }
    }
}