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
        private DispatcherTimer _dispatcherTimer = null;
        private string _networkError = null;
        private string _filterError = null;
        private bool _isRefreshing = false;
        private string _filter = string.Empty;
        private Regex _filterRegex = new Regex(DefaultRegexPattern);

        /// <summary>
        /// Creates a new instace of the ticker view model.
        /// </summary>
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

        /// <summary>
        /// The IPropertyChanged event signaling a change of a model has ocurred so UI can sync.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Command used to refresh the current ticker data.
        /// </summary>
        public RelayCommand RefreshCommand { get; set; }

        /// <summary>
        /// The command to invoke once UI is ready to initlaize the view model and load the first batch of data.
        /// </summary>
        public RelayCommand InitializeCommand { get; set; }

        /// <summary>
        /// The serialized timestamp of the most recent succesful request.
        /// </summary>
        public string Timestamp
        {
            get
            {
                return DateTime.Now.ToString();
            }
        }

        /// <summary>
        /// The currently set RegEx filter for filtering the ticker items.
        /// </summary>
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

        /// <summary>
        /// Determines whether a network request is in progress.
        /// </summary>
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

        /// <summary>
        /// Determines whether the auto-refresh timer is currently enabled.
        /// </summary>
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

        /// <summary>
        /// The entire ticker data. This is unfiltered.
        /// </summary>
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

        /// <summary>
        /// The collection view the UI should bind to to display filtered items of the ticker.
        /// </summary>
        public ICollectionView FilteredTickerItems
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(this.TickerItems);
                source.Filter = p => this.FilterItem(p);
                return source;
            }
        }

        /// <summary>
        /// The last ocurred network error message. It is only set for a failing request.
        /// </summary>
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

        /// <summary>
        /// The current error of a filtering expression.
        /// </summary>
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

        /// <summary>
        /// The current error of the view model. This displays the error of the highest priority.
        /// </summary>
        public string Error
        {
            get
            {
                return this._filterError ?? this._networkError;
            }
        }

        /// <summary>
        /// Predicate for filtering the ticker items based on the currently set RegEx filter.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterItem(object obj)
        {
            var item = (TickerItem)obj;
            return this._filterRegex.Match(item.CurrencyPair).Success;
        }

        /// <summary>
        /// Initializes the view model and loads the initial ticker data.
        /// </summary>
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
