﻿using FluentHub.Octokit.Models;
using Octokit.GraphQL;
using Octokit.GraphQL.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHub.Octokit.Queries.Repositories
{
    public class PullRequestQueries
    {
        public PullRequestQueries() => new App();

        public async Task<List<Models.PullRequest>> GetAllAsync(string name, string owner)
        {
            IssueOrder order = new() { Direction = OrderDirection.Desc, Field = IssueOrderField.CreatedAt };

            #region queries
            var query = new Query()
                .Repository(name, owner)
                .PullRequests(first: 30, orderBy: order)
                .Nodes
                .Select(x => new
                {
                    OwnerAvatarUrl = x.Repository.Owner.AvatarUrl(100),
                    OwnerLogin = x.Repository.Owner.Login,
                    x.Repository.Name,

                    x.Closed,
                    x.Merged,
                    x.IsDraft,

                    Labels = x.Labels(10, null, null, null, null).Nodes.Select(y => new
                    {
                        y.Color,
                        y.Name,
                    }).ToList(),

                    x.Number,
                    x.Title,
                    x.UpdatedAt,
                })
                .Compile();
            #endregion

            var response = await App.Connection.Run(query);

            #region copying
            List<Models.PullRequest> items = new();

            foreach (var res in response)
            {
                Models.PullRequest item = new();

                item.Labels = new();
                foreach (var label in res.Labels)
                {
                    Models.Label labels = new();
                    labels.Color = label.Color;
                    labels.Name = label.Name;

                    item.Labels.Add(labels);
                }

                item.IsClosed = res.Closed;
                item.IsMerged = res.Merged;
                item.IsDraft = res.IsDraft;

                item.Number = res.Number;
                item.Title = res.Title;
                item.UpdatedAt = res.UpdatedAt;
                item.Name = res.Name;
                item.OwnerLogin = res.OwnerLogin;
                item.OwnerAvatarUrl = res.OwnerAvatarUrl;

                items.Add(item);
            }
            #endregion

            return items;
        }

        public async Task<Models.PullRequest> GetAsync(string owner, string name, int number)
        {
            #region queries
            var query = new Query()
                .Repository(name, owner)
                .PullRequest(number)
                .Select(x => new
                {
                    OwnerAvatarUrl = x.Repository.Owner.AvatarUrl(100),
                    OwnerLogin = x.Repository.Owner.Login,
                    x.Repository.Name,

                    x.Closed,
                    x.Merged,
                    x.IsDraft,

                    Labels = x.Labels(10, null, null, null, null).Nodes.Select(y => new
                    {
                        y.Color,
                        y.Name,
                    }).ToList(),

                    x.Number,
                    x.Title,
                    x.UpdatedAt,
                })
                .Compile();
            #endregion

            var response = await App.Connection.Run(query);

            #region copying
            Models.PullRequest item = new();

            item.Labels = new();
            foreach (var label in response.Labels)
            {
                Models.Label labels = new();
                labels.Color = label.Color;
                labels.Name = label.Name;

                item.Labels.Add(labels);
            }

            item.IsClosed = response.Closed;
            item.IsMerged = response.Merged;
            item.IsDraft = response.IsDraft;

            item.Number = response.Number;
            item.Title = response.Title;
            item.UpdatedAt = response.UpdatedAt;
            item.Name = response.Name;
            item.OwnerLogin = response.OwnerLogin;
            item.OwnerAvatarUrl = response.OwnerAvatarUrl;
            #endregion

            return item;
        }
    }
}
