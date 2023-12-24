namespace RoomWroom.CommandHandling
{
    public record ResponseUnit(string Text, IEnumerable<ResponseCallbackButton>? ResponseCallbackButtons)
    {
        public static implicit operator ResponseUnit(string text) => new(text, null);
        
        public override string ToString() => Text;
    }
}
