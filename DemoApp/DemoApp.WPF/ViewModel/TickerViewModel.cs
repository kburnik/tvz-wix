/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using DemoApp.Lib;
using DemoApp.Lib.Model;
using DemoApp.WPF.View.ViewModel;

namespace DemoApp.WPF.ViewModel
{
    public class TickerViewModel : INotifyPropertyChanged
    {
        private PoloniexClient _poloniexClient = null;
        private ObservableCollection<TickerItem> _tickerItems = null;
        private string _error = null;
        private bool _isRefreshing = false;
        private DispatcherTimer _dispatcherTimer = null;

        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand InitializeCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Timestamp
        {
            get
            {
                return DateTime.Now.ToString();
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
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
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
                this._dispatcherTimer.IsEnabled = true;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTimerEnabled)));
            }
        }

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

        public ObservableCollection<TickerItem> TickerItems
        {
            get
            {
                return this._tickerItems;
            }

            set
            {
                this._tickerItems = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TickerItems)));
            }
        }

        public string Error
        {
            get
            {
                return this._error;
            }

            set
            {
                this._error = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }
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
                this.Error = null;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Timestamp)));
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }
    }
}
