using eHarfaApp.Shared.DAL;
using MudBlazor;

namespace eHarfaApp.Shared.Services;

internal static class SeedData
{
    public static List<Song> CreateSongs()
    {
        List<Song> songs = [];

        songs.Add(new Song
        {
            Id = "1",
            Title = "A mai trecut un an de zile din viața ta, din viața mea",
            Scale = "do minor",
            Content = "1. A mai trecut un an de zile din viața ta, din viața mea,\nAșa de repede trecut-au anii, toate de noi vor rămânea.\nA mai trecut un an de zile, și poate vor mai trece doi,\nDar cine ar putea să spună ce se va întâmpla de-acum cu noi?\n\nR: Așa s-au dus anii de-a rândul; prin multe, Doamne, ne-ai trecut.\nAm fost slabi, Doamne, în credință, aș vrea s-o iau de la-nceput!\nCăci nu știm ziua-n care, Doamne, pe nori, în slavă vei veni,\nCa să-Ți răpești a Ta Mireasă, s-o duci cu Tine-n veșnicii.\n\n2. A mai trecut un an de zile și nu știm câți vor mai trece,\nDar, după toate ce se-ntâmplă-n lume, curând va fi venirea Ta!\nTrezește-ne din amorțeală, parcă cu toți am adormit.\nVenirea Ta, Doamne, este aproape și noi nu suntem pregătiți.\n\n3. Se-ntâmplă, Doamne, atâtea-n lume, lumea parcă s-a-ntors pe dos.\nNu vor să mai audă Cuvântul, s-au lepădat de Isus Hristos.\nLumea parcă este un haos, păcatul s-a dezlănțuit;\nI-a prins în mreje pe aceia ce-au încheiat un legământ.",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "2",
            Title = "Tinerețe, tinerețe, câte imnuri ți-aș cânta",
            Scale = "Sol major",
            Content = "1. Tinerețe, tinerețe, câte imnuri ți-aș cânta\nDacă pân’ la bătrânețe ai fi tot mereu așa!\nNumai zel și dărnicie, flacără de tainic dor\nCântec, joc și bucurie, numai soare fără nor!\n\nR: Tinerețe, sfinte imnuri ți-aș cânta!\nTinerețe! Dac-ai fi mereu așa!\nTinerețe! Dorul marilor iubiri\nTinerețe! Ai frumoase amintiri\n/: Tinerețe, ești mai frumoasă azi!\nNu-mi mai dăruiești lacrimi pe obraz! :/\n\n2. Tinerețe, tinerețe, oază într-un larg pustiu\nTu îmi dai senin binețe, să fiu tânăr, veșnic, viu!\nDacă gândul tău se duce spre Părintele Etern\nÎnțelegi cum de pe cruce fericirile se-aștern!\n\n3. Tinerețe, tinerețe, psalmul marilor iubiri,\nCel ce ți-a dat frumusețe, azi te cheamă din priviri.\nEl îți împlinește dorul de-L asculți în orice zi\nCăci Mesia, Salvatorul, în curând va reveni!",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "3",
            Title = "A mai trecut un an, Stăpânul vine-n vie",
            Scale = "la minor",
            Content = "1. A mai trecut un an, Stăpânul vine-n vie,\nRod în smochin El iarăși va căuta,\nDar, ca și altădată, El roade nu găsește:\n„Taie-l, Nu-l voi mai lăsa!”\n\nR1: „Nu-l voi mai lăsa, nu-l voi mai ierta!\nDe trei ani Eu roadă nu găsesc.\nTaie-l, nu mai sta, nu mai amâna!\nÎn smochin Eu roadă nu găsesc.”\n\n2. „Mai lasă-L înc-un an și e de-ajuns, Stăpâne;\nDe el, de-aproape, Mă voi ocupa.\nLa rădăcină apă iarăși Eu îi voi pune,\nIar dacă nu rodește,-l vei tăia.\n\nR2: Poate, într-o zi, iarăși va rodi\nAcest smochin ce-aproape s-a uscat.\nȘi când vei veni roadă-i vei găsi.\nMai lasă-l, o, Stăpâne minunat!”\n\n3. De-atâta vreme Domnul mai caută roadă-n tine.\nCe va găsi când iarăși va veni?\nSă ai nădejde, frate, c-atunci El în mărire,\nPe-acel care rodește-l va răpi.\n\nR3: Stăpânul va veni, cum te va găsi,\nDacă tu nu vrei să mai rodești?\nDar, în veșnicii, tu cu El vei fi\nDacă pe pământ o să-L slujești!",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "4",
            Title = "A trecut încă un an ca o frunză de toamnă",
            Scale = "Sol major",
            Content = "1. A trecut încă un an ca o frunză de toamnă,\nCa un trandafir ofilit.\nȘi, cu orice petală care cade la pământ,\nNe vorbesc că toate au un sfârșit.\n\nR: Amintirea rămâne: zile grele, zile bune,\nToate s-au dus ca un vis!\nCe-ai făcut vei avea, e alegerea ta\nDe a fi în iad sau paradis.\n\n2. A trecut încă un an, ca și roua de pe câmp,\nCa un foc din vreascuri făcut,\nCa și apa ce ușor printre degete se scurge,\nAnii grei toți au trecut.\n\n3. A trecut încă un an și un altul ne așteaptă;\nNu știm cât ni-e dat de sus.\nPoate unii-l vom sfârși, poate alții vor rămâne,\nDar dorim să fim cu Isus!",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "5",
            Title = "A trecut înc-un an ca și cum n-ar fi fost",
            Scale = "Do major",
            Content = "1. A trecut înc-un an ca și cum n-ar fi fost,\nDar în cartea din ceruri s-au scris\nFapte bune, vorbe rele, tot ce am gândit,\nPromisiuni ce nu le-am împlinit.\n\nR: Mă întorc cu durere, Părinte divin,\nȘi Te rog să îmi dai azi iertare!\nNu am fost cum ai vrut. Iartă-mă, Isus iubit!\nScrie-n dreptul meu azi: Îndurare!\n\n2. N-am cuvinte-ndeajuns, Doamne, a-Ți mulțumi\nPentru tot ce-am primit de la Tine.\nNicio clipă, niciodată nu m-ai părăsit\nTu ești drept, măreț! Slavă Ție!\n\n3. Lângă Tine, Isuse, eu vreau să rămân,\nChiar de vânturi și ploi au să vină.\nCăci, cu Tine, prin toate ies biruitor.\nCât de bine-i, Isuse, cu Tine!",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "6",
            Title = "Anii trec ca norii, perii-ncărunțesc",
            Scale = "La major",
            Content = "1. Anii trec ca norii, perii-ncărunțesc,\nVremurile-s altfel, totul e-n schimbare,\nZilnic se preface tot ce-i pământesc,\n/: Numai Adevărul este-același Soare. :/\n\n2. Vara arzi în soare, iarna arzi în ger,\nCe te-ncântă astăzi, mâine ți-e povară.\nToate-apasă duhul, toate-l strâng ca-n fier,\n/: Numai conștiința sfântă e ușoară. :/\n\n3. Toți sunt ca și tine, slabi și schimbători,\nCei aproape astăzi sunt departe mâine.\nOamenii-s ca norii, aburi trecători.\n/: Numai Domnul singur neschimbat rămâne. :/\n\n4. Iarba se usucă, frunza cade iar;\nApa-și face valuri altele întruna.\nPeste toate-n lume plânsul e-n zadar,\n/: Numai în iubire cântu-i totdeauna. :/\n\n5. Nu-ți lega de nimeni inima acum,\nCa să nu ți-o smulgă ruperea ce vine!\nLeagă-ți-o de Domnul! Lumea-i vis și fum...\n/: Singur El rămâne veșnic, lângă tine. :/",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "7",
            Title = "Anii trec rând pe rând, ei se duc vrând-nevrând",
            Scale = "Si b major",
            Content = "1. Anii trec rând pe rând, ei se duc vrând-nevrând;\nDoar o viață ți-e dată să ai.\nCum trăiești? Ce iubești? Pentru cine te jertfești?\nLui Isus cât ești gata să-i dai?\n\nR: În curând, în curând se va vedea\nDacă sus în cer ți-a fost comoara ta!\nDac-ai strâns cu Isus spice-n lan,\nVei ajunge în cerescul Canaan!\n\n2. Dacă-n anul ce s-a dus n-ai trăit cum El a spus,\nNu-i târziu să începi chiar acum.\nEl, Isus, te v-ajuta să-ți îndrepți viața ta,\nFericit să-ncepi cu El un nou drum.\n\n3. Dacă ai zăbovit și anii tăi s-au înmulțit,\nAzi, ai parte de harul Domnului.\nTânăr sau bătrân de ești, pentru cer te potrivești\nDacă-n inimă tu azi Îl primești.\n\n4. Sus, acolo-n veșnicii, unde coruri mii și mii\nVor slăvi pe Dumnezeu neîncetat,\nTu vei fi, dac-aici jos ai trăit pentru Hristos;\nLuminând, spre ceruri calea-ai arătat.",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "8",
            Title = "Anul nou de îndurare, astăzi, iarăși îl serbăm",
            Scale = "Sol major",
            Content = "1. Anul nou de îndurare, astăzi, iarăși îl serbăm!\nZi de binecuvântare, pace-n suflet noi avem.\nDomnul Sfânt ne-a sprijinit, noi acuma-I mulțumim\nDe-ndurarea ce-am primit: har bogat, binevenit.\n\nR: An de an de îndurare, pentru noi, Domnul cel sfânt,\nNe-nnoiește-a Sa lucrare de-ndurare pe pământ.\n\n2. Anul nou de îndurare Domnului I-l închinăm;\nFie pentru-a Lui onoare, lauda noastră I-o cântăm.\nNoi dorim ca pentru El să depunem tot ce-avem,\nScump, iubitul nostru zel, pe altarul Său, solemn.\n\n3. Ceas de ceas din viața noastră n-om uita-ndurarea Sa;\nFi-va mângâierea noastră, peste veac ne va purta!\nAcolo, ne-om bucura, în prezența sfinților,\nȘi-om serba, la masa Sa, anul îndurărilor.",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "9",
            Title = "Anul vechi fruntea-și apleacă și își scrie-a lui finit",
            Scale = "sol minor",
            Content = "1. Anul vechi fruntea-și apleacă și își scrie-a lui finit,\nRămânând doar amintirea din ce-ai fost, cum ai trăit.\nFila-i ultimă se-ntoarce cu un pas așa grăbit,\nDomnul astăzi ne întreabă cum și ce noi am zidit.\n\nR: Se duc anii fără să vrei,\nȘi de-au fost buni și de-au fost răi,\nRămâne doar ce ai făcut  pentru Isus.\n\n2. Azi, la cumpăna de veacuri, Domnul bate iar ușor,\nAmintindu-ți căci apusul e aproape, al tuturor.\nAmintește-ți de iubirea, grija care ți-a purtat.\nJertf-adu-I o mulțumire pentru tot ce El ți-a dat!\n\n3. Recunoaște-I bunătatea, brațu-I care te-a purtat!\nȘi pe soare, și-n furtună, Dumnezeu, El a vegheat.\nEl nu doarme, El nu tace, El aminte ia la toți\nȘi o carte scrie-n ceruri, răsplătiri să dea la toți.\n\n4. Ușa ce azi se închide nu e un sfârșit etern,\nPentru mulți, o alta nouă se deschide făr’ să vrei.\nCe stă în puterea vremii e ce-alegi cum să trăiești,\nPentru cer sau pentru fire, pe altar ca să jertfești.\n\n5. Noul an e o enigmă, e un mult necunoscut,\nSau e poate doar puținul care-i dat să-L fi avut.\nDăruiește-ți viața-ntreagă: de-i o zi sau de-i un an\nPentru Isus tu lucrează, neobosit, an după an!",
            CategoryId = "1"
        });
        songs.Add(new Song
        {
            Id = "10",
            Title = "Atât de scurtă este viața, suntem atât de trecători",
            Scale = "Do major",
            Content = "1. Atât de scurtă este viața, suntem atât de trecători\nȘi-atât de mult suntem legați de lucruri ce nu au valori.\nE timpul azi pentru trezire, veghere-n rugăciuni s-avem.\nSă nu cădem de oboseală, căci nu e mult și vine El!\n\nR: În ceruri nu va fi tristețe, durere, lacrimi nu vor fi.\nIsus Hristos împărățește și sfinții cântă imnuri vii.\nSplendori, cum n-am văzut vreodată, ne-așteaptă sus, în veșnicii;\nCu Dumnezeu vom fi de-a pururi! Cu-ardoare-aștept aceea zi!\n\n2. Necazuri, boli sunt pe pământ, dar viața-aceasta-i trecătoare.\nSă nu privim nicicând ‘napoi! Răsplata-n ceruri va fi mare!\nDar, dac-aicea vom lupta și vom trăi după Cuvânt,\nÎn ceruri vom fi-ncoronați de Însuși Mirele Preasfânt!\n\n3. Nu îmi doresc mai mult, Isuse, decât să știu că sunt al Tău,\nȘi-n ziua-aceea glorioasă să fiu și eu în raiul Tău!\nMi-e dor, mi-e dor de Tin’ Isuse! Vreau fața Ta să o privesc,\nSă nu mă mai despart de Tine și ne-ncetat să Te slăvesc!",
            CategoryId = "1"
        });

        for (var i = 11; i < 100; i++)
        {
            songs.Add(new Song
            {
                Id = i.ToString(),
                Scale = "Do minor",
                Title = $"Cantare nr {i}",
                Content = $"Cantare continut nr {i}",
                CategoryId = $"{i % 16 + 1}"
            });
        }

        return songs;
    }

