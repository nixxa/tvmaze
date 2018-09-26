using System;
using Core.Interfaces;
using LiteDB;
using Microsoft.Extensions.Options;

namespace Core.Data
{
    public class DataProviderFactory : IDataProviderFactory
    {
        private readonly DatabaseOptions _options;

        public DataProviderFactory(IOptions<DatabaseOptions> options)
        {
            if (options.Value == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
        }

        public LiteDatabase Create()
        {
            return new LiteDatabase(_options.Path);
        }
    }
}