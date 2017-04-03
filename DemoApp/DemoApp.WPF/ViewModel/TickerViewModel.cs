/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
namespace DemoApp.WPF.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows.Data;
    using System.Windows.Threading;
    using DemoApp.Lib;
    using DemoApp.Lib.Model;
    using DemoApp.WPF.View.ViewModel;

    /// <summary>
    /// The main view model for the ticker logic and UI interaction.
    /// </summary>
    public class TickerViewModel : INotifyPropertyChanged
    {
        private static readonly string DefaultRegexPattern = ".*";

        private PoloniexClient _poloniexClient = null;
        private ObservableCollection<TickerItem> _tickerItems = null;
        private string _networkError = null;
        private string _filterError = null;
        private bool _isRefreshing = false;
        private DispatcherTimer _dispatcherTimer = null;
        private string _filter = string.Empty;
        private Regex _filterRegex = new Regex(DefaultRegexPattern);

        public TickerViewModel()
        {
            this._poloniexClient = new PoloniexClient();
            this._tickerItems = new ObservableCollection<TickerItem>();
            this.RefreshCommand = new RelayCommand((obj) => this.Refresh());
            this.InitializeCommand = new RelayCommand((obj) => this.Initialize());
            this._dispatcherTimer = new DispatcherTimer();
            this._dispatcherTimer.Tick += new EventHandler((sender, args) => this.Refresh());
            this._dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            this._dispatcherTimer.IsEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand InitializeCommand { get; set; }

        public string Timestamp
        {
            get
            {
                return DateTime.Now.ToString();
            }
        }

        public string Filter
        {
            get
            {
                return this._filter;
            }

            set
            {
                if (value == this._filter)
                {
                    return;
                }

                this._filter = value;
                try
                {
                    this._filterRegex = new Regex(this._filter, RegexOptions.IgnoreCase);
                    this.FilterError = null;
                    this.RaisePropertyChanged(nameof(this.Filter));
                    this.RaisePropertyChanged(nameof(this.FilteredTickerItems));
                }
                catch (Exception ex)
                {
                    this.FilterError = ex.Message;
                }
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return this._isRefreshing;
            }

            set
            {
                this._isRefreshing = value;
                this.RaisePropertyChanged(nameof(this.IsRefreshing));
            }
        }

        public bool IsTimerEnabled
        {
            get
            {
                return this._dispatcherTimer.IsEnabled;
            }

            set
            {
                this._dispatcherTimer.IsEnabled = value;
                this.RaisePropertyChanged(nameof(this.IsTimerEnabled));
            }
        }


        public ObservableCollection<TickerItem> TickerItems
        {
            get
            {
                return this._tickerItems;
            }

            set
            {
                this._tickerItems = value;
                this.RaisePropertyChanged(nameof(this.TickerItems));
                this.RaisePropertyChanged(nameof(this.FilteredTickerItems));
            }
        }

        public ICollectionView FilteredTickerItems
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(this.TickerItems);
                source.Filter = p => this.FilterItem(p);
                return source;
            }
        }

        public string NetworkError
        {
            get
            {
                return this._networkError;
            }

            set
            {
                this._networkError = value;
                this.RaisePropertyChanged(nameof(this.NetworkError));
                this.RaisePropertyChanged(nameof(this.Error));
            }
        }

        public string FilterError
        {
            get
            {
                return this._filterError;
            }

            set
            {
                this._filterError = value;
                this.RaisePropertyChanged(nameof(this.FilterError));
                this.RaisePropertyChanged(nameof(this.Error));
            }
        }

        public string Error
        {
            get
            {
                return this._filterError ?? this._networkError;
            }
        }

        private bool FilterItem(object obj)
        {
            var item = (TickerItem)obj;
            return this._filterRegex.Match(item.CurrencyPair).Success;
        }

        private void Initialize()
        {
            this.IsTimerEnabled = true;
            this.Refresh();
        }

        /// <summary>
        /// Reloads the ticker.
        /// </summary>
        private async void Refresh()
        {
            if (this.IsRefreshing)
            {
                return;
            }

            try
            {
                this.IsRefreshing = true;
                var tickerSnapshot = await this._poloniexClient.GetTickerAsync();
                this.TickerItems.Clear();
                foreach (var item in tickerSnapshot.Items)
                {
                    this.TickerItems.Add(item);
                }

                this.NetworkError = null;
                this.RaisePropertyChanged(nameof(this.Timestamp));
            }
            catch (Exception ex)
            {
                this.NetworkError = ex.Message;
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        /// <summary>
        /// Shorthand method for raising the property changed event.
        /// </summary>
        /// <param name="name"></param>
        private void RaisePropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
