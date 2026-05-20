using System.Text.Json;
using Microsoft.JSInterop;

namespace EbayTemplateGenerator.Services;

public interface ILocalStorageService
{
    Task<T?> GetItemAsync<T>(string key);
    Task SetItemAsync<T>(string key, T value);
}

public class LocalStorageService(IJSRuntime js) : ILocalStorageService
{
    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json, Helpers.JsonOptions);
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, Helpers.JsonOptions);
        await js.InvokeVoidAsync("localStorage.setItem", key, json);
    }
}
