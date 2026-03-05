using eHarfaApp.Shared.DAL;
using eHarfaApp.Shared.Pages;
using MudBlazor;

namespace eHarfaApp.Shared.Services;

public class SongService : ISongService
{
    public Task<List<Song>> GetSongsAsync()
    {
        List<Song> songs = new List<Song>();

        for (var i = 1; i < 100; i++)
        {
            songs.Add(new Song()
            {
                Id = i.ToString(),
                Scale = "Do minor",
                Title = $"Cantare nr {i}",
                Content = $"Cantare continut nr {i}",
                CategoryId = $"{i%16}"
            });
        }
        
        return Task.FromResult(songs);
    }

    public Task<List<SongCategory>> GetCategoriesAsync()
    {
        List<SongCategory> categories = new List<SongCategory>
        {
            new(id: "1", title: "Cântări despre anul nou și trecerea timpului", 
                icon: Icons.Material.Filled.Event),
            new(id: "2", title: "Cântări despre binecuvântarea copiilor și despre părinți",
                icon: Icons.Material.Filled.ChildCare),
            new(id: "3", title: "Cântări despre botezul în apă și venirea la pocăință",
                icon:Icons.Material.Filled.Water),
            new(id: "4", title: "Cântări despre căsătorie și dragoste",
                icon:Icons.Material.Filled.Favorite),
            new(id: "5", title: "Cântări despre Cina cea de taină și suferințele Domnului Isus",
                icon:Icons.Material.Filled.Bloodtype),
            new(id: "6", title: "Cântări despre Duhul Sfânt",
                icon:Icons.Material.Filled.FlashOn),
            new(id: "7", title: "Cântări pentru evanghelizare",
                icon:Icons.Material.Filled.Campaign),
            new(id: "8", title: "Cântări pentru mângâiere și îmbărbătare",
                icon:Icons.Material.Filled.VolunteerActivism),
            new(id: "9", title: "Cântări despre îndemn la veghere și pocăință",
                icon:Icons.Material.Filled.Lightbulb),
            new(id: "10", title: "Cântări despre înmormântare",
                icon:Icons.Material.Filled.HeartBroken),
            new(id: "11", title: "Cântări despre învierea și înălțarea Domnului",
                icon:Icons.Material.Filled.ArrowUpward),
            new(id: "12", title: "Cântări de laudă, mulțumire și bucurie",
                icon:Icons.Material.Filled.SentimentVerySatisfied),
            new(id: "13", title: "Cântări despre nașterea Domnului",
                icon:Icons.Material.Filled.StarPurple500),
            new(id: "14", title: "Cântări despre predarea în slujba lui Dumnezeu",
                icon:Icons.Material.Filled.Handshake),
            new(id: "15", title: "Cântări despre revenirea Domnului și Patria cerească",
                icon:Icons.Material.Filled.CloudSync),
            new(id: "16", title: "Cântări pentru timpul de rugăciune",
                icon:Icons.Material.Filled.SelfImprovement),
        };
        return Task.FromResult(categories);
    }

    public async Task<Song> GetSongByIdAsync(string id)
    {
        var category = await GetCategoriesAsync();
        var song = new Song()
        {
            Id = id,
            Title = "Din Beer-Șeba, Iacov, când pornise în călătoria lui înspre Haran",
            Category = category[1],
            CategoryId = "1",
            Content = "1. Din Beer-Șeba, Iacov, când pornise în călătoria lui înspre Haran,\\nDupă cum Isaac îi poruncise să rămână la Padan-Aram,\\nDupă-o zi de grea călătorie, seara se lăsase și el se opri,\\nCăci lumina soarelui, cea vie, coborâtu-se spre asfințit.\\n\\nR1: Iacov își luase cu drag o piatră\\nR: Și și-o puse-astfel sub capul lui.\\nR: „O, Doamne, ce noapte minunată,\\nR: Să dormi în mijlocul pustiului!”\\n\\n2. Dar Domnul, iată, că îi arată, îl cuprinde mreaja unui vis stingher.\\nEl vede o scară minunată, de pe pământ ce ajungea la cer.\\nDar, pe-această scară ne-nsemnată, se plimbau îngerii și în sus, și-n jos,\\nÎngerii Dumnezeului cel mare. - Iată, dragii mei, ce vis frumos! -\\n\\nR2: Dar Domnul sta privind deasupra scării\\nR: Și lui Iacov îi cuvânta:\\nR: „Pământul și ce are suflare,\\nR: Toate seminției tale îi voi da.\\n\\n3. Te vei întinde la-apus și la răsărit, la miazănoapte și până la miazăzi,\\nȘi toate semințiile pământului, toate ție ți le voi dărui.”\\nȘi, sculându-se de dimineață, a luat piatra și-a fixat-o în pământ,\\nCa loc de aducere aminte pentru Dumnezeu-Acela sfânt.\\n\\nR3: Plin de frică, Iacov, atunci își zise:\\nR: „Acest loc e-așa de minunat,\\nR: Căci aici e Casa lui Dumnezeu\\nR: Și poarta cerului înstelat!”\\n\\n4. Iacov a făcut o juruință, căci dacă Domnu-n viață îl va ocroti,\\nPământul și ce are suflare, lui Dumnezeu Îi va dărui.\"",
            Scale = "Mi major"
        };
        
        return await Task.FromResult(song);
    }
}