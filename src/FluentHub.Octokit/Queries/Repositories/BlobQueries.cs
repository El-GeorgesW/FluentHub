﻿using Octokit.GraphQL;
using Octokit.GraphQL.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHub.Octokit.Queries.Repositories
{
    public class BlobQueries
    {
        public BlobQueries() => new App();

        public async Task<(string, long)> GetAsync(string name, string owner, string branch, string path)
        {
            // Remove slash
            path = path.Remove(0, 1);

            var queryToGetFileInfo = new Query()
                .Repository(name, owner)
                .Object(expression: branch + ":" + path)
                .Cast<Blob>().Select(x => new
                {
                    x.Text,
                    x.ByteSize,
                })
                .Compile();

            var response = await App.Connection.Run(queryToGetFileInfo);

            return (response.Text, response.ByteSize);
        }
    }
}
