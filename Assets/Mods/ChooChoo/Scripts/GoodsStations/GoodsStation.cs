using System;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.BuildingsBlocking;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;
using Timberborn.Persistence;

namespace ChooChoo.GoodsStations
{
    public class GoodsStation : BaseComponent, IPersistentEntity, IFinishedStateListener, IRegisteredComponent, IPausableComponent
    {
        private static readonly ComponentKey GoodsStationKey = new(nameof(GoodsStation));
        private static readonly PropertyKey<string> StationNameKey = new(nameof(StationName));

        public static readonly int Capacity = 200;

        private GoodsStationReceivingInventory _goodsStationReceivingInventory;
        private GoodsStationSendingInventory _goodsStationSendingInventory;

        public string StationName { get; set; } = GetRandomStation();

        public void Awake()
        {
            _goodsStationReceivingInventory = GetComponentFast<GoodsStationReceivingInventory>();
            _goodsStationSendingInventory = GetComponentFast<GoodsStationSendingInventory>();
            enabled = false;
        }

        public void Save(IEntitySaver entitySaver)
        {
            var component = entitySaver.GetComponent(GoodsStationKey);
            if (string.IsNullOrEmpty(StationName))
                return;
            component.Set(StationNameKey, StationName);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(GoodsStationKey))
                return;
            var component = entityLoader.GetComponent(GoodsStationKey);
            if (!component.Has(StationNameKey))
                return;
            StationName = component.Get(StationNameKey);
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            SendingInventory.Enable();
            ReceivingInventory.Enable();
        }

        public void OnExitFinishedState()
        {
            SendingInventory.Disable();
            ReceivingInventory.Disable();
            enabled = false;
        }

        public Inventory SendingInventory => _goodsStationSendingInventory.Inventory;

        public Inventory ReceivingInventory => _goodsStationReceivingInventory.Inventory;

