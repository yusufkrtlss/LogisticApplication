namespace LogisticMVC.DTOs.Module
{
    public class GetAllModulesDTO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        

        public class Root
        {
            public string id { get; set; }
            public string moduleName { get; set; }
            public string moduleNameEn { get; set; }
            public string parentId { get; set; }
            public int screenOrder { get; set; }
        }

    }
}
