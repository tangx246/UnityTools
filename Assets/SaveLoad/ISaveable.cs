using Newtonsoft.Json;

public interface ISaveable
{
    string ToJson()
    {
        return JsonConvert.SerializeObject(Save(), GetSettings());
    }

    void FromJson(string saveState)
    {
        Load(JsonConvert.DeserializeObject(saveState, GetSettings()));
    }

    private JsonSerializerSettings GetSettings()
    {
        return new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
    }

    public object Save();
    public void Load(object saveData);
}