        private static string GetRandomStation()
        {
            var stationNames = new[]
            {
                "Amsterdam Centraal", "Rotterdam Centraal", "Utrecht Centraal", "Den Haag Centraal", "Schiphol Airport",
                "Paris Gare du Nord", "Paris Gare de Lyon", "Lyon Part-Dieu", "Marseille Saint-Charles", "Nice Ville",
                "London Waterloo", "London Paddington", "London Victoria", "Manchester Piccadilly", "Birmingham New Street",
                "Berlin Hauptbahnhof", "Munich Hauptbahnhof", "Hamburg Hauptbahnhof", "Frankfurt (Main) Hauptbahnhof", "Stuttgart Hauptbahnhof",
                "Rome Termini", "Milano Centrale", "Napoli Centrale", "Florence Santa Maria Novella", "Venice Santa Lucia",
                "Madrid Atocha", "Barcelona Sants", "Valencia Joaquín Sorolla", "Sevilla Santa Justa", "Malaga Maria Zambrano",
                "Tokyo Station", "Shinjuku Station", "Shibuya Station", "Kyoto Station", "Osaka Station",
                "New York Penn Station", "Grand Central Terminal", "Union Station (Chicago)", "Union Station (Los Angeles)", "Boston South Station",
                "Toronto Union Station", "Vancouver Pacific Central Station", "Montreal Central Station", "Ottawa Station", "Quebec City Gare du Palais",
                "Beijing Railway Station", "Shanghai Hongqiao Railway Station", "Guangzhou South Railway Station", "Chengdu East Railway Station", "Wuhan Railway Station",
                "Sydney Central", "Melbourne Southern Cross", "Brisbane Roma Street", "Perth Station", "Adelaide Station",
                "Moscow Leningradsky", "Saint Petersburg Moskovsky", "Kazan Railway Station", "Novosibirsk Glavny", "Vladivostok Railway Station",
                "Delhi Junction", "Mumbai Central", "Howrah Junction (Kolkata)", "Chennai Central", "Bangalore City Railway Station",
                "Istanbul Sirkeci", "Ankara Gar", "Izmir Basmane", "Bursa Terminal", "Antalya Station",
                "Johannesburg Park Station", "Cape Town Station", "Durban Station", "Pretoria Station", "Port Elizabeth Station",
                "São Paulo Luz", "Rio de Janeiro Central do Brasil", "Brasilia Station", "Salvador Station", "Fortaleza Station",
                "Buenos Aires Retiro", "Córdoba Mitre", "Rosario Norte", "Mendoza Station", "La Plata Station",
                "Lisbon Santa Apolonia", "Porto Campanha", "Coimbra B", "Braga Station", "Faro Station",
                "Zurich Hauptbahnhof", "Geneva Cornavin", "Bern Station", "Basel SBB", "Lausanne Station",
                "Vienna Hauptbahnhof", "Salzburg Hauptbahnhof", "Innsbruck Hauptbahnhof", "Linz Hauptbahnhof", "Graz Hauptbahnhof",
                "Oslo Central Station", "Bergen Station", "Trondheim Central", "Stavanger Station", "Kristiansand Station",
                "Stockholm Central", "Gothenburg Central", "Malmö Central", "Uppsala Central", "Linköping Central",
                "Helsinki Central", "Tampere Station", "Turku Central", "Oulu Station", "Kuopio Station",
                "Brussels Midi", "Antwerp Central", "Ghent-Saint-Peter's", "Bruges Station", "Liege-Guillemins",
                "Luxembourg Gare Centrale", "Esch-sur-Alzette Station", "Differdange Station", "Ettelbruck Station", "Bettembourg Station",
                "Leiden Centraal", "Groningen Station", "Maastricht Station", "Arnhem Centraal", "Zwolle Station",
                "Lille Europe", "Bordeaux Saint-Jean", "Strasbourg Station", "Nantes Station", "Toulouse Matabiau",
                "Edinburgh Waverley", "Glasgow Central", "Leeds Station", "Liverpool Lime Street", "Cardiff Central",
                "Vienna Westbahnhof", "Graz Hauptbahnhof", "Villach Hauptbahnhof", "Bregenz Station", "Klagenfurt Hauptbahnhof",
                "Zurich Stadelhofen", "Luzern Station", "Interlaken West", "St. Moritz Station", "Zermatt Station",
                "Prague Main Station", "Brno hlavní nádraží", "Ostrava-Svinov", "Pilsen hlavní nádraží", "České Budějovice Station",
                "Budapest Keleti", "Debrecen Station", "Pécs Station", "Győr Station", "Szeged Station",
                "Warsaw Central", "Kraków Główny", "Gdańsk Główny", "Łódź Fabryczna", "Wrocław Główny",
                "Lisbon Oriente", "Lisbon Rossio", "Cascais Station", "Sintra Station", "Aveiro Station",
                "Helsinki Pasila", "Espoo Station", "Jyväskylä Station", "Lappeenranta Station", "Vantaa Tikkurila",
                "Oslo S", "Bodø Station", "Drammen Station", "Tønsberg Station", "Lillestrøm Station",
                "Copenhagen H", "Odense Station", "Aarhus H", "Aalborg Station", "Esbjerg Station",
                "Stockholm City", "Umeå Central", "Sundsvall Central", "Karlstad Station", "Helsingborg Central",
                "Athens Larissa", "Thessaloniki Station", "Patras Station", "Heraklion Station", "Piraeus Station",
                "Beijing South", "Xi'an North", "Shenzhen North", "Tianjin Railway Station", "Chongqing North",
                "Seoul Station", "Busan Station", "Incheon Station", "Daejeon Station", "Daegu Station",
                "Mexico City Buenavista", "Guadalajara Station", "Monterrey Station", "Mérida Station", "Tijuana Station",
                "São Paulo Júlio Prestes", "Brasília Station", "Recife Station", "Porto Alegre Station", "Curitiba Station",
                "Buenos Aires Constitución", "Rosario Sur", "San Miguel de Tucumán Station", "Bahía Blanca Station", "Mar del Plata Station",
                "Istanbul Haydarpaşa", "Eskişehir Station", "Konya Station", "Adana Station", "Kayseri Station",
                "Jerusalem Yitzhak Navon", "Tel Aviv HaShalom", "Haifa Hof HaCarmel", "Be'er Sheva Center", "Ashdod Station"
            };

            var random = new Random();
            var index = random.Next(stationNames.Length);
            return stationNames[index];
        }
    }
}