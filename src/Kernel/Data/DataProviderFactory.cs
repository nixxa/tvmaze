using System;
using Kernel.Interfaces;
using LiteDB;
using Microsoft.Extensions.Options;
using Models;

namespace Kernel.Data
{
    public class DataProviderFactory : IDataProviderFactory
    {
        private readonly DatabaseOptions _options;

        public DataProviderFactory(IOptions<DatabaseOptions> options)
        {
            if (options.Value == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            Configure();
        }

        public void Configure()
        {
            var mapper = BsonMapper.Global;
            mapper.Entity<TvShow>().Id(x => x.Id);
            mapper.Entity<TvShow>().DbRef(x => x.Casts, nameof(Person));

            mapper.Entity<Person>().Id(x => x.Id);
        }

        public LiteDatabase Create()
        {
            return new LiteDatabase(_options.Path);
        }
    }
}