    public static List<SongCategory> CreateCategories()
    {
        return
        [
            new(id: "1", title: "Cântări despre anul nou și trecerea timpului",
                icon: Icons.Material.Filled.Event),
            new(id: "2", title: "Cântări despre binecuvântarea copiilor și despre părinți",
                icon: Icons.Material.Filled.ChildCare),
            new(id: "3", title: "Cântări despre botezul în apă și venirea la pocăință",
                icon: Icons.Material.Filled.Water),
            new(id: "4", title: "Cântări despre căsătorie și dragoste",
                icon: Icons.Material.Filled.Favorite),
            new(id: "5", title: "Cântări despre Cina cea de taină și suferințele Domnului Isus",
                icon: Icons.Material.Filled.Bloodtype),
            new(id: "6", title: "Cântări despre Duhul Sfânt",
                icon: Icons.Material.Filled.FlashOn),
            new(id: "7", title: "Cântări pentru evanghelizare",
                icon: Icons.Material.Filled.Campaign),
            new(id: "8", title: "Cântări pentru mângâiere și îmbărbătare",
                icon: Icons.Material.Filled.VolunteerActivism),
            new(id: "9", title: "Cântări despre îndemn la veghere și pocăință",
                icon: Icons.Material.Filled.Lightbulb),
            new(id: "10", title: "Cântări despre înmormântare",
                icon: Icons.Material.Filled.HeartBroken),
            new(id: "11", title: "Cântări despre învierea și înălțarea Domnului",
                icon: Icons.Material.Filled.ArrowUpward),
            new(id: "12", title: "Cântări de laudă, mulțumire și bucurie",
                icon: Icons.Material.Filled.SentimentVerySatisfied),
            new(id: "13", title: "Cântări despre nașterea Domnului",
                icon: Icons.Material.Filled.StarPurple500),
            new(id: "14", title: "Cântări despre predarea în slujba lui Dumnezeu",
                icon: Icons.Material.Filled.Handshake),
            new(id: "15", title: "Cântări despre revenirea Domnului și Patria cerească",
                icon: Icons.Material.Filled.CloudSync),
            new(id: "16", title: "Cântări pentru timpul de rugăciune",
                icon: Icons.Material.Filled.SelfImprovement),
        ];
    }

