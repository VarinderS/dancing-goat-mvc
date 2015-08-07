﻿using System;

using CMS.DataEngine;
using CMS.DocumentEngine;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Represents a contract for objects that provide information about pages and info objects using their runtime type.
    /// The objects are thread-safe and class names and object types are normalized, i.e. they are converted to lowercase.
    /// </summary>
    public interface IContentItemMetadataProvider
    {
        /// <summary>
        /// Returns a class name of a page.
        /// </summary>
        /// <param name="type">The runtime type that represents pages, i.e. it is derived from the <see cref="CMS.DocumentEngine.TreeNode"/> class.</param>
        /// <returns>The lowercase class name of a page.</returns>
        string GetClassNameFromPageRuntimeType(Type type);


        /// <summary>
        /// Returns a class name of a page.
        /// </summary>
        /// <typeparam name="T">The runtime type that represents pages, i.e. it is derived from the <see cref="CMS.DocumentEngine.TreeNode"/> class.</typeparam>
        /// <returns>The lowercase class name of a page.</returns>
        string GetClassNameFromPageRuntimeType<T>() where T : TreeNode, new();


        /// <summary>
        /// Returns an object type of an info object.
        /// </summary>
        /// <param name="type">The runtime type that represents info objects, i.e. it is derived from the <see cref="CMS.DataEngine.AbstractInfo`1"/> class.</param>
        /// <returns>The lowercase object type of an info object.</returns>
        string GetObjectTypeFromInfoObjectRuntimeType(Type type);


        /// <summary>
        /// Returns an object type of an info object.
        /// </summary>
        /// <typeparam name="T">The runtime type that represents info objects, i.e. it is derived from the <see cref="CMS.DataEngine.AbstractInfo`1"/> class.</typeparam>
        /// <returns>The lowercase object type of an info object.</returns>
        string GetObjectTypeFromInfoObjectRuntimeType<T>() where T : AbstractInfo<T>, new();
    }
}