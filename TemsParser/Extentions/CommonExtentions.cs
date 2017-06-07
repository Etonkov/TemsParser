using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using TemsParser.ViewModels;
using TemsParser.Models;
using System.Reflection;
using TemsParser.Models.Config;
using System.Linq.Expressions;
using TemsParser.CustomAttributes;
using System.Diagnostics;
using TemsParser.Models.Settings;

namespace TemsParser.Extentions
{
    public static class CommonExtentions
    {
        private static readonly Dictionary<LambdaExpressionKey, Func<object, object>> LambdaExpressions
            = new Dictionary<LambdaExpressionKey, Func<object, object>>();

        private static readonly Dictionary<Type, PropertyInfo[]> PropertyInfos = new Dictionary<Type, PropertyInfo[]>();

        private static Func<object, object> CreateGetter(object entity, PropertyInfo propertyInfo) 
        {
            //DynamicExpression.Lambda
            var param = Expression.Parameter(typeof(object), "e");
            Expression body = Expression.Property(Expression.TypeAs(param, entity.GetType()), propertyInfo);

            if (propertyInfo.PropertyType != typeof(object))
            {
                body = Expression.Convert(body, typeof(object));
            }

            var getterExpression = Expression.Lambda<Func<object, object>>(body, param);
            return getterExpression.Compile();
        }

        private struct LambdaExpressionKey
        {
            private Type _entityType;
            private string _propertyName;

            public LambdaExpressionKey(Type entityType, string propertyName)
            {
                _entityType = entityType;
                _propertyName = propertyName;
            }

            public Type EntityType
            {
                get { return _entityType; }
            }

            public string PropertyName
            {
                get { return _propertyName; }
            }
        }

        /// <summary>
        /// Gives a collections of the alements contained in the collection bypassing a tree-structure.
        /// Uses recursive search whith the cursor function.
        /// </summary>
        /// <typeparam name="T">A type of items which contained in collection.</typeparam>
        /// <param name="collection">The input collection of objects to traverse.</param>
        /// <param name="cursor">This is a function(pointer) to the child element.</param>
        /// <returns>Collection of all items bypassing a tree-structure.</returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> cursor)
        {
            var result = collection.ToList<T>();

            foreach (var itemCollection in collection)
            {
                var childrenCollection = cursor(itemCollection) as IEnumerable<T>;
                result.AddRange(childrenCollection.Traverse(cursor));
            }

            return result;
        }

        /// <summary>
        /// Removes all white space characters(space, line and paragraph separator) from input string.
        /// </summary>
        /// <param name="input">Parameter requires a string argument.</param>
        /// <returns>A string without white space.</returns>
        public static string RemoveWhiteSpace(this string input)
        {
            return new string(input.ToCharArray()
                                  .Where(c => !Char.IsWhiteSpace(c))
                                  .ToArray());
        }

        /// <summary>
        /// Searches for the specified object(value).
        /// Returns the index of the last occurrence within the entire collection.
        /// If not found returns -1.
        /// </summary>
        /// <param name="collection">The collection to search.</param>
        /// <param name="value">The object to locate in collection.</param>
        /// <returns>The index of the last occurrence of value within the entire collection, if found.
        /// Otherwise, -1. </returns>
        public static int FindIndex(this IEnumerable<dynamic> collection, dynamic value)
        {
            int result = -1;
            var list = collection.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (value == list[i])
                {
                    result = i;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a distinct collection of the objects with type of T.
        /// Uses recursive search in properties of object.
        /// Properties with type in ignoredTypes is ignored.
        /// If the property is marked by IgnoreGetAllChildrenAttribute it will be ignored.
        /// Properties of string objects is ignored.
        /// </summary>
        /// <typeparam name="T">The type of searched items.</typeparam>
        /// <param name="inputObject">The object in which to search.</param>
        /// <param name="ignoredTypes">The types which to ignore.</param>
        /// <returns>Collection of the objects with the type of T which contained in the input object.</returns>
        private static bool GetAllChildren<T>(this object inputObject, out IEnumerable<T> result)
        {
            var tempResult = new List<T>();

            // If the obj is type of T then add it to the result.
            if (inputObject is T)
            {
                tempResult.Add((T)inputObject);
            }

            // This is for ignoring indexed properties contained in the string.
            if (!(inputObject is String))
            {
                if (inputObject is IEnumerable<object>) /* In case obj is IEnumerable. */
                {

                    // Recursive search objects and add them to the result.
                    foreach (var childrenObjItem in (IEnumerable<object>)inputObject)
                    {
                        IEnumerable<T> collection;

                        if (childrenObjItem.GetAllChildren<T>(out collection) == true)
                        {
                            tempResult.AddRange(collection);
                        }
                    }
                }
                else /* Else recursive search properties type of T and add them to the result. */
                {

                    var inputObjectType = inputObject.GetType();

                    PropertyInfo[] inputObjectPropertyInfos;

                    if (PropertyInfos.TryGetValue(inputObjectType, out inputObjectPropertyInfos) == false)
                    {
                        inputObjectPropertyInfos = inputObject.GetType().GetProperties();

                        inputObjectPropertyInfos = inputObjectPropertyInfos
                            .Where(o => (o.GetCustomAttribute(typeof(IgnoreGetAllChildrenAttribute)) == null))
                            .ToArray();

                        PropertyInfos.Add(inputObjectType, inputObjectPropertyInfos);

                    }

                    foreach (var propInfo in inputObjectPropertyInfos)
                    {
                        try
                        {
                            Func<object, object> lambdaExpressionValue;

                            var lambdaExpressionKey = new LambdaExpressionKey(inputObjectType, propInfo.Name);

                            if (LambdaExpressions.TryGetValue(lambdaExpressionKey, out lambdaExpressionValue) == false)
                            {
                                lambdaExpressionValue = CreateGetter(inputObject, propInfo);
                                LambdaExpressions.Add(lambdaExpressionKey, lambdaExpressionValue);
                            }

                            var propValue = lambdaExpressionValue(inputObject);

                            if (propValue != null)
                            {
                                IEnumerable<T> collection;
                                
                                if (propValue.GetAllChildren<T>(out collection) == true)
                                {
                                    tempResult.AddRange(collection);
                                }
                            }
                        }
                        catch (TargetParameterCountException) /* Ignore all indexed properties. */
                        {
                            continue;
                        }
                    }
                }
            }

            if (tempResult.Count != 0)
            {
                result = tempResult.Distinct();
                return true;
            }
            else
            {
                result = new List<T>();
                return false;
            }
        }

        /// <summary>
        /// Returns a distinct collection of the objects with type of T.
        /// Uses recursive search in properties of object.
        /// If the property is marked by IgnoreGetAllChildrenAttribute it will be ignored.
        /// Properties of string objects is ignored.
        /// </summary>
        /// <typeparam name="T">The type of searched items.</typeparam>
        /// <param name="inputObject">The object in which to search.</param>
        /// <returns>Collection of the objects with the type of T which contained in the input object.</returns>
        public static IEnumerable<T> GetAllChildren<T>(this object inputObject)
        {
            IEnumerable<T> returns;
            GetAllChildren(inputObject, out returns);
            return returns;
        }

        public static IEnumerable<FilePathViewModel> ToFilePathViewModelCollection(this IEnumerable<string> inputObject)
        {
            var returns = new List<FilePathViewModel>();

            foreach (var filePath in inputObject)
            {
                returns.Add(new FilePathViewModel(filePath));
            }

            return returns;
        }
    }
}