﻿using Octokit.GraphQL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHub.Octokit.Queries.Users
{
    public class FollowingQueries
    {
        public FollowingQueries() => new App();

        public async Task<List<Models.User>> GetAllAsync(string login)
        {
            #region query
            var query = new Query()
                    .User(login)
                    .Following(first: 30)
                    .Nodes
                    .Select(x => new
                    {
                        AvatarUrl = x.AvatarUrl(100),
                        x.Name,
                        x.Bio,
                        x.Login,
                        x.Id,
                    })
                    .Compile();
            #endregion

            var result = await App.Connection.Run(query);

            #region copying
            List<Models.User> formattedFollowingList = new();

            foreach (var res in result)
            {
                Models.User item = new();

                item.AvatarUrl = res.AvatarUrl;
                item.Name = res.Name;
                item.Login = res.Login;
                item.Bio = res.Bio;

                // If user is organization, id starts with "O_"
                if (res.Id.ToString()[0] == 'O' && res.Id.ToString()[1] == '_')
                {
                    item.IsOrganization = true;
                }

                formattedFollowingList.Add(item);
            }
            #endregion

            return formattedFollowingList;
        }

        public async Task<List<Models.User>> GetAllAsync()
        {
            #region query
            var query = new Query()
                    .Viewer
                    .Following(first: 30)
                    .Nodes
                    .Select(x => new
                    {
                        AvatarUrl = x.AvatarUrl(100),
                        x.Name,
                        x.Bio,
                        x.Login,
                        x.Id,
                    })
                    .Compile();
            #endregion

            var result = await App.Connection.Run(query);

            #region copying
            List<Models.User> formattedFollowingList = new();

            foreach (var res in result)
            {
                Models.User item = new();

                item.AvatarUrl = res.AvatarUrl;
                item.Name = res.Name;
                item.Login = res.Login;
                item.Bio = res.Bio;

                // If user is organization, id starts with "O_"
                if (res.Id.ToString()[0] == 'O' && res.Id.ToString()[1] == '_')
                {
                    item.IsOrganization = true;
                }

                formattedFollowingList.Add(item);
            }
            #endregion

            return formattedFollowingList;
        }
    }
}
