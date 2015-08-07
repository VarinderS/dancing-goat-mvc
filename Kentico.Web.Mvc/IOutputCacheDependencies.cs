using System.Collections.Generic;

using CMS.DocumentEngine;

namespace Kentico.Web.Mvc
{
    public interface IOutputCacheDependencies
    {
        void Add<T>(IEnumerable<T> items) where T : TreeNode, new();
        void Add<T>(T item) where T : TreeNode, new();
    }
}