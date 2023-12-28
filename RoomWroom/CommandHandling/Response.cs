namespace RoomWroom.CommandHandling
{
    public record Response (IEnumerable<ResponseUnit> ResponseUnits)
    {
        public Response(ResponseUnit responseUnit) : this ([responseUnit]) { }
        
        public static implicit operator Response(string text) => new(new ResponseUnit(text));
    }
}