    public static Song CreateSongById(string id)
    {
        var categories = CreateCategories();
        var song = CreateSongs().FirstOrDefault(entry => entry.Id == id);
        if (song != null)
        {
            song.Category = categories.First(category => category.Id == song.CategoryId);
            return song;
        }

        return new Song
        {
            Id = id,
            Title = "Din Beer-Șeba, Iacov, când pornise în călătoria lui înspre Haran",
            Category = categories[0],
            CategoryId = "1",
            Content = "1. Din Beer-Șeba, Iacov, când pornise în călătoria lui înspre Haran,\\nDupă cum Isaac îi poruncise să rămână la Padan-Aram,\\nDupă-o zi de grea călătorie, seara se lăsase și el se opri,\\nCăci lumina soarelui, cea vie, coborâtu-se spre asfințit.\\n\\nR1: Iacov își luase cu drag o piatră\\nR: Și și-o puse-astfel sub capul lui.\\nR: „O, Doamne, ce noapte minunată,\\nR: Să dormi în mijlocul pustiului!”\\n\\n2. Dar Domnul, iată, că îi arată, îl cuprinde mreaja unui vis stingher.\\nEl vede o scară minunată, de pe pământ ce ajungea la cer.\\nDar, pe-această scară ne-nsemnată, se plimbau îngerii și în sus, și-n jos,\\nÎngerii Dumnezeului cel mare. - Iată, dragii mei, ce vis frumos! -\\n\\nR2: Dar Domnul sta privind deasupra scării\\nR: Și lui Iacov îi cuvânta:\\nR: „Pământul și ce are suflare,\\nR: Toate seminției tale îi voi da.\\n\\n3. Te vei întinde la-apus și la răsărit, la miazănoapte și până la miazăzi,\\nȘi toate semințiile pământului, toate ție ți le voi dărui.”\\nȘi, sculându-se de dimineață, a luat piatra și-a fixat-o în pământ,\\nCa loc de aducere aminte pentru Dumnezeu-Acela sfânt.\\n\\nR3: Plin de frică, Iacov, atunci își zise:\\nR: „Acest loc e-așa de minunat,\\nR: Căci aici e Casa lui Dumnezeu\\nR: Și poarta cerului înstelat!”\\n\\n4. Iacov a făcut o juruință, căci dacă Domnu-n viață îl va ocroti,\\nPământul și ce are suflare, lui Dumnezeu Îi va dărui.\"",
            Scale = "Mi major"
        };
    }

    public static Settings CreateDefaultSettings(string contact)
    {
        return new Settings
        {
            FontSize = 20,
            FontFamily = "INTER",
            ApplicationColor = ApplicationColor.Automatic,
            Contact = contact,
            LastSynchronized = DateTime.UtcNow,
        };
    }

    public static List<string> CreateFontFamilies()
    {
        return ["INTER", "Arial"];
    }
}
