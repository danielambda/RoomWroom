namespace RoomWroom.CommandHandling
{
    public record ResponseUnit(string? Text, IEnumerable<IEnumerable<ResponseCallbackButton>>? ResponseCallbackButtons = null)
    {
        public ResponseUnit(string text, IEnumerable<ResponseCallbackButton> responseCallbackButtons)
            : this(text, [responseCallbackButtons]) { }
        
        public ResponseUnit(string text, ResponseCallbackButton responseCallbackButton)
            : this(text, [[responseCallbackButton]]) { }

        public static implicit operator ResponseUnit(string text) => new(text);
        
        public override string? ToString() => Text;
    }
}
