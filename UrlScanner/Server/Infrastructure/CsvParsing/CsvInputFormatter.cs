using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using UrlScanner.Server.Infrastructure.Extensions;
using static System.Convert;
using static System.StringSplitOptions;

namespace UrlScanner.Server.Infrastructure.CsvParsing
{
    internal sealed class CsvInputFormatter : InputFormatter
    {
        public CsvInputFormatter() => SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        
        protected override bool CanReadType(Type type)
        {
            return typeof(IList).IsAssignableFrom(type) && type.IsGenericType && type.IsClass;
        }
        
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var results = (IList)Activator.CreateInstance(context.ModelType);
            var itemType = results.GetType().GetTypeInfo().GenericTypeArguments[0];
            var properties = itemType.GetProperties()
                .Where(p => !p.GetCustomAttributes<CsvIgnoreAttribute>().Any())
                .ToList();
            
            var headerHasBeenSkipped = false;
            string line;

            using var reader = new StreamReader(context.HttpContext.Request.Body);
            while((line = await reader.ReadLineAsync()) != null)
            {
                if (headerHasBeenSkipped)
                {
                    results.Add(CreateItemFrom(line, itemType, properties));
                } 
                else 
                {
                    headerHasBeenSkipped = true;
                }
            }
            
            return await InputFormatterResult.SuccessAsync(results);
        }

        private static object CreateItemFrom(string line, Type itemType, IList<PropertyInfo> properties)
        {
            var item = Activator.CreateInstance(itemType);
            line.Split(",", TrimEntries).ForEach((value, index) =>
            {
                properties[index].SetValue(item, ChangeType(value, properties[index].PropertyType));
            });

            return item;
        }
    }
}