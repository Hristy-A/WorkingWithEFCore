namespace Store.Data.Entities
{
    public class EventTypeInfo
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected EventTypeInfo() { }

        public EventTypeInfo(EventType eventType)
        {
            TypeId = (int)eventType;
            Name = eventType.ToString();
            Description = eventType.GetDescriptionIfExsists();
        }
    }
}
