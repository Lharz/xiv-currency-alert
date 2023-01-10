using System;
using System.Collections.Generic;
using System.Threading;
using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using KamiLib.ChatCommands;

namespace CurrencyAlert.System.cs;

public class CurrencyTracker : IDisposable
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    
    public readonly List<TrackedCurrency> ActiveWarnings = new();
    
    public CurrencyTracker()
    {
        UpdateCurrencies();

        Service.ClientState.TerritoryChanged += OnZoneChange;
    }

    private void UpdateCurrencies()
    {
        Service.Framework.RunOnTick(UpdateCurrencies, TimeSpan.FromSeconds(1), 0, cancellationTokenSource.Token);
        
        ActiveWarnings.Clear();
        if (!Service.ClientState.IsLoggedIn) return;
        
        foreach (var currency in Service.Configuration.TrackedCurrencies)
        {
            if (currency.Enabled)
            {
                if (currency.CurrencyInfo.GetCurrentQuantity() <= currency.Threshold.Value)
                {
                    ActiveWarnings.Add(currency);
                }
            }
        }
    }

    private void OnZoneChange(object? sender, ushort e)
    {
        if (!Service.Configuration.ChatNotification) return;

        foreach (var currency in ActiveWarnings)
        {
            Chat.Print($"{currency.CurrencyInfo.ItemName}", Strings.ChatWarningText);
        }
    }
    
    public void Dispose()
    {
        Service.ClientState.TerritoryChanged -= OnZoneChange;
        
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }
}