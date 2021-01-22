using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Shopkeeper.Crafting;

namespace Shopkeeper.World
{
    public class GameState
    {
        private SceneLookup sceneLookup;

        public GameState()
        {
            sceneLookup = SceneLookup.LoadFromResources();
        }

        public void StartGame()
        {
            SaveSystem.LoadState(WorldContext.PlayerState);
            SceneManager.LoadScene(sceneLookup.Main);
        }
        public void StartMine()
        {
            OpenScene(sceneLookup.Mine);
        }
        public void EndDay()
        {
            OpenScene(sceneLookup.EndOfDay);
        }
        public void StartDay()
        {
            OpenScene(sceneLookup.Main);
        }


        private static void OpenScene(string scene)
        {
            SaveSystem.SaveState(WorldContext.PlayerState);
            SceneManager.LoadScene(scene);
        }
    }

    public static class SaveSystem
    {
        const string filePath = "save.sav";
        readonly static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new CraftingMaterialsConverter(),
                new ItemConverter()
            },
            Formatting = Formatting.Indented
        };

        public static void LoadState(PlayerState state)
        {
            string path = Path.Combine(Application.persistentDataPath, filePath);
            if (!File.Exists(path))
                return;

            Debug.Log($"Loading data from {path}");
            string json = File.ReadAllText(path);

            JsonConvert.PopulateObject(json, state, settings);
        }

        public static void SaveState(PlayerState state)
        {
            string path = Path.Combine(Application.persistentDataPath, filePath);
            string json = JsonConvert.SerializeObject(state, settings);

            File.WriteAllText(path, json);
            Debug.Log($"Saved data to {path}");
        }
    }

    public class CraftingMaterialsConverter : JsonConverter<CraftingMaterials>
    {
        private readonly ItemDatabase database;

        public CraftingMaterialsConverter()
        {
            database = ItemDatabase.LoadFromResources();
        }

        public override CraftingMaterials ReadJson(JsonReader reader, Type objectType, CraftingMaterials existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var materials = new CraftingMaterials();

            while(true)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                Item item = database.Get((string)reader.Value);
                reader.Read();
                int value = (int)(long)reader.Value;

                materials.Add(item);
                materials[item] = value;
            }

            return materials;
        }

        public override void WriteJson(JsonWriter writer, CraftingMaterials value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var item in value.AllValues)
            {

                writer.WritePropertyName(item.Key.Name);
                writer.WriteValue(item.Value);
            }
            writer.WriteEndObject();
        }
    }

    public class ItemConverter : JsonConverter<Item>
    {
        private readonly ItemDatabase database;

        public ItemConverter()
        {
            database = ItemDatabase.LoadFromResources();
        }

        public override Item ReadJson(JsonReader reader, Type objectType, Item existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return database.Get(reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, Item value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Name);
        }
    }

}
