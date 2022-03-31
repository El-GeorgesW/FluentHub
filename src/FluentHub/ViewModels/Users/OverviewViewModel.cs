﻿using FluentHub.Backend;
using FluentHub.Models;
using FluentHub.Octokit.Models;
using FluentHub.Octokit.Queries.Users;
using FluentHub.ViewModels.Repositories;
using FluentHub.ViewModels.UserControls.ButtonBlocks;
using Humanizer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FluentHub.ViewModels.Users
{
    public class OverviewViewModel : ObservableObject
    {
        #region constructor
        public OverviewViewModel(IMessenger messenger = null, ILogger logger = null)
        {
            _logger = logger;
            _messenger = messenger;
            _repositoryItems = new();
            RepositoryItems = new(_repositoryItems);

            RefreshRepositoryCommand = new AsyncRelayCommand<string>(RefreshRepositoryAsync);
        }
        #endregion

        #region fields
        private readonly ILogger _logger;
        private readonly IMessenger _messenger;
        private readonly ObservableCollection<RepoButtonBlockViewModel> _repositoryItems;
        private RepoContextViewModel _contextViewModel;
        #endregion

        #region properties
        public ReadOnlyObservableCollection<RepoButtonBlockViewModel> RepositoryItems { get; }
        public RepoContextViewModel ContextViewModel { get => _contextViewModel; set => SetProperty(ref _contextViewModel, value); }

        public IAsyncRelayCommand RefreshRepositoryCommand { get; }
        #endregion

        #region methods
        private async Task RefreshRepositoryAsync(string login)
        {
            try
            {
                PinnedItemQueries queries = new();
                List<Repository> items;

                items = login == null ?
                    await queries.GetAllAsync() :
                    await queries.GetAllAsync(login, true);

                if (items == null) return;

                _repositoryItems.Clear();
                foreach (var item in items)
                {
                    RepoButtonBlockViewModel viewModel = new()
                    {
                        Item = item,
                        DisplayDetails = false,
                        DisplayStarButton = false,
                    };

                    _repositoryItems.Add(viewModel);
                }

                RepoContextViewModel readmeRepoViewModel = new()
                {
                    Owner = login,
                    Name = login,
                };
                ContextViewModel = readmeRepoViewModel;
            }
            catch (Exception ex)
            {
                _logger?.Error("RefreshIssuesAsync", ex);
                if (_messenger != null)
                {
                    UserNotificationMessage notification = new("Something went wrong", ex.Message, UserNotificationType.Error);
                    _messenger.Send(notification);
                }
                throw;
            }
        }
        #endregion
    }
}
