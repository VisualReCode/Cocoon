using MessagePack;

namespace SharedStuff
{
    [MessagePackObject]
    public class Foo
    {
        [Key(0)]
        public string Bar { get; set; }
    }
}