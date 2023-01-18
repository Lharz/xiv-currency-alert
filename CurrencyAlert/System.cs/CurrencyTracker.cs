using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using Dalamud.Logging;
using KamiLib.ChatCommands;
using KamiLib.GameState;

namespace CurrencyAlert.System.cs;

public class CurrencyTracker : IDisposable
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly Stopwatch timer = new();
    
    public readonly List<TrackedCurrency> ActiveWarnings = new();
    
    public CurrencyTracker()
    {
        UpdateCurrencies();

        Service.ClientState.TerritoryChanged += OnZoneChange;
    }

    private void UpdateCurrencies()
    {
        Service.Framework.RunOnTick(UpdateCurrencies, TimeSpan.FromMilliseconds(250), 0, cancellationTokenSource.Token);
        
        ActiveWarnings.Clear();
        if (!Service.ClientState.IsLoggedIn) return;
        
        foreach (var currency in Service.Configuration.TrackedCurrencies)
        {
            if (currency.Enabled)
            {
                if (currency.CurrencyInfo().GetCurrentQuantity() >= currency.Threshold.Value)
                {
                    ActiveWarnings.Add(currency);
                }
            }
        }
    }

    private void OnZoneChange(object? sender, ushort e)
    {
        if (!Service.Configuration.ChatNotification) return;
        if (Condition.IsBoundByDuty()) return;
        
        if (timer.Elapsed.Minutes >= 5 || timer.IsRunning == false)
        {
            timer.Restart();
            foreach (var currency in ActiveWarnings)
            {
                Chat.Print($"{currency.CurrencyInfo().ItemName}", Strings.ChatWarningText);
            }
        }
        else
        {
            var lockoutRemaining = TimeSpan.FromMinutes(5) - timer.Elapsed;
            PluginLog.Debug($"Zone Change Messages Suppressed, '{lockoutRemaining}' Remaining");
        }
    }
    
    public void Dispose()
    {
        Service.ClientState.TerritoryChanged -= OnZoneChange;
        
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }
}
