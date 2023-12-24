namespace RoomWroom.CommandHandling
{
    public class Response
    {
        public IEnumerable<ResponseUnit> ResponseUnits { get; init; }

        public Response(IEnumerable<ResponseUnit> responseUnits)
        {
            ResponseUnits = responseUnits;
        }

        public Response(ResponseUnit responseUnit)
        {
            ResponseUnits = [responseUnit];
        }

        public static implicit operator Response(string text) => new(new ResponseUnit(text, null));
    }
}
